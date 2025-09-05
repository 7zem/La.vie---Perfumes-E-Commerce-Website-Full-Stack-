using Microsoft.Extensions.Options;
using Perfumes.BLL.Configuration;
using Perfumes.BLL.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Perfumes.BLL.Services.Implementations
{
    public class SendGridEmailService(IOptions<SendGridSettings> options) : IEmailService
    {
        private readonly SendGridSettings _settings = options.Value;

        public async Task SendEmailAsync(string to, string subject, string htmlBody)
        {
            var client = new SendGridClient(_settings.ApiKey);

            var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
            var toEmail = new EmailAddress(to);

            var message = MailHelper.CreateSingleEmail(from, toEmail, subject, plainTextContent: null, htmlContent: htmlBody);

            var response = await client.SendEmailAsync(message);

            // throw if failed
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Body.ReadAsStringAsync();
                throw new Exception($"SendGrid error: {error}");
            }
        }

        public async Task SendTemplateEmailAsync(string to, string templateId, Dictionary<string, string> placeholders)
        {
            var client = new SendGridClient(_settings.ApiKey);

            var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
            var toEmail = new EmailAddress(to);

            var msg = new SendGridMessage
            {
                From = from,
                TemplateId = templateId
            };

            msg.AddTo(toEmail);

            msg.SetTemplateData(placeholders);

            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Body.ReadAsStringAsync();
                throw new Exception($"SendGrid template error: {error}");
            }
        }
    }
}
