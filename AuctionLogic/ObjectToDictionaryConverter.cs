using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AuctionLogic
{
    public class ObjectToDictionaryConverter
    {
        public static Dictionary<string, object> ConvertToDictionary<T>(T obj)
        {
            var dictionary = new Dictionary<string, object>();

            // Получаем все свойства типа T
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                // Получаем значение свойства
                var value = property.GetValue(obj);

                // Добавляем в словарь
                dictionary.Add(property.Name, value);
            }

            return dictionary;
        }
    }
}
