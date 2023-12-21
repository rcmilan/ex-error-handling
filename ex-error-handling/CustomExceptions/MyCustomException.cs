namespace ex_error_handling.CustomExceptions
{
    public class MyCustomException(string? message) : Exception(message)
    {
    }
}
