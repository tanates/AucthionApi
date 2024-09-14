using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionEntity.Model.DTO.ErrorDTO
{
    public class ErrorHandlerDTO<T>
    {
        public ErrorHandlerDTO()
        {
        }

        public ErrorHandlerDTO(T? value , bool result = true, string messange = null)
        {
            Messange=messange;
            Result=result;
            Value=value;
        }

        public string Messange { get; set; }
        public bool Result { get; set; }
        public T? Value { get; set; }
    }
}
