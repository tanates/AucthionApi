using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQConnection.DTO;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MQConnection
{
    public interface IMessageProducer
    {
        public  Task<(bool IsSuccses, string Message)> Send<T>(IEnumerable<IConfigurationSection> configurations, T message);
        public void ReceiveAsync(IEnumerable<IConfigurationSection> configurations);

    }

    public class RabbitProducer : IMessageProducer 
    {
        private readonly IRabbitConnection _connection;
        private readonly ILogger _logger; 
        public RabbitProducer(IRabbitConnection connection, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RabbitProducer>();
            _connection=connection;
        }

        public void ReceiveAsync(IEnumerable<IConfigurationSection> configurations )
        { 


            using var channel = _connection.Connection.CreateModel();
            var settings = ConnectionSettingsDTO.GetSettingsDTO(configurations);
            // Убедитесь, что обменник существует
            channel.ExchangeDeclare(exchange: settings.Exchange, type: settings.ExType);

            // Создание временной очереди
            channel.QueueDeclare(
                queue:settings.QueueName,
                settings.Durable,
                settings.Exclusive, 
                settings.AutoDelete, 
                settings.Arguments );

            // Привязка очереди к обменнику
            channel.QueueBind(
                queue: settings.QueueName,
                exchange: settings.Exchange, 
                routingKey: settings.KeyQ,
                arguments: settings.Arguments);

           


             channel.BasicQos(0, 1, false);
            // Возвращаем задачу, которую мы будем завершать в событии consumer.Received
        }


        public async Task <(bool IsSuccses ,string Message)>  Send<T>(IEnumerable<IConfigurationSection> configurations , T message)
        {
            using var channel = _connection.Connection.CreateModel();

            try
            {
                var tcs = new TaskCompletionSource<Task>();

                var settings = ConnectionSettingsDTO.GetSettingsDTO(configurations);



                channel.ExchangeDeclare(
                    exchange: settings.Exchange,
                    type: settings.ExType,
                    durable: settings.Durable,
                    autoDelete: settings.AutoDelete,
                    arguments: settings.Arguments);


                var body = Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(message));
                
                channel.BasicPublish(settings.Exchange, settings.KeyQ, null, body);
                await Task.Delay(1000);
                var r = tcs.Task.Result;
                
                _logger.LogInformation("Result send : true \n Message : " + message);
                return (true, "message send to RabbitMQ. Message :" + message);
                
            }
            catch (Exception ex)
            {
                return  ( false,  ex.Message );
            }
        }

        
    }
}
