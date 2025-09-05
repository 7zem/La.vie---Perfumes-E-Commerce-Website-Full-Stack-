using System.ComponentModel.DataAnnotations;

namespace Perfumes.BLL.DTOs.Chat
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public ChatMessageType Type { get; set; } = ChatMessageType.User;
        public List<ChatSuggestionDto>? Suggestions { get; set; }
        public List<ProductRecommendationDto>? ProductRecommendations { get; set; }
    }

    public class ChatSuggestionDto
    {
        public string Text { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string? Value { get; set; }
    }

    public class ProductRecommendationDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string? BrandName { get; set; }
        public string? CategoryName { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public enum ChatMessageType
    {
        User,
        Bot,
        System
    }

    public class ChatRequestDto
    {
        [Required]
        public string Message { get; set; } = string.Empty;
        public string? SessionId { get; set; }
        public string? UserId { get; set; }
        public ChatContextDto? Context { get; set; }
    }

    public class ChatResponseDto
    {
        public string Response { get; set; } = string.Empty;
        public List<ChatSuggestionDto> Suggestions { get; set; } = new();
        public List<ProductRecommendationDto> ProductRecommendations { get; set; } = new();
        public string SessionId { get; set; } = string.Empty;
        public bool IsTyping { get; set; } = false;
        public ChatIntent Intent { get; set; } = ChatIntent.General;
    }

    public class ChatContextDto
    {
        public string? CurrentPage { get; set; }
        public int? CurrentProductId { get; set; }
        public string? UserPreference { get; set; }
        public List<string> PreviousMessages { get; set; } = new();
    }

    public enum ChatIntent
    {
        General,
        ProductSearch,
        ProductRecommendation,
        OrderStatus,
        PaymentHelp,
        ShippingInfo,
        ReturnPolicy,
        AccountHelp,
        TechnicalSupport
    }
}
