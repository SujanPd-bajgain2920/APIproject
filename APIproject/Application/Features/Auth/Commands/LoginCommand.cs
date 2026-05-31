using APIproject.Application.DTOs;
using MediatR;

namespace APIproject.Application.Features.Auth.Commands
{
    public class LoginCommand : IRequest<UserResponseDto?>
    {
        public string EmailAddress { get; set; } = null!;
        public string Password { get; set; } = null!;

    }
}
