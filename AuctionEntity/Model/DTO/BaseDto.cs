
using AuctionEntity.Model.DTO.ErrorDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionEntity.Model.DTO
{
    public class BaseDto<T>
    {
        public ErrorHandlerDTO<T>  ErrorDto { get; set; }
        public T Data { get; set; }
        

        public BaseDto(T data)
        {
          
            Data = data;
        }
        public BaseDto()
        {
        }

        public BaseDto(T data, ErrorHandlerDTO<T>? errorDto = null)
        {
           
            ErrorDto=errorDto;
            Data=data;
        }
    }
}
