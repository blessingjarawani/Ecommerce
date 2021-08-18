using BoookStoreDatabase2.BLL.Infrastructure.Shared.Config;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models;
using BoookStoreDatabase2.BLL.Models.DTO;
using BoookStoreDatabase2.BLL.ViewModels;
using Ecommerce.BLL.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BoookStoreDatabase2.BLL.Infrastructure.Shared.Services
{
    public class ApplicationUsersService : IApplicationUsersService

    {
        private readonly IApplicationUsersRepository _repo;
        private readonly AppSettings _appSettings;
        private readonly ILogger<ApplicationUsersService> _logger;

        public ApplicationUsersService(IApplicationUsersRepository repo, IOptions<AppSettings> appSettings, ILogger<ApplicationUsersService> logger)
        {
            _repo = repo;
            _appSettings = appSettings.Value;
            _logger = logger;
        }


        public async Task<ObjectResponse<AuthenticateResponse>> Authenticate(LoginViewModel request)
        {
            try
            {

                var user = await _repo.Login(request);
                if (!user.Succeeded)
                {
                    return new ObjectResponse<AuthenticateResponse> { Success = false, Message = "Invalid User" };
                }

                var login = await _repo.GetUserByUserName(request.UserName);

                var token = generateJwtToken(login);
                var authResponse = new AuthenticateResponse(login, token);

                return new ObjectResponse<AuthenticateResponse> { Success = true, Data = authResponse };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message);
                return new ObjectResponse<AuthenticateResponse> { Success = false, Message = ex.GetBaseException().Message };
            }

        }
        public async Task<ObjectResponse<List<ApplicationUsersDTO>>> GetAllUsers()
        {
            try
            {
                var result = await _repo.GetAll();
                return new ObjectResponse<List<ApplicationUsersDTO>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ObjectResponse<List<ApplicationUsersDTO>> { Success = false, Message = ex.GetBaseException().Message };
            }
        }

        private string generateJwtToken(UserDTO user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task LockOutUser(string userId)
        {
            await _repo.LockOutUser(userId);
        }
        public async Task<ObjectResponse<bool>> Register(RegisterUserViewModel request)
        {
            try
            {

                var user = await _repo.Register(request);
                if (user == null || !user.Succeeded)
                {
                    return new ObjectResponse<bool> { Success = false, Message = "Failed To Register User" };
                }

                return new ObjectResponse<bool> { Success = true, Data = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message);
                return new ObjectResponse<bool> { Success = false, Message = ex.GetBaseException().Message };
            }
        }


        public async Task<ObjectResponse<bool>> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            try
            {
                var result = await _repo.ChangePassword(changePasswordViewModel);
                return result ? new ObjectResponse<bool> { Success = true } :
                 new ObjectResponse<bool> { Success = false, Message = "Password change failed" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message);
                return new ObjectResponse<bool> { Success = false, Message = ex.GetBaseException().Message };
            }

        }
    }

}

