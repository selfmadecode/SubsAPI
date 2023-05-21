using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SubsAPI.Data;
using SubsAPI.DTO;
using SubsAPI.Entities;
using SubsAPI.Helpers;
using SubsAPI.Models;
using System;
using System.Threading.Tasks;

namespace SubsAPI.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly TokenLenght _tokenLenght;

        public AuthService(ApplicationDbContext dbContext, IConfiguration configuration,
            IOptions<TokenLenght> tokenLenght)
        {
            _dbContext = dbContext;
            _tokenLenght = tokenLenght.Value;
        }
        public async Task<BaseResponse<JwtResponseDTO>> Login(LoginDto model)
        {
            var result = new BaseResponse<JwtResponseDTO>();

            var service = await _dbContext.Services
                .FirstOrDefaultAsync(x => x.Id == model.ServiceId);

            // Incorrect password
            if (service == null || service.Password.Decrypt() != model.Password)
            {
                Errors.Add("ServiceId or Password incorrect");
                result.ResponseMessage = "ServiceId or Password incorrect";
                return new BaseResponse<JwtResponseDTO>(result.ResponseMessage, Errors);
            }
            
            var data = await GenerateToken(service);

            result.Data = data;
            return result;
        }

        private async Task<JwtResponseDTO> GenerateToken(Service service)
        {
            var userToken = await _dbContext.UserTokens.FirstOrDefaultAsync(x => x.ServiceId == service.Id);

            string token = "";
            DateTime expiration;

            if (userToken == null)
            {
                (token, expiration) = await CreateTokenAsync(_tokenLenght.AuthToken, _dbContext);

                await _dbContext.UserTokens.AddAsync(new UserToken
                {
                    Token = token,
                    Expiration = expiration,
                    ServiceId = service.Id
                });
                
            }
            else if (userToken.Expiration < DateTime.UtcNow) // has expired
            {
                (token, expiration) = await CreateTokenAsync(_tokenLenght.AuthToken, _dbContext);

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
    }
}
