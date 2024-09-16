using MassTransit;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Masstransit.Event
{
    public class StartAuction
    {
        public StartAuction(CreatLote lote , StartAuction startAuction)
        {
            Id=startAuction.Id;
            Name=startAuction.Name;
            EndTime=startAuction.EndTime;
            IdOwner=startAuction.IdOwner ;
            Discription=startAuction.Discription;
            LotID=lote.Id;
        }

        public StartAuction()
        {
        }

        public Guid Id { get; set; } =  Guid.NewGuid();
        public string Name { get; set; }
        public DateTime EndTime { get; set; }
        public Guid IdOwner { get; set; }
        public string Discription { get; set; }
        public Guid ? LotID { get; set; }
        public ResponseGateway? ResponseGateway { get; set ; }
        public static StartAuction Start(Dictionary<string, string> data)
        {
            var str = JsonConvert.SerializeObject(data);
            var settings = new JsonSerializerSettings
            {
                DateFormatString = "dd.MM.yyyy HH:mm:ss" // Задаем нужный формат даты
            };
            return JsonConvert.DeserializeObject<StartAuction>(str, settings);

        }

        public  static StartAuction SetLot(string jsonString)
        {
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
            var Auction = JsonConvert.DeserializeObject<StartAuction>(jsonObject["auction"].ToString());
            var  Lot = JsonConvert.DeserializeObject<CreatLote>(jsonObject["lot"].ToString());
            return  new StartAuction(Lot, Auction);
        }
        public static ResponseGateway SetGateway(bool res, string massenge)
        {
            return new ResponseGateway { Data = massenge, IsSuccess = res };
        }
    }
}
