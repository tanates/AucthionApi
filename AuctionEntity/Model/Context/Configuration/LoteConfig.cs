using AuctionEntity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AuctionEntity.Model.Context.Configuration
{
    public class LoteConfig : IEntityTypeConfiguration<Lote>
    {
        public void Configure(EntityTypeBuilder<Lote> builder)
        {
            builder.HasOne(l => l.Auctione)
              .WithOne(a => a.Lote)
              .HasForeignKey<Lote>(l => l.IdAuctione);
        }

       


    }
}
