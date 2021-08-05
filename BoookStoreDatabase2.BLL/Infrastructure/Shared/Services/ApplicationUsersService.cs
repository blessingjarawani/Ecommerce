﻿using BoookStoreDatabase2.BLL.Infrastructure.Shared.Config;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models;
using BoookStoreDatabase2.BLL.Models.DTO;
using BoookStoreDatabase2.BLL.ViewModels;
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
    public class ApplicationUsersService :IApplicationUsersService

    {
        private readonly IApplicationUsersRepository _repo;
        private readonly AppSettings _appSettings;
        public ApplicationUsersService(IApplicationUsersRepository repo, IOptions<AppSettings> appSettings)
        {
            _repo = repo;
            _appSettings = appSettings.Value;
        }


        public async Task<Response<AuthenticateResponse>> Authenticate(LoginViewModel request)
        {
            try
            {
              
                var user = await _repo.Login(request);
                if (!user.Succeeded)
                { 
                    return new Response<AuthenticateResponse> { Success = false, Message = "Invalid User"};
                }

                var login = new UserDTO
                {
                    UserName = user..UserName,
                    FirstName = result.FirstName,
                    UserId = result.Id,
                    LastName = result.LastName,
                    UserRole = result.UserRole
                };

                var token = generateJwtToken(user);

                return new AuthenticateResponse(user, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }

        }
        public Task<Response<List<ApplicationUsersDTO>>> GetAllUsers()
        {
            throw new NotImplementedException();
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
    }
}