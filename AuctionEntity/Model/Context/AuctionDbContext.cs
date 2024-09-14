using AuctionEntity.Entity;
using AuctionEntity.Model.Context.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionEntity.Model.Context
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AucEntity> Auction{ get; set; }
        public DbSet<Lote> Lotes{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Применение конфигурации из LoteConfig
            modelBuilder.ApplyConfiguration(new AucConfig());
            modelBuilder.ApplyConfiguration(new LoteConfig());
        }
    }
}
