using Microsoft.EntityFrameworkCore;
using SubsAPI.Data;
using SubsAPI.DTO;
using SubsAPI.Helpers;
using SubsAPI.Models;
using SubsAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubsAPI.Services
{
    public class TokenConfigService : BaseService, ITokenConfigService
    {
        private readonly ApplicationDbContext _dbContext;

        public TokenConfigService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponse<int>> CreateOrUpdateTokenLenght(UpsertToken createToken)
        {
            var token = await _dbContext.TokenExpiration.FirstOrDefaultAsync();

            if(token == null)
            {
                var newToken = new TokenExpiration
                {
                    Hours = createToken.Hours
                };

               await _dbContext.TokenExpiration.AddAsync(newToken);
            }
            else
            {
                token.Hours = createToken.Hours;
                _dbContext.TokenExpiration.Update(token);
            }

            await _dbContext.SaveChangesAsync();
            return new BaseResponse<int>(token.Hours, "Token expiration Updated");
        }

        public async Task<BaseResponse<int>> GetTokenLenght()
        {
            var token = await _dbContext.TokenExpiration.FirstOrDefaultAsync();

            if(token == null)
            {
                Errors.Add("Token not found");
                return new BaseResponse<int>("Token not found", Errors);
            }

            return new BaseResponse<int>(token.Hours);
        }
    }
}
