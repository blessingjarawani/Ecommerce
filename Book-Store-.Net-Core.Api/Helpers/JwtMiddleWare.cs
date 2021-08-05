using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_.Net_Core.Api.Helpers
{
    public class JwtMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;
        private readonly ILogger<JwtMiddleWare> _logger;

        public JwtMiddleWare(RequestDelegate next, IOptions<AppSettings> appSettings, ILogger<JwtMiddleWare> logger)
        {
            _next = next;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await AttachUserToContext(context, token);

            await _next(context);
        }

        private async Task AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // attach user to context on successful jwt validation
                var user = await userService.GetById(userId);
                currentUser.Set(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
