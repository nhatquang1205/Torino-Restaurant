namespace TorinoRestaurant.Application.Abstractions.Services
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
    }
}