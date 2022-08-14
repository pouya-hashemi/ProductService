namespace Beta.ProductService.WebApi.Interfaces
{
    public interface IRabbitMqPublisher
    {
        void Publish<T>(T message)
                    where T : IRabbitMessage;
    }
}
