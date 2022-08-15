using System.Net;
using Beta.ProductService.WebApi.Exceptions;
using Newtonsoft.Json;

namespace Beta.ProductService.WebApi.Common;

public class ExceptionHandlerMiddleWare
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleWare(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BadRequestException badEx)
        {
            context.Response.StatusCode = (int) badEx.Data["HttpStatus"];
            var body = JsonConvert.SerializeObject(new {ErrorMessage = badEx.Message});
            await context.Response.WriteAsync(body);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            var body = JsonConvert.SerializeObject(new {ErrorMessage = "Somthing Went Wrong"});
            await context.Response.WriteAsync(body);

            throw;
        }
    }
}