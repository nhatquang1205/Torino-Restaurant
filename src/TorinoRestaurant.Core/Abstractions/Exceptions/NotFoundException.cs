namespace TorinoRestaurant.Core.Abstractions.Exceptions
{
    public sealed class NotFoundException(string message) : Exception(message)
    {
        public NotFoundException() : this("Not found")
        {

        }
    }
}