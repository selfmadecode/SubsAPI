using SubsAPI.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SubsAPI.Models
{
    public class UserToken
    {
        [Key]
        public int Id { get; set; }

        public Service Service { get; set; }
        public string ServiceId { get; set; }

        public string Token { get; set; }

        public DateTime Expiration { get; set; }

    }
}
