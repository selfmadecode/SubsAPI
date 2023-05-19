using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SubsAPI.Data;
using SubsAPI.DTO;
using SubsAPI.Entities;
using SubsAPI.Helpers;
using SubsAPI.Models;
using SubsAPI.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SubsAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;


        public AuthService(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task<BaseResponse<JwtResponseDTO>> Login(LoginDto model)
        {
            var result = new BaseResponse<JwtResponseDTO>();

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.ServiceId == model.ServiceId);

            // Incorrect password
            if (user == null || user.Password.Decrypt() != model.Password)
            {
                result.Errors.Add("ServiceId or Password incorrect");
                return result;
            }
            
            var data = await GenerateToken(user);

            result.Data = data;
            return result;
        }

        private async Task<JwtResponseDTO> GenerateToken(User user)
        {
            var userToken = await _dbContext.UserTokens.FirstOrDefaultAsync(x => x.UserId == user.Id);

            string token = "";
            DateTime expiration;

            if (userToken == null)
            {
                (token, expiration) = await CreateJwtTokenAsync(user);

                await _dbContext.UserTokens.AddAsync(new UserToken
                {
                    Token = token,
                    Expiration = expiration,
                    UserId = user.Id
                });
            }
            else if (userToken.Expiration < DateTime.Now) // has expired
            {
                (token, expiration) = await CreateJwtTokenAsync(user);

                userToken.Expiration = expiration;
                userToken.Token = token;

                _dbContext.UserTokens.Update(userToken);
            }
            else
            {
                token = userToken.Token;
                expiration = userToken.Expiration;
            }

            await _dbContext.SaveChangesAsync();

            return new JwtResponseDTO
            {
                Token = token,
                Expiration = expiration
            };
        }

        private async Task<(string, DateTime)> CreateJwtTokenAsync(User user)
        {

            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);

            var userClaims = await BuildUserClaims(user);

            var signKey = new SymmetricSecurityKey(key);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.GetSection("JWT:ValidIssuer").Value,
                notBefore: DateTime.UtcNow,
                audience: _configuration.GetSection("JWT:ValidAudience").Value,
                expires: DateTime.UtcNow.AddHours(_dbContext.TokenExpiration.First().Hours),
                claims: userClaims,
                signingCredentials: new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256));

            return (new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken), jwtSecurityToken.ValidTo);
        }

        private async Task<List<Claim>> BuildUserClaims(User user)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.ServiceId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            
            return userClaims;
        }
    }
}
