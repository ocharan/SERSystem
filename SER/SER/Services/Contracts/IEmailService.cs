
using SER.Models;

namespace SER.Services
{
  public interface IEmailService
  {
    Task<bool> SendEmailAsync(EmailMessage emailMessage);
  }
}