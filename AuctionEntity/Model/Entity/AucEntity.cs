using AuctionEntity.DTO.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionEntity.Entity
{
    public class AucEntity 
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid IdOwner { get; set; }
        public string Discription { get; set; }
        public Guid IdLote { get; set; }

        public Lote Lote { get; set; }


        public static AucEntity ConvertToAucEntity(AuctionDTO auctionDTO)
        {
            return new AucEntity()
            {
                StartTime = DateTime.Now,
                Id = auctionDTO.Id,
                Name = auctionDTO.Name,
                EndTime = auctionDTO.EndTime,
                IdLote = auctionDTO.IdLote,
                IdOwner = auctionDTO.IdOwner,
                Discription = auctionDTO.Discription,
            };
        }

    }
}
