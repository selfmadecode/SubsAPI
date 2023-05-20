using Microsoft.AspNetCore.Http;
using SubsAPI.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SubsAPI.Helpers
{
    public class TokenValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _dbContext = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

            var token = value?.ToString();
            var serviceId = validationContext.ObjectInstance.GetType()
                .GetProperty("ServiceId")?.GetValue(validationContext.ObjectInstance)
                ?.ToString();

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(serviceId))
                return new ValidationResult("Wrong Token ID"); // Return error message if token or service ID is not provided


            var currentDateTime = DateTime.Now;

            var tokenData = _dbContext.UserTokens.FirstOrDefault(t => t.Token == token && t.ServiceId == serviceId);

            if (tokenData == null)
                return new ValidationResult("Invalid token or service ID"); // Return error message if token or service ID is invalid


            if (tokenData.Expiration < currentDateTime)
                return new ValidationResult("Token ID expired"); // Return error message if token has expired


            return ValidationResult.Success; // Return success if token is valid and not expired
        }
    }
}
