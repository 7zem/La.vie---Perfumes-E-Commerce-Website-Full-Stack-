using AutoMapper;
using Perfumes.BLL.DTOs.Chat;
using Perfumes.BLL.DTOs.Product;
using Perfumes.BLL.Services.Interfaces;
using Perfumes.DAL.UnitOfWork;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Perfumes.BLL.Services.Implementations
{
    public class ChatbotService : IChatbotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly ILoggingService _loggingService;
        private readonly ICachingService _cachingService;
        private readonly IMapper _mapper;

        // Knowledge base for the chatbot
        private readonly Dictionary<string, string> _faqResponses = new()
        {
            { "shipping", "We offer free shipping on orders over $50. Standard shipping takes 3-5 business days, while express shipping (1-2 days) is available for an additional fee." },
            { "return", "We have a 30-day return policy. Products must be unused and in original packaging. Return shipping is free for defective items." },
            { "payment", "We accept all major credit cards, PayPal, and Apple Pay. All payments are processed securely through our payment gateway." },
            { "warranty", "All our perfumes come with a 1-year warranty against manufacturing defects. Contact our support team for warranty claims." },
            { "gift", "Yes! We offer gift wrapping and personalized gift cards. You can add a personal message during checkout." },
            { "authenticity", "All our perfumes are 100% authentic and sourced directly from authorized distributors. We guarantee the authenticity of every product." }
        };

        private readonly Dictionary<string, string[]> _intentKeywords = new()
        {
            { "ProductSearch", new[] { "find", "search", "looking for", "need", "want", "recommend", "suggest" } },
            { "OrderStatus", new[] { "order", "track", "shipping", "delivery", "when", "status" } },
            { "PaymentHelp", new[] { "payment", "pay", "card", "credit", "debit", "paypal", "refund" } },
            { "ShippingInfo", new[] { "shipping", "delivery", "ship", "arrive", "time", "cost" } },
            { "ReturnPolicy", new[] { "return", "refund", "exchange", "policy", "warranty" } },
            { "AccountHelp", new[] { "account", "login", "password", "register", "profile" } },
            { "TechnicalSupport", new[] { "problem", "issue", "error", "broken", "not working", "help" } }
        };

        public ChatbotService(
            IUnitOfWork unitOfWork,
            IProductService productService,
            ICategoryService categoryService,
            IBrandService brandService,
            ILoggingService loggingService,
            ICachingService cachingService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _loggingService = loggingService;
            _cachingService = cachingService;
            _mapper = mapper;
        }

        public async Task<ChatResponseDto> ProcessMessageAsync(ChatRequestDto request)
        {
            try
            {
                _loggingService.LogInformation($"Processing chat message: {request.Message}");

                var sessionId = request.SessionId ?? GenerateSessionId();
                var intent = await AnalyzeIntentAsync(request.Message);
                var response = await GenerateResponseAsync(request.Message, request.Context);
                var suggestions = await GetQuickSuggestionsAsync();
                var productRecommendations = await GetProductRecommendationsAsync(request.Message);

                var chatResponse = new ChatResponseDto
                {
                    Response = response,
                    Suggestions = suggestions,
                    ProductRecommendations = productRecommendations,
                    SessionId = sessionId,
                    Intent = intent
                };

                // Save the conversation
                await SaveMessageAsync(new ChatMessageDto
                {
                    Message = request.Message,
                    Response = response,
                    SessionId = sessionId,
                    UserId = request.UserId ?? "anonymous",
                    Type = ChatMessageType.User,
                    Timestamp = DateTime.UtcNow
                });

                return chatResponse;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error processing chat message: {ex.Message}", ex);
                return new ChatResponseDto
                {
                    Response = "I apologize, but I'm experiencing some technical difficulties. Please try again in a moment or contact our support team.",
                    SessionId = request.SessionId ?? GenerateSessionId(),
                    Intent = ChatIntent.General
                };
            }
        }

        public async Task<string> GenerateResponseAsync(string userMessage, ChatContextDto? context = null)
        {
            var message = userMessage.ToLower();
            var intent = await AnalyzeIntentAsync(userMessage);

            // Handle specific intents
            switch (intent)
            {
                case ChatIntent.ProductSearch:
                case ChatIntent.ProductRecommendation:
                    return await GenerateProductResponseAsync(userMessage);

                case ChatIntent.OrderStatus:
                    return "To check your order status, please provide your order number or log into your account. You can also contact our support team for assistance.";

                case ChatIntent.PaymentHelp:
                    return _faqResponses["payment"];

                case ChatIntent.ShippingInfo:
                    return _faqResponses["shipping"];

                case ChatIntent.ReturnPolicy:
                    return _faqResponses["return"];

                case ChatIntent.AccountHelp:
                    return "For account-related issues, you can reset your password, create a new account, or contact our support team. What specific help do you need?";

                case ChatIntent.TechnicalSupport:
                    return "I'm here to help with technical issues. Could you please describe the problem you're experiencing in more detail?";

                default:
                    return await GenerateGeneralResponseAsync(userMessage);
            }
        }

        private async Task<string> GenerateProductResponseAsync(string userMessage)
        {
            var products = await _productService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();
            var brands = await _brandService.GetAllAsync();

            var message = userMessage.ToLower();

            // Check for specific product mentions
            foreach (var product in products)
            {
                if (message.Contains(product.Name.ToLower()))
                {
                    return $"I found '{product.Name}' in our collection! It's a {product.CategoryName} fragrance by {product.BrandName}, priced at ${product.Price}. Would you like to know more about it or see similar products?";
                }
            }

            // Check for category mentions
            foreach (var category in categories)
            {
                if (message.Contains(category.Name.ToLower()))
                {
                    var categoryProducts = await _productService.GetProductsByCategoryAsync(category.CategoryId);
                    var count = categoryProducts.Count();
                    return $"We have {count} {category.Name} fragrances in our collection. Would you like me to show you some popular options?";
                }
            }

            // Check for brand mentions
            foreach (var brand in brands)
            {
                if (message.Contains(brand.Name.ToLower()))
                {
                    var brandProducts = await _productService.GetProductsByBrandAsync(brand.BrandId);
                    var count = brandProducts.Count();
                    return $"We carry {count} fragrances from {brand.Name}. Would you like to see our {brand.Name} collection?";
                }
            }

            // Check for price range
            var priceMatch = Regex.Match(message, @"(\d+)\s*-\s*(\d+)|under\s*(\d+)|over\s*(\d+)");
            if (priceMatch.Success)
            {
                return "I can help you find perfumes in your preferred price range. What specific price range are you looking for?";
            }

            // Check for gender preference
            if (message.Contains("men") || message.Contains("male"))
            {
                var menProducts = await _productService.GetProductsByGenderAsync("Men");
                return $"We have a great selection of men's fragrances! Would you like me to show you some popular options?";
            }
            else if (message.Contains("women") || message.Contains("female"))
            {
                var womenProducts = await _productService.GetProductsByGenderAsync("Women");
                return $"We have a beautiful collection of women's fragrances! Would you like me to show you some popular options?";
            }

            return "I'd be happy to help you find the perfect fragrance! Could you tell me more about what you're looking for? For example, are you interested in a specific brand, category, or price range?";
        }

        private async Task<string> GenerateGeneralResponseAsync(string userMessage)
        {
            var message = userMessage.ToLower();

            // Check FAQ responses
            foreach (var faq in _faqResponses)
            {
                if (message.Contains(faq.Key))
                {
                    return faq.Value;
                }
            }

            // Greeting responses
            if (message.Contains("hello") || message.Contains("hi") || message.Contains("hey"))
            {
                return "Hello! Welcome to our perfume store. I'm here to help you find the perfect fragrance. How can I assist you today?";
            }

            if (message.Contains("thank"))
            {
                return "You're very welcome! Is there anything else I can help you with?";
            }

            if (message.Contains("bye") || message.Contains("goodbye"))
            {
                return "Thank you for chatting with us! Have a wonderful day and feel free to return if you need any assistance.";
            }

            // Default response
            return "I'm here to help you with your perfume shopping experience. You can ask me about our products, shipping, returns, or any other questions you might have. What would you like to know?";
        }

        public async Task<ChatIntent> AnalyzeIntentAsync(string message)
        {
            var lowerMessage = message.ToLower();

            foreach (var intent in _intentKeywords)
            {
                if (intent.Value.Any(keyword => lowerMessage.Contains(keyword)))
                {
                    return Enum.Parse<ChatIntent>(intent.Key);
                }
            }

            return ChatIntent.General;
        }

        public async Task<List<ChatSuggestionDto>> GetQuickSuggestionsAsync()
        {
            return new List<ChatSuggestionDto>
            {
                new() { Text = "Find men's fragrances", Action = "search", Value = "men" },
                new() { Text = "Find women's fragrances", Action = "search", Value = "women" },
                new() { Text = "Check shipping info", Action = "info", Value = "shipping" },
                new() { Text = "Return policy", Action = "info", Value = "return" },
                new() { Text = "Payment methods", Action = "info", Value = "payment" },
                new() { Text = "Track my order", Action = "order", Value = "track" }
            };
        }

        public async Task<List<ProductRecommendationDto>> GetProductRecommendationsAsync(string query, int limit = 5)
        {
            try
            {
                var products = await _productService.GetAllAsync();
                var recommendations = new List<ProductRecommendationDto>();

                var queryLower = query.ToLower();

                // Simple recommendation logic based on query keywords
                var relevantProducts = products
                    .Where(p => 
                        p.Name.ToLower().Contains(queryLower) ||
                        p.Description?.ToLower().Contains(queryLower) == true ||
                        p.BrandName?.ToLower().Contains(queryLower) == true ||
                        p.CategoryName?.ToLower().Contains(queryLower) == true ||
                        p.Gender?.ToLower().Contains(queryLower) == true)
                    .Take(limit)
                    .ToList();

                foreach (var product in relevantProducts)
                {
                    recommendations.Add(new ProductRecommendationDto
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        ImageUrl = product.ImageUrl,
                        Price = product.Price,
                        BrandName = product.BrandName,
                        CategoryName = product.CategoryName,
                        Reason = $"Matches your search for '{query}'"
                    });
                }

                // If no specific matches, return popular products
                if (!recommendations.Any())
                {
                    var popularProducts = products.OrderByDescending(p => p.Price).Take(limit);
                    foreach (var product in popularProducts)
                    {
                        recommendations.Add(new ProductRecommendationDto
                        {
                            ProductId = product.ProductId,
                            Name = product.Name,
                            ImageUrl = product.ImageUrl,
                            Price = product.Price,
                            BrandName = product.BrandName,
                            CategoryName = product.CategoryName,
                            Reason = "Popular choice"
                        });
                    }
                }

                return recommendations;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting product recommendations: {ex.Message}", ex);
                return new List<ProductRecommendationDto>();
            }
        }

        public async Task<List<ChatMessageDto>> GetChatHistoryAsync(string sessionId, int limit = 50)
        {
            try
            {
                var cacheKey = $"chat_history_{sessionId}";
                var cached = await _cachingService.GetAsync<List<ChatMessageDto>>(cacheKey);
                
                if (cached != null)
                    return cached.Take(limit).ToList();

                // In a real implementation, you'd fetch from database
                return new List<ChatMessageDto>();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting chat history: {ex.Message}", ex);
                return new List<ChatMessageDto>();
            }
        }

        public async Task<bool> SaveMessageAsync(ChatMessageDto message)
        {
            try
            {
                var cacheKey = $"chat_history_{message.SessionId}";
                var history = await _cachingService.GetAsync<List<ChatMessageDto>>(cacheKey) ?? new List<ChatMessageDto>();
                
                history.Add(message);
                
                // Keep only last 100 messages
                if (history.Count > 100)
                    history = history.Skip(history.Count - 100).ToList();

                await _cachingService.SetAsync(cacheKey, history, TimeSpan.FromHours(24));
                
                _loggingService.LogInformation($"Saved chat message for session {message.SessionId}");
                return true;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error saving chat message: {ex.Message}", ex);
                return false;
            }
        }

        public async Task<bool> ClearChatHistoryAsync(string sessionId)
        {
            try
            {
                var cacheKey = $"chat_history_{sessionId}";
                await _cachingService.RemoveAsync(cacheKey);
                return true;
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error clearing chat history: {ex.Message}", ex);
                return false;
            }
        }

        public async Task<Dictionary<string, object>> GetChatAnalyticsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            // In a real implementation, you'd fetch analytics from database
            return new Dictionary<string, object>
            {
                { "totalConversations", 0 },
                { "averageResponseTime", 0 },
                { "mostCommonIntents", new List<string>() },
                { "customerSatisfaction", 0.0 }
            };
        }

        private string GenerateSessionId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
