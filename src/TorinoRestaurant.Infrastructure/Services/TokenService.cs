using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Application.Abstractions.Services;
using TorinoRestaurant.Application.Authentication.Models;
using TorinoRestaurant.Application.Commons;
using TblUser = TorinoRestaurant.Core.Users.Entities.User;

namespace TorinoRestaurant.Infrastructure.Services
{
    public class TokenService(
        IOptions<JwtSettings> jwtSettings, IUnitOfWork unitOfWork) : ITokenService
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<AuthenticatedResult> GetTokenAsync(TblUser user)
        {
            string token = GenerateJwt(user);

            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

            user.UpdateRefreshToken(refreshToken, refreshTokenExpiryTime);

            await _unitOfWork.CommitAsync();

            return new AuthenticatedResult(token, refreshToken, refreshTokenExpiryTime);
        }

        private string GenerateJwt(TblUser user) =>
        GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user));

        private IEnumerable<Claim> GetClaims(TblUser user)
        {
            return
            [
                new(TorinoRestaurantJwtClaims.NameIdentifier, user.Id.ToString()),
                new(TorinoRestaurantJwtClaims.MobilePhone, user.PhoneNumber ?? string.Empty),
                new(TorinoRestaurantJwtClaims.Fullname, user.FullName ?? string.Empty),
                new(TorinoRestaurantJwtClaims.ImageUrl, user.ImageUrl ?? string.Empty),
            ];
        }

        private static string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
                signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = TorinoRestaurantJwtClaims.Role,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new UnauthorizedAccessException("Invalid Token.");
            }

            return principal;
        }

        private SigningCredentials GetSigningCredentials()
        {
            byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }
    }
}