using MediatR;
using APIproject.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace APIproject.Application.Features.Auth.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContext;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IEmailService emailService,
            IFileService fileService,
            IHttpContextAccessor httpContext)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _fileService = fileService;
            _httpContext = httpContext;
        }

        public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {

            var session = _httpContext.HttpContext?.Session;
            var data = request.Data;

            // Check if email already exists
            if (await _userRepository.EmailExistsAsync(data.EmailAddress))
                throw new InvalidOperationException("User already exist with this email!!");
            // Save profile picture to temp location
            if (data.UserFile != null)
            {
                var tempPath = await _fileService.SaveTempFileAsync(data.UserFile);
                session.SetString("UserTempPath", tempPath);
                session.SetString("UserFileName", data.UserFile.FileName);
            }

            // store registration data in session for later use 
            var userData = new
            {
                data.FirstName,
                data.MiddleName,
                data.LastName,
                data.Phone,
                data.EmailAddress,
                data.Province,
                data.District,
                data.City,
                data.Gender,
                data.UserPassword,
                data.UserRole
            };
            session.SetString("UserData", JsonSerializer.Serialize(userData));


            // Generate otp and store in session
            var otp = new Random().Next(100000, 999999).ToString();
            session.SetString("RegisterOtp", otp);
            // send otp email
            await _emailService.SendOtpAsync(data.EmailAddress, otp);

            return true;
        }
    }
}
