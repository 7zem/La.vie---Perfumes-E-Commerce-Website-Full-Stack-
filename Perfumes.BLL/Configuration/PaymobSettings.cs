using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfumes.BLL.Configuration
{
    public class PaymobSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string IframeId { get; set; } = string.Empty;
        public string IntegrationId { get; set; } = string.Empty;
        public string HmacSecret { get; set; } = string.Empty;
        public string CallbackUrl { get; set; } = string.Empty;
    }
}
