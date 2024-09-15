using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQConnection.DTO
{
    public class EndpointDTO
    {
       
        public static string  ActionEnd { get; set; }
        public static EndpointDTO  ActionENDAdd(string actionEnd)
        {
            return JsonConvert.DeserializeObject<EndpointDTO>(actionEnd);
        }
       
    }
}
