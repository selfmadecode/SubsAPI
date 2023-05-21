using Newtonsoft.Json;
using SubsAPI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SubsAPI.DTO
{
    public class SubscribeDto
    {
        [JsonPropertyName("token")]
        [Required(ErrorMessage = "Token required")]
        [TokenValidation]
        public string Token { get; set; }

        [JsonPropertyName("service_id")]
        [Required(ErrorMessage = "Service Id required")]
        public string ServiceId { get; set; }
        
        [JsonPropertyName("phone_number")]
        [Required(ErrorMessage = "Phone number required")]
        [Phone]
        public string PhoneNumber { get; set; }
    }

    public class UnSubscribeDto : SubscribeDto
    {

    }
    
    public class CheckSubscriptionStatusDto : SubscribeDto
    {

    }
}
