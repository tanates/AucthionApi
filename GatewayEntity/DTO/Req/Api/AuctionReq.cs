using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayEntity.DTO.Req.Api
{
    public class AuctionReq
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime EndTime { get; set; }
        public Guid IdOwner { get; set; }
        public string Discription { get; set; }
        public Guid IdLote { get; set; }

        public AuctionReq Get<T>(T model)
        {
            if (model == null) throw new ArgumentNullException("model");
            if(model is object date)
            {
                var s = JsonConvert.SerializeObject(date);
                return JsonConvert.DeserializeObject<AuctionReq>(s);
            }
            return JsonConvert.DeserializeObject<AuctionReq>(model as string);
        }
    }
}
