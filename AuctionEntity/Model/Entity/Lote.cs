using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionEntity.Entity
{
    public class Lote
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public float StartPrice { get; set; }
        public float EndPrice { get; set; } = 0;
        public Guid IdAuctione  { get; set; }
        public AucEntity Auctione  { get; set; }
    } 
}
