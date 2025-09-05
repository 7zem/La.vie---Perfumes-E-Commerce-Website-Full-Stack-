using Perfumes.BLL.DTOs.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfumes.BLL.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> InitiatePaymentAsync(PaymentRequestDto request);
        Task HandlePaymentCallbackFromJsonAsync(string jsonPayload);
        bool VerifyHmacFromJson(string jsonPayload, string receivedHmac);
    }
}