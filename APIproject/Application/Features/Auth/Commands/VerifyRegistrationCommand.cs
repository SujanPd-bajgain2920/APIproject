using MediatR;
namespace APIproject.Application.Features.Auth.Commands
{
    public class VerifyRegistrationCommand : IRequest<bool>
    {
        public string EmailAddress { get; set; } = null!;
        public string EnteredOtp { get; set; } = null!;
    }
}
