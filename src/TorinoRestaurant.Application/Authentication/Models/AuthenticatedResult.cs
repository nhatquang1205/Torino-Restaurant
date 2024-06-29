namespace TorinoRestaurant.Application.Authentication.Models
{
    public record AuthenticatedResult(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);
}