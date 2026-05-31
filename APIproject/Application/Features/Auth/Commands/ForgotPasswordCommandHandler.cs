using APIproject.Domain.Interfaces;
using MediatR;

namespace APIproject.Application.Features.Auth.Commands
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContext;
        public ForgotPasswordCommandHandler(IUserRepository userRepository,
            IEmailService emailService,
            IHttpContextAccessor httpContext)
        {
            _userRepository = userRepository;
            _emailService = emailService;

        }
        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.EmailAddress);
            if (user == null)
                throw new InvalidOperationException("User with the provided email address does not exist.");

            var token = new Random().Next(100000, 999999).ToString();
            _httpContext.HttpContext!.Session.SetString("token", token);

            await _emailService.SendPasswordResetTokenAsync(user.EmailAddress, token);
            return true;
        }
    }
}
