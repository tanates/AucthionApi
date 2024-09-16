using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace GatewayEntity.DTO
{
    public class BaseDtoGetaway
    {
        public string Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string  TaskDescription { get; set; }
        public bool IsCompleted { get; set; }

    }
}
