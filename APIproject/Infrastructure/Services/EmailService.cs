using APIproject.Domain.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net.Mail;

namespace APIproject.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // shared private method — same as your existing SendEmailAsync
        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("BidNetra System", emailSettings["FromEmail"]));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;
            message.Body = new BodyBuilder { HtmlBody = body }.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(emailSettings["SmtpServer"],
                emailSettings.GetValue<int>("SmtpPort"),
                SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(emailSettings["FromEmail"], emailSettings["FromPassword"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        // called from RegisterUserCommandHandler
        public async Task SendOtpAsync(string toEmail, string otp)
        {
            var body = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <style>
        body, html {{ margin:0; padding:0; font-family:'Segoe UI',sans-serif; color:#333; }}
        .container {{ max-width:600px; margin:0 auto; background:#fff; border-radius:8px; }}
        .header {{ background:linear-gradient(135deg,#1e40af,#1e3a8a); padding:30px 20px; text-align:center; color:white; }}
        .content {{ padding:40px 30px; }}
        .footer {{ background:#f8f8f8; padding:15px; text-align:center; font-size:12px; color:#666; border-top:1px solid #ddd; }}
        .token-box {{ background:#f1f5f9; padding:15px; border-radius:6px; font-family:monospace; font-size:24px; font-weight:bold; text-align:center; margin:20px 0; color:#1e40af; }}
        .security-note {{ background:#f8fafc; border-left:4px solid #3b82f6; padding:15px; margin:20px 0; font-size:14px; color:#64748b; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'><h1 style='margin:0;'>Email Verification</h1></div>
        <div class='content'>
            <h2 style='color:#0056b3;'>Welcome to BidNetra</h2>
            <p>Thank you for registering. Please use the verification code below to complete your registration.</p>
            <div class='token-box'>{otp}</div>
            <div class='security-note'>
                <strong>Security Tip:</strong> This code will expire after use.
                Do not share this code with anyone.
            </div>
            <p>If you did not request this, please ignore this email.</p>
        </div>
        <div class='footer'>
            <p>This is an automated message from BidNetra. Please do not reply.</p>
            <p>&copy; {DateTime.Now.Year} BidNetra. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

            await SendEmailAsync(toEmail, "Email Verification for Registration", body);
        }

        // called from ForgotPasswordCommandHandler
        public async Task SendPasswordResetTokenAsync(string toEmail, string token)
        {
            var body = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <style>
        body, html {{ margin:0; padding:0; font-family:'Segoe UI',sans-serif; color:#333; }}
        .container {{ max-width:600px; margin:0 auto; background:#fff; border-radius:8px; }}
        .header {{ background:linear-gradient(135deg,#1e40af,#1e3a8a); padding:30px 20px; text-align:center; color:white; }}
        .content {{ padding:40px 30px; }}
        .footer {{ background:#f8f8f8; padding:15px; text-align:center; font-size:12px; color:#666; border-top:1px solid #ddd; }}
        .security-note {{ background:#f8fafc; border-left:4px solid #3b82f6; padding:15px; margin:20px 0; font-size:14px; color:#64748b; }}
        .info-table {{ width:100%; border-collapse:collapse; margin:15px 0; }}
        .info-table td {{ padding:8px; border-bottom:1px solid #eee; }}
        .info-table td:first-child {{ font-weight:bold; width:140px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'><h1 style='margin:0;'>Password Reset Request</h1></div>
        <div class='content'>
            <h2 style='color:#0056b3;'>Reset Your Password</h2>
            <p>We received a request to reset your BidNetra account password.</p>
            <table class='info-table'>
                <tr><td>Account Email:</td><td>{toEmail}</td></tr>
                <tr><td>Request Time:</td><td>{DateTime.Now:dd MMM yyyy, HH:mm}</td></tr>
            </table>
            <div style='text-align:center; margin:20px 0;'>
                <strong>Your verification code:</strong>
                <div style='font-size:28px; font-weight:bold; color:#1e40af; margin-top:8px;'>{token}</div>
            </div>
            <div class='security-note'>
                <strong>Security Tip:</strong> Never share this code with anyone.
                BidNetra will never ask for your password or this token.
            </div>
            <p>If you did not request this, please ignore this email.</p>
        </div>
        <div class='footer'>
            <p>This is an automated message from BidNetra. Please do not reply.</p>
            <p>&copy; {DateTime.Now.Year} BidNetra. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

            await SendEmailAsync(toEmail, "Reset Your BidNetra Password", body);
        }
    }
}