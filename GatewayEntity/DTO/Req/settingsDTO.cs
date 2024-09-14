using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

public class settingsDTO
{
    public string Exchange { get; set; }
    public string QueueName { get; set; }
    public string KeyQ { get; set; }
    public string ExType { get; set; }
    public bool Durable { get; set; }
    public bool AutoDelete { get; set; }
    public bool Exclusive { get; set; }
    public IDictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>();

    public IDictionary<string, object> Message { get; set; }= new Dictionary<string, object>();
    public static settingsDTO GetSettingsDTO<T>(T settingJson)
    {

        if (settingJson is IConfiguration config)
        {
            var z = JsonConvert.SerializeObject(config.GetSection("RabbitSettings").GetChildren().ToDictionary(x => x.Key, x => x.Value));
            return JsonConvert.DeserializeObject<settingsDTO>(z);
        }
        var s = JsonConvert.SerializeObject(settingJson);
        return JsonConvert.DeserializeObject<settingsDTO>(s);
    }
}