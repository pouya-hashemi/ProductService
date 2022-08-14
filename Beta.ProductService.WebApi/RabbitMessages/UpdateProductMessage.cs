using Beta.ProductService.WebApi.Dtos.ProductDtos;
using Beta.ProductService.WebApi.Interfaces;

namespace Beta.ProductService.WebApi.RabbitMessages
{
    public class UpdateProductMessage : IRabbitMessage
    {
        public UpdateProductMessage(ProductReadDto messageData)
        {
            this.ExchangeName = "UpdateProduct";
            this.ExchangeType = RabbitMQ.Client.ExchangeType.Fanout;
            this.MessageData = messageData;
        }

        public string ExchangeName { get; private set; }
        public string ExchangeType { get; private set; }
        public object MessageData { get; private set; }
    }
}
