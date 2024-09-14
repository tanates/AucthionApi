using MQConnection.DTO;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMQConnection
{
    public interface  IRabbitConnection
    {
        IConnection Connection { get; }
     
    }


    public class RabbitConnection : IRabbitConnection, IDisposable 
    {
        private  IConnection ?_connection;
   
        public RabbitConnection()
        {
            InitializeConnection();
        }

        private void InitializeConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = "jackal-01.rmq.cloudamqp.com",
                VirtualHost = "xyagawfh",
                Port =  5672,
                UserName ="xyagawfh",
                Password = "ucohri2GZ4TE5TYNqBQpxLkXDJQ5PeIo",
                Uri = new Uri("amqps://xyagawfh:ucohri2GZ4TE5TYNqBQpxLkXDJQ5PeIo@jackal.rmq.cloudamqp.com/xyagawfh")

            };
          
           
        }

        public IConnection Connection => _connection! ;
        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
