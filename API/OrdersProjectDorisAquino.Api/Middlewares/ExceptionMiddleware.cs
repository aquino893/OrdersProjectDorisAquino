using System.Net;
using System.Text.Json;
using OrdersProjectDorisAquino.Application.CustomExceptions;

namespace OrdersProjectDorisAquino.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex, context);
        }
    }

    private static async Task HandleExceptionAsync(Exception ex, HttpContext context)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var details = string.Empty;
        switch (ex)
        {
            case BadRequestException badRequest:
                statusCode = HttpStatusCode.BadRequest;
                break;
            case NotFoundException notFound:
                statusCode = HttpStatusCode.NotFound;
                break;
            case ConflictException conflict:
                statusCode = HttpStatusCode.Conflict;
                break;
            default:
                details = ex.StackTrace ?? "";
                break;
        }

        var response = new { message = ex.Message, statusCode, details };

        var jsonResponse = JsonSerializer.Serialize(response);
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(jsonResponse);
    }
}