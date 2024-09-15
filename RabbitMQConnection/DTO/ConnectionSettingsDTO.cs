using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQConnection.DTO
{
    public class ConnectionSettingsDTO : IConfigServisec
    {
        private  IConfiguration _configuration;
        public ConnectionSettingsDTO(IConfiguration configuration)
        {
            ConfigurationSet(configuration);
            
        }

        public ConnectionSettingsDTO()
        {
        }

        private void ConfigurationSet(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Exchange { get; set; }
        public string QueueName { get; set; }
        public string KeyQ { get; set; }
        public string ExType { get; set; }
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public bool Exclusive { get; set; }
        public IDictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>();
        public IDictionary<string, object> Message { get; set; }

        public IConfiguration Configuration =>_configuration;

        public static ConnectionSettingsDTO GetSettingsDTO<T>(T settingJson ) 
        {
            if(settingJson is IConfigurationSection ss)
            {
                var z = JsonConvert.SerializeObject(ss.GetChildren().ToDictionary(x => x.Key, x => x.Value));
                return JsonConvert.DeserializeObject<ConnectionSettingsDTO>(z);
            }
            if (settingJson is IEnumerable<IConfigurationSection> config )
            {
                var z = JsonConvert.SerializeObject(config.ToDictionary(x => x.Key, x => x.Value));
                return JsonConvert.DeserializeObject<ConnectionSettingsDTO>(z);
            }

            if(settingJson is IConfiguration configuration)
            {
                var z = JsonConvert.SerializeObject(configuration.GetSection("StartRabbitSettings").GetChildren().ToDictionary(x => x.Key, x => x.Value));
                return JsonConvert.DeserializeObject<ConnectionSettingsDTO>(z);
            }
            var s = JsonConvert.SerializeObject(settingJson as IDictionary<string ,string>);
            
            return JsonConvert.DeserializeObject<ConnectionSettingsDTO>(s);
        }
    }
}
