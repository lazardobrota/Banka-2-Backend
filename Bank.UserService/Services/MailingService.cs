using System.Net;
using System.Net.Mail;

using Bank.Application.Endpoints;
using Bank.UserService.Configurations;
using Bank.UserService.Models;
using Bank.UserService.Repositories;
using Bank.UserService.Security;

namespace Bank.UserService.Services;

public interface IEmailService
{
    public Task<Result> Send(EmailType type, User user);
}

public class EmailService(IEmailRepository emailRepository) : IEmailService
{
    private readonly IEmailRepository m_EmailRepository = emailRepository;

    public async Task<Result> Send(EmailType type, User user)
    {
        var email = m_EmailRepository.Find(type);

        try
        {
            var client = new SmtpClient(Configuration.Email.Server, Configuration.Email.Port);
            client.EnableSsl             = true;
            client.UseDefaultCredentials = false;
            client.Credentials           = new NetworkCredential(Configuration.Email.Address, Configuration.Email.Password);

            // TODO: one day & environment variable
            var body = type == EmailType.UserActivateAccount
                       ? email.FormatBody($"http://localhost:5173/activate?token={TokenProvider.GenerateToken(user)}")
                       : email.FormatBody($"http://localhost:5173/reset-password?token={TokenProvider.GenerateToken(user)}");

            var mailMessage = new MailMessage();
            mailMessage.From       = new MailAddress(Configuration.Email.Address);
            mailMessage.Subject    = email.Subject;
            mailMessage.Body       = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(user.Email);

            await client.SendMailAsync(mailMessage);

            return Result.Ok();
        }
        catch (Exception _)
        {
            return Result.BadRequest();
        }
    }
}
