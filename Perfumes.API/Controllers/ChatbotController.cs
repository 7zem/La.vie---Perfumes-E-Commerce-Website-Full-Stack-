using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perfumes.BLL.DTOs.Chat;
using Perfumes.BLL.Services.Interfaces;

namespace Perfumes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly IChatbotService _chatbotService;
        private readonly ILoggingService _loggingService;

        public ChatbotController(IChatbotService chatbotService, ILoggingService loggingService)
        {
            _chatbotService = chatbotService;
            _loggingService = loggingService;
        }

        [HttpPost("chat")]
        public async Task<ActionResult<ChatResponseDto>> SendMessage([FromBody] ChatRequestDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Message))
                {
                    return BadRequest(new { message = "Message cannot be empty" });
                }

                var response = await _chatbotService.ProcessMessageAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error in chatbot chat endpoint: {ex.Message}", ex);
                return StatusCode(500, new { message = "An error occurred while processing your message" });
            }
        }

        [HttpGet("history/{sessionId}")]
        public async Task<ActionResult<List<ChatMessageDto>>> GetChatHistory(string sessionId, [FromQuery] int limit = 50)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sessionId))
                {
                    return BadRequest(new { message = "Session ID cannot be empty" });
                }

                var history = await _chatbotService.GetChatHistoryAsync(sessionId, limit);
                return Ok(history);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting chat history: {ex.Message}", ex);
                return StatusCode(500, new { message = "An error occurred while retrieving chat history" });
            }
        }

        [HttpGet("suggestions")]
        public async Task<ActionResult<List<ChatSuggestionDto>>> GetQuickSuggestions()
        {
            try
            {
                var suggestions = await _chatbotService.GetQuickSuggestionsAsync();
                return Ok(suggestions);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting quick suggestions: {ex.Message}", ex);
                return StatusCode(500, new { message = "An error occurred while retrieving suggestions" });
            }
        }

        [HttpGet("recommendations")]
        public async Task<ActionResult<List<ProductRecommendationDto>>> GetProductRecommendations([FromQuery] string query, [FromQuery] int limit = 5)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest(new { message = "Query cannot be empty" });
                }

                var recommendations = await _chatbotService.GetProductRecommendationsAsync(query, limit);
                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting product recommendations: {ex.Message}", ex);
                return StatusCode(500, new { message = "An error occurred while retrieving recommendations" });
            }
        }

        [HttpDelete("history/{sessionId}")]
        public async Task<ActionResult> ClearChatHistory(string sessionId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sessionId))
                {
                    return BadRequest(new { message = "Session ID cannot be empty" });
                }

                var success = await _chatbotService.ClearChatHistoryAsync(sessionId);
                if (success)
                {
                    return Ok(new { message = "Chat history cleared successfully" });
                }
                else
                {
                    return StatusCode(500, new { message = "Failed to clear chat history" });
                }
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error clearing chat history: {ex.Message}", ex);
                return StatusCode(500, new { message = "An error occurred while clearing chat history" });
            }
        }

        [HttpGet("analytics")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Dictionary<string, object>>> GetChatAnalytics([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var analytics = await _chatbotService.GetChatAnalyticsAsync(startDate, endDate);
                return Ok(analytics);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting chat analytics: {ex.Message}", ex);
                return StatusCode(500, new { message = "An error occurred while retrieving analytics" });
            }
        }

        [HttpPost("test")]
        public async Task<ActionResult<ChatResponseDto>> TestChat([FromBody] string message)
        {
            try
            {
                var request = new ChatRequestDto
                {
                    Message = message,
                    SessionId = "test-session"
                };

                var response = await _chatbotService.ProcessMessageAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error in test chat: {ex.Message}", ex);
                return StatusCode(500, new { message = "An error occurred during test" });
            }
        }
    }
}
