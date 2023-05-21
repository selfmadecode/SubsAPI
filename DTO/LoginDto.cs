using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SubsAPI.DTO
{
    public class LoginDto
    {
        [JsonPropertyName("service_id")]
        [Required]
        public string ServiceId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
