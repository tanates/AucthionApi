using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GatewayEntity.DTO.Req.Api
{
    public class ApiReq
    {
        public string RequestType { get; set; } // "Bid", "Item", "User", etc.

        public  object? Result { get; set; }

        public ReqFromClient ReqFromClient { get; set; }



        public static object[] ConvertJson<T>(T model)
        {
            if (model == null)
            {
                return null;
            }

            if (model is ReqFromClient fromClient)
            {
                var result = new object[]
                {
                    new { Service = fromClient.ServisecName },
                    new { Message = JsonConvert.SerializeObject(fromClient.Data) }
                };

                return result;
            }

            return null;
        }

    }



   public enum ReqTyp
    {
        Auct = 1 , 
        Lote = 2 ,
    }
}
