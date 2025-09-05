using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perfumes.BLL.DTOs.Payments;
using Perfumes.BLL.Services.Interfaces;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Perfumes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _configuration;

        public PaymentController(IPaymentService paymentService, IConfiguration configuration)
        {
            _paymentService = paymentService;
            _configuration = configuration;
        }

        [HttpPost("initiate")]
        public async Task<IActionResult> InitiatePayment([FromBody] PaymentRequestDto request)
        {
            var result = await _paymentService.InitiatePaymentAsync(request);

            if (!result.Success)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(new { paymentUrl = result.PaymentUrl });
        }


        [HttpPost("callback")]
        public async Task<IActionResult> HandleCallback([FromQuery] string hmac)
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var jsonPayload = await reader.ReadToEndAsync();

            var isValid = _paymentService.VerifyHmacFromJson(jsonPayload, hmac);
            if (!isValid)
                return Unauthorized("Invalid HMAC signature.");

            await _paymentService.HandlePaymentCallbackFromJsonAsync(jsonPayload);
            return Ok();
        }

        // Some gateways redirect the user to the same callback URL (GET) after payment.
        // Support that by redirecting to frontend payment result page.
        [HttpGet("callback")]
        public IActionResult CallbackRedirect()
        {
            var baseUrl = _configuration["FrontendBaseUrl"] ?? "http://localhost:4200";
            var query = HttpContext.Request.QueryString.Value ?? string.Empty;
            var redirect = $"{baseUrl}/store/payment-result{query}";
            return Redirect(redirect);
        }
    }
}
