using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using SER.Configuration;
using SER.Models;

namespace SER.Services
{
  public class EmailService : IEmailService
  {
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
      _emailSettings = emailSettings.Value;
    }

    private MimeMessage CreateMimeMessage(EmailMessage emailMessage)
    {
      MimeMessage mimeMessage = new MimeMessage();

      mimeMessage.From.Add(new MailboxAddress(
        _emailSettings.DisplayName,
        _emailSettings.Username
      ));

      mimeMessage.To.Add(MailboxAddress.Parse(emailMessage.To));
      mimeMessage.Subject = emailMessage.Subject;

      mimeMessage.Body = new TextPart(TextFormat.Html)
      {
        Text = emailMessage.Content
      };

      return mimeMessage;
    }

    public async Task<bool> SendEmailAsync(EmailMessage emailMessage)
    {
      MimeMessage email = CreateMimeMessage(emailMessage);
      SmtpClient client = new SmtpClient();
      bool isSent = false;

      try
      {
        await client.ConnectAsync(
          _emailSettings.Host,
          _emailSettings.Port,
          SecureSocketOptions.StartTls
        );

        await client.AuthenticateAsync(
          _emailSettings.Username,
          _emailSettings.Password
        );

        await client.SendAsync(email);
        isSent = true;
      }
      catch (Exception ex)
      {
        ExceptionLogger.LogException(ex);
        throw new Exception("Ha ocurrido un error al enviar el correo electrónico");
      }
      finally
      {
        await client.DisconnectAsync(true);
        client.Dispose();
      }

      return isSent;
    }
  }
}