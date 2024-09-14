using AuctionEntity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionEntity.Model.Context.Configuration
{
    public class AucConfig : IEntityTypeConfiguration<AucEntity>
    {
        public void Configure(EntityTypeBuilder<AucEntity> builder)
        {
            builder.HasOne(l => l.Lote)
              .WithOne(a => a.Auctione)
              .HasForeignKey<AucEntity>(l => l.IdLote);
        }
    }
}
