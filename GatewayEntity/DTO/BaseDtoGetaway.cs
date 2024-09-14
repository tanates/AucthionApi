using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace GatewayEntity.DTO
{
    public class BaseDtoGetaway<T>
    {

        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        public T Data { get; }

        private BaseDtoGetaway(bool isSuccess, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public BaseDtoGetaway(bool isSuccess, T data, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Data = data;
        }

        public static BaseDtoGetaway<T> Success(T data)
        {
            return new BaseDtoGetaway<T>(true, data);
        }

        public static BaseDtoGetaway<T> Failure(string errorMessage)
        {
            return new BaseDtoGetaway<T>(false, errorMessage);
        }
    }
}
