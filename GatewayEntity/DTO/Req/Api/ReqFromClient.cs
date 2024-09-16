
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayEntity.DTO.Req.Api
{
    public class ReqFromClient
    {
        public string ServisecName { get; set; }

        public Dictionary<string ,string> Data { get; set; }

        public static ReqFromClient addData(string data)
        {
            var dataDictionary = new Dictionary<string, string>
            {
                { "Key" , data}
            };
            
            return new ReqFromClient { Data = dataDictionary };
        }
    }
}
