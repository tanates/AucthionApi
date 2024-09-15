
using LibMessage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQConnection.DTO;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQConnection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace MQConnection
{
    public class RabbitMQServisec : BackgroundService 
    {
        
        
        private IServiceProvider _serviceProvider;
        private readonly ConnectionSettingsDTO settings;
        private IConnection? _connection;
        private IModel? _channel;
        private readonly ILogger _logger;
        public RabbitMQServisec(

            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider
            )
        {
            this._logger = loggerFactory.CreateLogger<RabbitMQServisec>();
            _serviceProvider= serviceProvider;
            using (var scope = _serviceProvider.CreateScope())
            {
                var conf = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                
                settings = ConnectionSettingsDTO.GetSettingsDTO(conf);
                
            }

        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await Init();
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Close();
            _logger.LogInformation("Connections state"+_connection.IsOpen.ToString());
            await base.StopAsync(cancellationToken);
        }

        public async Task Init()
        {

            using var scope = _serviceProvider.CreateScope();
            var con  = scope.ServiceProvider.GetRequiredService<IRabbitConnection>();
            _connection  = con.Connection;
            _channel = _connection.CreateModel();
            
            // Убедитесь, что обменник существует
            _channel.ExchangeDeclare(exchange: settings.Exchange, type: settings.ExType);

            // Создание временной очереди
            _channel.QueueDeclare(
                queue: settings.QueueName,
                settings.Durable,
                settings.Exclusive,
                settings.AutoDelete,
                settings.Arguments);

            // Привязка очереди к обменнику
            _channel.QueueBind(
                queue: settings.QueueName,
                exchange: settings.Exchange,
                routingKey: settings.KeyQ,
                arguments: settings.Arguments);




            _channel.BasicQos(0, 1, false);
            // Возвращаем задачу, которую мы будем завершать в событии consumer.Received
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Init();

            while (!stoppingToken.IsCancellationRequested)
            {
                var message = await HandleReceivedMessag(stoppingToken);
                if (message != null)
                {
                    using (var sсope = _serviceProvider.CreateScope())
                    {
                        var messageService = sсope.ServiceProvider.GetRequiredService<IMessageService>();
                        
                        _logger.LogInformation(messageService.GetType().Name, message);
                        messageService.StoreMessage(message);

                    }
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
       


    
        private async Task<string> HandleReceivedMessag(CancellationToken stoppingToken)
        {
            
            try
            {

                var tcs = new TaskCompletionSource<string>();
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received +=  (ch, message) =>
                {
                    // received body
                    if (!tcs.Task.IsCompleted)
                    {
                        var content = Encoding.UTF8.GetString(message.Body.ToArray());
                        tcs.SetResult(content);
                        
                        _channel.BasicAck(message.DeliveryTag, false);
                        _logger.LogInformation($"Received {message.DeliveryTag}" + $"\n Message : {content}" + $"\n Scope : {_serviceProvider.GetType().Name}");
                    }
                };


                _channel.BasicConsume(settings.QueueName, false, consumer);
                using (stoppingToken.Register(() => tcs.TrySetCanceled()))
                {
                    return await tcs.Task; // Ожидаем получения сообщения
                };
            }
            catch (OperationCanceledException ex)
            {

                throw ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения сообщения из RabbitMQ");
                throw; // Перебрасываем исключение выше
            }
        }
        
        
        public override void Dispose()
        {
            this._connection.Dispose();
            this._channel.Dispose();
            base.Dispose();
        }
    }
}
