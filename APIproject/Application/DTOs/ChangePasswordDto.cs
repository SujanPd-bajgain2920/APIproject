namespace APIproject.Application.DTOs
{
    public class ChangePasswordDto
    {
        public string? EmailAddress { get; set; }
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }
}
