using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQConnection.DTO
{
    public interface IConfigServisec
    {
       public IConfiguration ?Configuration { get; }

    }
}
