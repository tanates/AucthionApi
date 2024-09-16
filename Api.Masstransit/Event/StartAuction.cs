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
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime EndTime { get; set; }
        public Guid IdOwner { get; set; }
        public string Discription { get; set; }
        public Guid IdLote { get; set; }

        public static StartAuction Start (Dictionary<string , string> data)
        {
            var str= JsonConvert.SerializeObject(data);
            var settings = new JsonSerializerSettings
            {
                DateFormatString = "dd.MM.yyyy HH:mm:ss" // Задаем нужный формат даты
            };
            return JsonConvert.DeserializeObject<StartAuction>(str ,settings);

        }
    }
}
