using RabbitMQ.Client;

namespace Beta.ProductService.WebApi.Interfaces
{
    public interface IRabbitMessage
    {
        public string ExchangeName { get; }
        public string ExchangeType { get; }
        public object MessageData { get; }
    }
}
