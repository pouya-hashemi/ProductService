using Beta.ProductService.WebApi.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Beta.ProductService.WebApi.Common
{
    public class RabbitMqPublisher<T>: IRabbitMqPublisher<T>
        where T : IRabbitMessage
    {
        private readonly IConfiguration _configuration;

        public RabbitMqPublisher(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public void Publish(T message)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(_configuration["RabbitMqConnection"])
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message.MessageData));

            channel.ExchangeDeclare(message.ExchangeName, message.ExchangeType);

            channel.BasicPublish(message.ExchangeName, "", null, body);
        }
    }
}
