using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
namespace AuctionEntity.Model.DTO
{
    public class settingsDTO
    {
        
        public string Exchange { get; set; }
        public string QueueName { get; set; }
        public string KeyQ { get; set; }
        public string ExType { get; set; }
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public bool Exclusive { get; set; }
        public IDictionary<string, string> Arguments { get; set; } = new Dictionary<string, string>();

        public IDictionary<string, string> Message { get; set; } = new Dictionary<string, string>();
        public static settingsDTO GetSettingsDTO<T>(T settingJson)
        {
            
            if (settingJson is IConfiguration config )
            {
                var z = JsonConvert.SerializeObject(config.GetSection("RabbitSettings").GetChildren().ToDictionary(x => x.Key, x => x.Value));
                return JsonConvert.DeserializeObject<settingsDTO>(z);
            }
            var s = JsonConvert.SerializeObject(settingJson);
            return JsonConvert.DeserializeObject<settingsDTO>(s);
        }
        
    }
}
