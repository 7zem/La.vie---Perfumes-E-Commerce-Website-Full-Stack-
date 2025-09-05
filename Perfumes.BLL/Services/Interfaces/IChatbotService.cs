using Perfumes.BLL.DTOs.Chat;

namespace Perfumes.BLL.Services.Interfaces
{
    public interface IChatbotService
    {
        Task<ChatResponseDto> ProcessMessageAsync(ChatRequestDto request);
        Task<List<ChatMessageDto>> GetChatHistoryAsync(string sessionId, int limit = 50);
        Task<bool> SaveMessageAsync(ChatMessageDto message);
        Task<List<ChatSuggestionDto>> GetQuickSuggestionsAsync();
        Task<List<ProductRecommendationDto>> GetProductRecommendationsAsync(string query, int limit = 5);
        Task<string> GenerateResponseAsync(string userMessage, ChatContextDto? context = null);
        Task<ChatIntent> AnalyzeIntentAsync(string message);
        Task<bool> ClearChatHistoryAsync(string sessionId);
        Task<Dictionary<string, object>> GetChatAnalyticsAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}
