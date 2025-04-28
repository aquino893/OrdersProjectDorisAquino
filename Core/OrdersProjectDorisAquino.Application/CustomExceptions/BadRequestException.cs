namespace OrdersProjectDorisAquino.Application.CustomExceptions;

public class BadRequestException(string message) : Exception(message)
{
}