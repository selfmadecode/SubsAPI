using SubsAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubsAPI.Models
{
    public class UserToken
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

    }
}
