namespace TorinoRestaurant.Core.Abstractions.Exceptions
{
    public class DomainException(string message) : Exception(message)
    {
    }
}