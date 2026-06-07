using APIproject.Application.DTOs;
using APIproject.Domain.Interfaces;
using APIproject.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.DataProtection;

namespace APIproject.Application.Features.Auth.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, UserResponseDto?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDataProtector _protector;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(IUserRepository userRepository,
            IDataProtectionProvider dataProtectionProvider,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _protector = dataProtectionProvider.CreateProtector("UserLogin");
            _jwtService = jwtService;

        }

        public async Task<UserResponseDto?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.EmailAddress);
            if (user == null)
            {
                return null; // User not found
            }

            var decryptedPassword = _protector.Unprotect(user.UserPassword);
            if (decryptedPassword != request.Password)
                return null;

            var token = _jwtService.GenerateToken(
                user.UserId,
                user.EmailAddress,
                user.UserRole
            );

            return new UserResponseDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                EmailAddress = user.EmailAddress,
                UserRole = user.UserRole,
                UserPhoto = user.UserPhoto,
                Token = token
            };
        }
    }
}
