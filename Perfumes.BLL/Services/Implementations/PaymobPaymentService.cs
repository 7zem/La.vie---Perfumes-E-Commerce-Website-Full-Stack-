using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Web;
using Perfumes.BLL.DTOs.Payments;
using Microsoft.Extensions.Options;
using Perfumes.BLL.Configuration;
using Perfumes.BLL.Services.Interfaces;
using Perfumes.DAL;
using System.Security.Cryptography;
using Perfumes.DAL.UnitOfWork;
using System.Text.Json.Nodes;

namespace Perfumes.BLL.Services.Implementations
{
    public class PaymobPaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaymobSettings _settings;
        private readonly HttpClient _httpClient;

        public PaymobPaymentService(IUnitOfWork unitOfWork, IOptions<PaymobSettings> settings)
        {
            _unitOfWork = unitOfWork;
            _settings = settings.Value;
            _httpClient = new HttpClient();
        }


        public async Task<PaymentResponseDto> InitiatePaymentAsync(PaymentRequestDto request)
        {
            try
            {
                // 1. Get Auth Token
                var authResponse = await _httpClient.PostAsync(
                    "https://accept.paymob.com/api/auth/tokens",
                    new StringContent(JsonSerializer.Serialize(new
                    {
                        api_key = _settings.ApiKey
                    }), Encoding.UTF8, "application/json"));

                if (!authResponse.IsSuccessStatusCode)
                    return new PaymentResponseDto { Success = false, ErrorMessage = "Auth failed" };

                var authJson = await authResponse.Content.ReadAsStringAsync();
                var authToken = JsonDocument.Parse(authJson).RootElement.GetProperty("token").GetString();

                // 2. Create Order
                var orderResponse = await _httpClient.PostAsync(
                    "https://accept.paymob.com/api/ecommerce/orders",
                    new StringContent(JsonSerializer.Serialize(new
                    {
                        auth_token = authToken,
                        delivery_needed = false,
                        amount_cents = (int)(request.Amount * 100),
                        currency = request.Currency,
                        items = new object[] { }
                    }), Encoding.UTF8, "application/json"));

                if (!orderResponse.IsSuccessStatusCode)
                    return new PaymentResponseDto { Success = false, ErrorMessage = "Order creation failed" };

                var orderJson = await orderResponse.Content.ReadAsStringAsync();
                var orderId = JsonDocument.Parse(orderJson).RootElement.GetProperty("id").GetInt32();

                // 3. Create Payment Key
                var paymentKeyResponse = await _httpClient.PostAsync(
                    "https://accept.paymob.com/api/acceptance/payment_keys",
                    new StringContent(JsonSerializer.Serialize(new
                    {
                        auth_token = authToken,
                        amount_cents = (int)(request.Amount * 100),
                        expiration = 3600,
                        order_id = orderId,
                        billing_data = new
                        {
                            apartment = "NA",
                            email = request.Email,
                            floor = "NA",
                            first_name = request.FullName,
                            last_name = "Client",
                            street = "NA",
                            building = "NA",
                            phone_number = request.Phone,
                            city = "Cairo",
                            country = "EG",
                            state = "NA"
                        },
                        currency = request.Currency,
                        integration_id = int.Parse(_settings.IntegrationId)
                    }), Encoding.UTF8, "application/json"));

                if (!paymentKeyResponse.IsSuccessStatusCode)
                    return new PaymentResponseDto { Success = false, ErrorMessage = "Payment key failed" };

                var paymentKeyJson = await paymentKeyResponse.Content.ReadAsStringAsync();
                var paymentKey = JsonDocument.Parse(paymentKeyJson).RootElement.GetProperty("token").GetString();

                // 4. Return payment URL with absolute return URL to our backend (which will redirect to frontend)
                string returnUrl;
                try
                {
                    var cb = new Uri(_settings.CallbackUrl ?? string.Empty);
                    var baseAuthority = $"{cb.Scheme}://{cb.Authority}";
                    returnUrl = baseAuthority + "/api/Order/payment-return";
                }
                catch
                {
                    // Fallback to no return url if settings malformed
                    returnUrl = string.Empty;
                }

                var paymentUrl = string.IsNullOrEmpty(returnUrl)
                    ? $"https://accept.paymob.com/api/acceptance/iframes/{_settings.IframeId}?payment_token={paymentKey}"
                    : $"https://accept.paymob.com/api/acceptance/iframes/{_settings.IframeId}?payment_token={paymentKey}&return_url={HttpUtility.UrlEncode(returnUrl)}";

                return new PaymentResponseDto
                {
                    Success = true,
                    PaymentUrl = paymentUrl
                };
            }
            catch (Exception ex)
            {
                return new PaymentResponseDto
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task HandlePaymentCallbackFromJsonAsync(string jsonPayload)
        {
            var data = JsonNode.Parse(jsonPayload)?["obj"];
            if (data == null) throw new Exception("Missing callback data.");

            var orderId = int.Parse(data["order"]?["id"]?.ToString() ?? "0");
            var transactionId = data["id"]?.ToString();
            var success = data["success"]?.ToString()?.ToLower() == "true";
            var amountCents = decimal.Parse(data["amount_cents"]?.ToString() ?? "0");
            var status = success ? "Paid" : "Failed";

            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                await _unitOfWork.Orders.UpdateAsync(order);
            }

            var payment = await _unitOfWork.Payments.FirstOrDefaultAsync(p => p.TransactionId == transactionId);
            if (payment != null)
            {
                payment.Status = status;
                payment.AmountPaid = amountCents / 100;
                await _unitOfWork.Payments.UpdateAsync(payment);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public bool VerifyHmacFromJson(string jsonPayload, string receivedHmac)
        {
            var data = JsonNode.Parse(jsonPayload)?["obj"];
            if (data == null) return false;

            var valuesToConcatenate =
                data["amount_cents"]?.ToString() +
                data["created_at"]?.ToString() +
                data["currency"]?.ToString() +
                data["error_occured"]?.ToString()?.ToLower() +
                data["has_parent_transaction"]?.ToString()?.ToLower() +
                data["id"]?.ToString() +
                data["integration_id"]?.ToString() +
                data["is_3d_secure"]?.ToString()?.ToLower() +
                data["is_auth"]?.ToString()?.ToLower() +
                data["is_capture"]?.ToString()?.ToLower() +
                data["is_refunded"]?.ToString()?.ToLower() +
                data["is_standalone_payment"]?.ToString()?.ToLower() +
                data["is_voided"]?.ToString()?.ToLower() +
                data["order"]?["id"]?.ToString() +
                data["owner"]?.ToString() +
                data["pending"]?.ToString()?.ToLower() +
                data["source_data"]?["pan"]?.ToString() +
                data["source_data"]?["sub_type"]?.ToString() +
                data["source_data"]?["type"]?.ToString() +
                data["success"]?.ToString()?.ToLower();

            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_settings.HmacSecret));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(valuesToConcatenate));
            var computed = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return computed == receivedHmac.ToLower();
        }

    }
}
