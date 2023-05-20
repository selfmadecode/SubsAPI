using Newtonsoft.Json;
using SubsAPI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SubsAPI.DTO
{
    public class SubscribeDto
    {
        [JsonProperty("token")]
        [Required]
        [TokenValidation]
        public string Token { get; set; }

        [JsonProperty("service_id")]
        [Required]
        public string ServiceId { get; set; }
        
        [JsonProperty("phone_number")]
        [Required]
        public string PhoneNumber { get; set; }
    }

    public class UnSubscribeDto : SubscribeDto
    {

    }
}
