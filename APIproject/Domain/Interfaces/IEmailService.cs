namespace APIproject.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendOtpAsync(string toEmail, string otp);
        Task SendPasswordResetTokenAsync(string toEmail, string token);
    }
}
