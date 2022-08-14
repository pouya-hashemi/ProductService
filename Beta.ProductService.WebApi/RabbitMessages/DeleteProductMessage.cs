using Beta.ProductService.WebApi.Interfaces;

namespace Beta.ProductService.WebApi.RabbitMessages
{
    public class DeleteProductMessage : IRabbitMessage
    {
        public DeleteProductMessage(long Id)
        {
            this.ExchangeName = "DeleteProduct";
            this.ExchangeType = RabbitMQ.Client.ExchangeType.Fanout;
            this.MessageData = Id;
        }

        public string ExchangeName { get; private set; }
        public string ExchangeType { get; private set; }
        public object MessageData { get; private set; }
    }
}
