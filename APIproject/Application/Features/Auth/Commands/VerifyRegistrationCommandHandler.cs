using APIproject.Domain.Entities.Models;
using APIproject.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
namespace APIproject.Application.Features.Auth.Commands
{
    public class VerifyRegistrationCommandHandler : IRequestHandler<VerifyRegistrationCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IDataProtector _Protector;
        public VerifyRegistrationCommandHandler(
            IUserRepository userRepository,
            IEmailService emailService,
            IFileService fileService,
            IHttpContextAccessor httpContext,
            IDataProtectionProvider dataProtectionProvider)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _fileService = fileService;
            _httpContext = httpContext;
            _Protector = dataProtectionProvider.CreateProtector("UserRegistration");
        }
        public async Task<bool> Handle(VerifyRegistrationCommand request, CancellationToken cancellationToken)
        {
            var session = _httpContext.HttpContext!.Session;
            var storedOtp = session.GetString("RegisterOtp");
            var userDataJson = session.GetString("UserData");

            // Validate OTP
            if (storedOtp != request.EnteredOtp || string.IsNullOrEmpty(userDataJson))
                throw new InvalidOperationException("Invalid OTP!!");

            var userData = JsonSerializer.Deserialize<RegisteredUserSession>(userDataJson)!;

            // generate new user ID
            var userId = await _userRepository.GetNextUserIdAsync();

            // Move uploaded profile picture from temp to permanent location
            string? userPhotoPath = null;
            if (session.TryGetValue("UserTempPath", out var tempBytes))
            {
                var tempPath = Encoding.UTF8.GetString(tempBytes);
                var ext = Path.GetExtension(session.GetString("UserFileName"));
                var fileName = $"user_{userId}{ext}";
                userPhotoPath = await _fileService.MoveToPermanentAsync(tempPath, "UserImage", fileName);

            }

            // build and save user entity
            var user = new UserList
            {
                UserId = userId,
                FirstName = userData.FirstName,
                MiddleName = userData.MiddleName,
                LastName = userData.LastName,
                Phone = userData.Phone,
                EmailAddress = userData.EmailAddress,
                Province = userData.Province,
                District = userData.District,
                City = userData.City,
                Gender = userData.Gender,
                UserPhoto = userPhotoPath,
                UserRole = userData.UserRole,
                UserPassword = _Protector.Protect(userData.UserPassword)
            };

            await _userRepository.AddAsync(user);

            // cleanup session
            foreach (var key in new[] { "RegisterOtp", "UserData", "UserTempPath", "UserFileName" })
                session.Remove(key);

            return true;
        }
    }

    // small internal class to hold deserialized session data
    public class  RegisteredUserSession
    {
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string District { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
        public string UserRole { get; set; } = null!;

    }
}