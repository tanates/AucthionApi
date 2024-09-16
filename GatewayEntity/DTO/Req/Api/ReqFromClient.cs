
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

         
    }
}
