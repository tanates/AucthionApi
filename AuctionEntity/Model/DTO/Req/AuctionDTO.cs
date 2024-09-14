using AuctionEntity.Entity;
using AuctionEntity.Model.DTO;
using System.Net.Http.Headers;

namespace AuctionEntity.DTO.Req
{
    public class AuctionDTO 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime EndTime { get; set; }
        public Guid IdOwner { get; set; }
        public string Discription { get; set; }
        public Guid IdLote { get; set; }

        public static AuctionDTO ConvertToAuctionDTO(AucEntity entity)
        {
            return new AuctionDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                EndTime = entity.EndTime,
                IdOwner = entity.IdOwner,
                Discription = entity.Discription,
                IdLote = entity.IdLote
            };
        }



    }
}