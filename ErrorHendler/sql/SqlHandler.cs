
using AuctionEntity.Model.DTO;
using AuctionEntity.Model.DTO.ErrorDTO;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ErrorHendler.sql
{
    public class SqlHandler {

        //public static async Task<T> ExecuteDbOperationAsync<T>(Func<Task> operation)
        //{
        //    try
        //    {
        //       return await operation();

        //    }
        //    catch (Exception ex)
        //    {

        //        throw new InvalidOperationException("Ошибка при выполнении операции с базой данных", ex);

        //    }


        //}

        public static async Task<T> ExecuteDbOperationAsync<T>(Func<Task<T>> operation)
        {
            try
            {
                return await operation();
            }
            catch (SqlException sqlEx)
            {
                // Логирование или специальные действия для SqlException
                throw new InvalidOperationException("Ошибка SQL при выполнении операции с базой данных", sqlEx);
            }
            catch (DbUpdateException dbEx)
            {
                // Логирование или специальные действия для DbUpdateException
                throw new InvalidOperationException("Ошибка обновления базы данных", dbEx);
            }
            
            catch (TimeoutException timeoutEx)
            {
                // Логирование или специальные действия для TimeoutException
                throw new InvalidOperationException("Превышено время ожидания операции с базой данных", timeoutEx);
            }
            catch (Exception ex)
            {
                // Обработка других исключений
                throw new InvalidOperationException("Ошибка при выполнении операции с базой данных", ex);
            }
        }


    }


}
