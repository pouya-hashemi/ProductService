using Beta.ProductService.WebApi.Dtos.ProductDtos;
using Beta.ProductService.WebApi.Interfaces;
using RabbitMQ.Client;

namespace Beta.ProductService.WebApi.RabbitMessages
{
    public class CreateProductMessage : IRabbitMessage
    {
        public CreateProductMessage(ProductReadDto messageData)
        {
            this.ExchangeName = "CreateProduct";
            this.ExchangeType = RabbitMQ.Client.ExchangeType.Fanout;   
            this.MessageData = messageData;
        }

        public string ExchangeName { get;private set; }
        public string ExchangeType { get;private set; }
        public object MessageData { get;private set; }
    }
}
