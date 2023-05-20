using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubsAPI.Data.Mappings
{
    public class TokenExpirationMap : IEntityTypeConfiguration<TokenExpiration>
    {
        public void Configure(EntityTypeBuilder<TokenExpiration> builder)
        {
            var tokenConfig = new TokenExpiration
            {
                Hours = 8,
                Id = 1
            };

            builder.HasData(tokenConfig);
        }
    }
}
