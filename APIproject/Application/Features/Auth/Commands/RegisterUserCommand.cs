using APIproject.Application.DTOs;
using MediatR;

namespace APIproject.Application.Features.Auth.Commands
{
    public class RegisterUserCommand : IRequest<bool>
    {
        public RegisterUserDto Data { get; set; } = null!;

        public RegisterUserCommand(RegisterUserDto data) 
        {
            Data = data;
        }
    }
}
