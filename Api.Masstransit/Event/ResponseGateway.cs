using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Masstransit.Event
{
    public class ResponseGateway
    {
        public bool IsSuccess { get; set; }
        public string Data { get; set; }
        public string Error { get; set; }
    }
}
