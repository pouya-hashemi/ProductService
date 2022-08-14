namespace Beta.ProductService.WebApi.Interfaces
{
    public interface IRabbitMqPublisher<T>
         where T : IRabbitMessage
    {
        void Publish(T message);
    }
}
