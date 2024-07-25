using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using TorinoRestaurant.Application.Abstractions.Repositories;
using TorinoRestaurant.Application.Abstractions.Services;
using TorinoRestaurant.Application.Authentication.Models;
using TorinoRestaurant.Application.Commons;
using TorinoRestaurant.Core.Abstractions.Exceptions;
using TorinoRestaurant.Core.Abstractions.Guards;
using TblUser = TorinoRestaurant.Core.Users.Entities.User;

namespace TorinoRestaurant.Application.Authentication.Command
{
    public sealed record LoginCommand(string Username, string Password) : IRequest<AuthenticatedResult> { }

    public sealed class LoginCommandHandler(IRepository<TblUser, long> usersRepository,
        ITokenService tokenService) : IRequestHandler<LoginCommand, AuthenticatedResult>
    {
        private readonly IRepository<TblUser, long> _usersRepository = usersRepository;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<AuthenticatedResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var usersQuery = _usersRepository.GetAll();
            var user = await usersQuery.FirstOrDefaultAsync(x => x.PhoneNumber == request.Username, cancellationToken: cancellationToken);
            user = Guard.Against.NotFound(user, $"Phone number {request.Username} is not correct");

            if (Security.GetMD5(request.Password) != user.Password)
            {
                throw new DomainException("Invalid phone number or password");
            }
            return await _tokenService.GetTokenAsync(user);
        }
    }
}