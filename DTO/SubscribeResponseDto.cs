using SubsAPI.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubsAPI.DTO
{
    public class SubscribeResponseDto
    {
        public string SubscriptionId { get; set; }
        public string ServiceId { get; set; }
        public string PhoneNumber { get; set; }
        public SubscriptionStatus Status { get; set; }
        public DateTime DateSubscribed { get; set; }
    }

    public class UnSubscribeResponseDto
    {
        public string ServiceId { get; set; }
        public string PhoneNumber { get; set; }
        public SubscriptionStatus Status { get; set; }
        public DateTime? DateUnSubscribed { get; set; }
    }
}
