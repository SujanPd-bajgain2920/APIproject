namespace APIproject.Application.DTOs
{
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string UserRole { get; set; } = null!;
        public string? UserPhoto { get; set; }
        public string Token { get; set; } = null!;
    }
}
