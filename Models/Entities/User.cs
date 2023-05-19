using SubsAPI.Models.Enum;
using System;

namespace SubsAPI.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string ServiceId { get; set; }

        public string Password { get; set; }

        public DateTime DateCreated { get; set; }

    }
}
