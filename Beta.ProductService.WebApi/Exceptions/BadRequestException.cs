using System.Net;

namespace Beta.ProductService.WebApi.Exceptions;

public class BadRequestException:Exception
{
    public BadRequestException(string message):base(message)
    {
        this.Data.Add("HttpStatus",(int)HttpStatusCode.BadRequest);
    }
}