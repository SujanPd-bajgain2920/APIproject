using MediatR;
namespace APIproject.Application.Features.Auth.Commands
{
    public class ForgotPasswordCommand : IRequest<bool>
    {
        public string EmailAddress { get; set; } = null!;
    }
}
