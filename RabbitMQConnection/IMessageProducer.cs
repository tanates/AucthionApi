using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        public  Task<(bool IsSuccses, string Message)> Send<T>(IConfigurationSection configurations, T message, IRabbitConnection connection);
        public void ReceiveAsync(IEnumerable<IConfigurationSection> configurations);

    }

    public class RabbitProducer : IMessageProducer 
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IRabbitConnection _connection;
        private readonly ILogger _logger; 
        public RabbitProducer( ILoggerFactory loggerFactory, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = loggerFactory.CreateLogger<RabbitProducer>();
            _serviceScopeFactory = serviceScopeFactory; 
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


        public  async Task<(bool IsSuccses, string Message)> Send<T>(IConfigurationSection configurations, T message , IRabbitConnection connection)
        {
            try
            {
                var settings = ConnectionSettingsDTO.GetSettingsDTO(configurations);
                using var scope = _serviceScopeFactory.CreateScope();
                //var connection = scope.ServiceProvider.GetRequiredService<IRabbitConnection>(); 
                using var channel = connection.Connection.CreateModel();

              
                channel.QueueDeclare(queue: settings.QueueName,
                                      durable: true,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                channel.BasicPublish(settings.Exchange, settings.KeyQ, null, body);

                _logger.LogInformation("Result send : true \n Message : " + message);
                return (true, "Message sent to RabbitMQ. Message: " + message);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }


    }
}
