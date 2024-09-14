using AuctionEntity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AuctionEntity.Model.DTO.Req
{
    public  class LoteDTO
    {
 
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public float  StartPrice { get; set; }
        public float EndPrice { get; set; } = 0;
        public Guid  IdAuctione { get; set; }

        public static LoteDTO ConvertToLoteDTO(Lote lote)
        {
            return new LoteDTO
            {
                Description = lote.Description,
                EndPrice = lote.EndPrice,
                Id = lote.Id,
                IdAuctione = lote.IdAuctione,
                StartPrice = lote.StartPrice,
                Name = lote.Name,
            };
        }
    }
}
