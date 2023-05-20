using SubsAPI.Entities;
using SubsAPI.Models.Enum;
using System;

namespace SubsAPI.Models
{
    public class Subscriber
    {
        public int Id { get; set; }

        public Service Service { get; set; }
        public string ServiceId { get; set; }

        public string SubscriptionId { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime SubscribeDate { get; set; }

        public DateTime? UnsubscribeDate { get; set; }

        public SubscriptionStatus Status { get; set; }
    }
}
