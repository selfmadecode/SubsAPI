using SubsAPI.Models.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace SubsAPI.Entities
{
    public class Service
    {
        [Key]
        public string Id { get; set; } // serviceId
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
