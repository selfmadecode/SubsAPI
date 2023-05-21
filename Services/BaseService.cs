using Microsoft.EntityFrameworkCore;
using SubsAPI.Data;
using SubsAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubsAPI.Services
{
    public class BaseService
    {
        private static Random random = new Random();
        public List<string> Errors { get; set; } = new List<string>();

        public string GenerateSubscriptionId(int length)
        {
            return GenerateRandomToken(length);
        }

        public async Task<(string, DateTime)> CreateTokenAsync(int lenght, ApplicationDbContext dbContext)
        {
            var subscriptionId = GenerateRandomToken(lenght);

            int hours = 8;

            var expirationHour = await dbContext.TokenExpiration.FirstOrDefaultAsync();

            if (expirationHour != null)
            {
                hours = expirationHour.Hours;
            }

            var expiration = DateTime.UtcNow.AddHours(hours);

            return (subscriptionId, expiration);
        }

        private string GenerateRandomToken(int lenght)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            var subscriptionId = new string(Enumerable.Repeat(chars, lenght)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return subscriptionId;
        }

        public async Task<BaseResponse<bool>> IsTokenValid(string token, string serviceId,  ApplicationDbContext _dbContext)
        {
            var result = new BaseResponse<bool>();

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(serviceId))
            {
                Errors.Add("Token or ServiceId is empty");
                result.ResponseMessage = "Token or ServiceId is empty";
                result.Data = false;

                return new BaseResponse<bool>(result.ResponseMessage, Errors);
            }

            var currentDateTime = DateTime.Now;

            var tokenData = await _dbContext.UserTokens.FirstOrDefaultAsync(t => t.Token == token && t.ServiceId == serviceId);

            if (tokenData == null)
            {
                Errors.Add("Token or ServiceId not found");
                result.ResponseMessage = "Token or ServiceId not found";
                result.Data = false;

                return new BaseResponse<bool>(result.ResponseMessage, Errors);
            }

            if (tokenData.Expiration < currentDateTime)
            {
                Errors.Add("Token has expired");
                result.ResponseMessage = "Token has expired";
                result.Data = false;

                return new BaseResponse<bool>(result.ResponseMessage, Errors);
            }

            result.Data = true;
            return result;
        }
    }
}
