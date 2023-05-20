using Microsoft.EntityFrameworkCore;
using SubsAPI.Data;
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
    }
}
