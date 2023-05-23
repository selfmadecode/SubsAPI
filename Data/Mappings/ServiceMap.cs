using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubsAPI.Entities;
using SubsAPI.Helpers;
using System;
using System.Collections.Generic;

namespace SubsAPI.Data.Mappings
{
    public class ServiceMap : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            SetUpServiceAccount(builder);
        }

        public void SetUpServiceAccount(EntityTypeBuilder<Service> builder)
        {
            var ServiceAccount1 = new Service
            {
                Id = "subsadmin@gmail.com",
                Password = "SubsAdmin1".Encrypt(),
                DateCreated = DateTime.UtcNow
            };

            var ServiceAccount2 = new Service
            {
                Id = "subsadmin1@gmail.com",
                Password = "SubsAdmin2".Encrypt(),
                DateCreated = DateTime.UtcNow
            };

            var SeriveAccounts = new List<Service> { ServiceAccount1, ServiceAccount2 };
            builder.HasData(SeriveAccounts);
        }
    }
}