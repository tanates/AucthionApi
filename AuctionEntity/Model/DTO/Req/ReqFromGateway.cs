
using AuctionEntity.Entity;
using AuctionEntity.Model.DTO;
using AuctionEntity.Model.DTO.Req;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionEntity.DTO.Req
{
    public class ReqFromGateway
    {

        public Dictionary<string, object> Data { get; set; }
        public AuctionDTO? AuctionDTO { get; set; }
        public LoteDTO? LoteDTO { get; set; }


    }
}
