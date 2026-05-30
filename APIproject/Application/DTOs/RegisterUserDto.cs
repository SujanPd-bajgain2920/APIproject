using Microsoft.AspNetCore.Http;
namespace APIproject.Application.DTOs
{
        public class RegisterUserDto
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
            public IFormFile? UserFile { get; set; }
        }
    }

