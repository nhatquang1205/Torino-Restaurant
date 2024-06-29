using TorinoRestaurant.Application.Authentication.Models;
using TblUser = TorinoRestaurant.Core.Users.Entities.User;

namespace TorinoRestaurant.Application.Abstractions.Services
{
    public interface ITokenService
    {
        Task<AuthenticatedResult> GetTokenAsync(TblUser user);

        // Task<AuthenticatedResult> RefreshTokenAsync(RefreshTokenQuery request);
    }
}