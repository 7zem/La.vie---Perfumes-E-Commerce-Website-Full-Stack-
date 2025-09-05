using Microsoft.AspNetCore.Mvc;
using Perfumes.BLL.Services.Interfaces;

namespace Perfumes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendTestEmail([FromQuery] string to)
        {
            var subject = "Test Email from Perfumes Store";
            var htmlBody = "<h3>Hello!</h3><p>This is a test email sent using SendGrid.</p>";

            try
            {
                await _emailService.SendEmailAsync(to, subject, htmlBody);
                return Ok(new { message = "Email sent successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("send-template")]
        public async Task<IActionResult> SendTemplateTestEmail([FromQuery] string to)
        {
            try
            {
                await _emailService.SendTemplateEmailAsync(
                    to,
                    "d-c3292025cc1a4ebfab1aae1af921e81d",
                    new Dictionary<string, string>
                    {
                        { "name", "Test Client" },
                        { "orderId", "ORDER-9999" }
                    });

                return Ok(new { message = "Template email sent successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
