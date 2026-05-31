using APIproject.Application.DTOs;
using APIproject.Domain.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using MediatR;

namespace APIproject.Application.Features.Auth.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, UserResponseDto?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDataProtector _protector;

        public LoginCommandHandler(IUserRepository userRepository,
            IDataProtectionProvider dataProtectionProvider)
        {
            _userRepository = userRepository;
            _protector = dataProtectionProvider.CreateProtector("UserLogin");

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

            return new UserResponseDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                EmailAddress = user.EmailAddress,
                UserRole = user.UserRole,
                UserPhoto = user.UserPhoto
            };
        }
    }
}
