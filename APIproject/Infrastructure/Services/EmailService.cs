using APIproject.Domain.Interfaces;
using APIproject.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace APIproject.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSetting)
        {
            _emailSettings = emailSetting.Value;
        }

        private SmtpClient CreateSmtpClient()
        {
            return new SmtpClient
            {
                Host = _emailSettings.SmtpServer,
                Port = _emailSettings.SmtpPort,
                Credentials = new NetworkCredential(
                    _emailSettings.FromEmail,
                    _emailSettings.FromPassword
                ),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }

        public async Task SendOtpAsync(string toEmail, string otp)
        {
            var body = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Email Verification</title>
    <style>
        body, html {{ margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.05); }}
        .header {{ background: linear-gradient(135deg, #1e40af 0%, #1e3a8a 100%); padding: 30px 20px; text-align: center; color: white; }}
        .content {{ padding: 40px 30px; background-color: #ffffff; }}
        .footer {{ background-color: #f8f8f8; padding: 15px; text-align: center; font-size: 12px; color: #666; border-top: 1px solid #ddd; }}
        .token-box {{ background-color: #f1f5f9; padding: 15px; border-radius: 6px; font-family: monospace; font-size: 24px; font-weight: bold; text-align: center; margin: 20px 0; color: #1e40af; }}
        .security-note {{ background-color: #f8fafc; border-left: 4px solid #3b82f6; padding: 15px; margin: 20px 0; font-size: 14px; color: #64748b; }}
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
                Do not share this code with anyone. BidNetra representatives will never ask for this code.
            </div>
            <p>If you did not request this registration, please ignore this email.</p>
        </div>
        <div class='footer'>
            <p>This is an automated message from BidNetra. Please do not reply.</p>
            <p>&copy; {DateTime.Now.Year} BidNetra. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

            var mail = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail),
                Subject = "Email Verification for Registration",
                Body = body,
                IsBodyHtml = true
            };
            mail.To.Add(toEmail);

            using var smtp = CreateSmtpClient();
            await smtp.SendMailAsync(mail);
        }

        public async Task SendPasswordResetTokenAsync(string toEmail, string token)
        {
            var body = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Password Reset Request</title>
    <style>
        body, html {{ margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.05); }}
        .header {{ background: linear-gradient(135deg, #1e40af 0%, #1e3a8a 100%); padding: 30px 20px; text-align: center; color: white; }}
        .content {{ padding: 40px 30px; background-color: #ffffff; }}
        .footer {{ background-color: #f8f8f8; padding: 15px; text-align: center; font-size: 12px; color: #666; border-top: 1px solid #ddd; }}
        .info-table {{ width: 100%; border-collapse: collapse; margin: 15px 0; }}
        .info-table td {{ padding: 8px; border-bottom: 1px solid #eee; }}
        .info-table td:first-child {{ font-weight: bold; width: 140px; }}
        .security-note {{ background-color: #f8fafc; border-left: 4px solid #3b82f6; padding: 15px; margin: 20px 0; font-size: 14px; color: #64748b; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'><h1 style='margin:0;'>Password Reset Request</h1></div>
        <div class='content'>
            <h2 style='color:#0056b3;'>Reset Your Password</h2>
            <p>We received a request to reset your password for your BidNetra account.</p>
            <table class='info-table'>
                <tr><td>Account Email:</td><td>{toEmail}</td></tr>
                <tr><td>Request Time:</td><td>{DateTime.Now:dd MMM yyyy, HH:mm}</td></tr>
            </table>
            <div style='text-align:center; margin: 20px 0;'>
                <strong>Your verification code:</strong>
                <div style='font-size:28px; font-weight:bold; color:#1e40af; margin-top:8px;'>{token}</div>
            </div>
            <div class='security-note'>
                <strong>Security Tip:</strong> Never share this code with anyone.
                BidNetra representatives will never ask for your password or this token.
            </div>
            <p>If you did not request a password reset, please ignore this email.</p>
        </div>
        <div class='footer'>
            <p>This is an automated message from BidNetra. Please do not reply.</p>
            <p>&copy; {DateTime.Now.Year} BidNetra. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

            var mail = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail),
                Subject = "Reset Your BidNetra Password",
                Body = body,
                IsBodyHtml = true
            };
            mail.To.Add(toEmail);

            using var smtp = CreateSmtpClient();
            await smtp.SendMailAsync(mail);
        }
    }
}