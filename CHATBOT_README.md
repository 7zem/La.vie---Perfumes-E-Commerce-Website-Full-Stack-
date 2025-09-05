# 🤖 Perfume Store AI Chatbot

A modern, intelligent chatbot for your perfume e-commerce website that provides instant customer support, product recommendations, and personalized assistance.

## ✨ Features

### 🧠 AI-Powered Intelligence
- **Smart Intent Recognition**: Automatically detects customer intent (product search, support, orders, etc.)
- **Contextual Responses**: Provides relevant answers based on conversation context
- **Product Knowledge**: Deep understanding of your perfume catalog, brands, and categories
- **Natural Language Processing**: Understands various ways customers phrase their questions

### 🛍️ Product Recommendations
- **Smart Matching**: Recommends products based on customer queries and preferences
- **Category Suggestions**: Suggests relevant perfume categories (men's, women's, unisex, etc.)
- **Brand Recognition**: Identifies and provides information about specific brands
- **Price Range Filtering**: Helps customers find perfumes within their budget

### 💬 Customer Support
- **24/7 Availability**: Always ready to help customers
- **FAQ Handling**: Instant answers to common questions about shipping, returns, payments
- **Order Tracking**: Helps customers check order status and shipping information
- **Account Support**: Assists with login, registration, and account-related issues

### 🎨 Modern UI/UX
- **Floating Widget**: Non-intrusive chat button that expands into a full conversation interface
- **Mobile Responsive**: Works perfectly on all devices and screen sizes
- **Smooth Animations**: Professional typing indicators and smooth transitions
- **Quick Suggestions**: Clickable suggestion buttons for common queries
- **Product Cards**: Visual product recommendations with images and details

## 🚀 Quick Start

### 1. Backend Setup

The chatbot is already integrated into your ASP.NET Core API. The following components are included:

- **ChatbotService**: Core AI logic and response generation
- **ChatbotController**: REST API endpoints for chat functionality
- **Chat DTOs**: Data transfer objects for chat messages and responses

### 2. Frontend Integration

#### Option A: Floating Widget (Recommended)

Add this to any webpage where you want the chatbot:

```html
<!-- Add the chatbot widget script -->
<script src="/chatbot-widget.js"></script>

<!-- Initialize the chatbot -->
<script>
    new PerfumeChatbotWidget({
        apiUrl: 'http://localhost:5080/api/chatbot',
        position: 'bottom-right', // or 'bottom-left'
        title: 'Perfume Assistant',
        subtitle: 'Ask me anything!'
    });
</script>
```

#### Option B: Full-Page Chat Interface

For a dedicated chat page, use the full interface:

```html
<!-- Include the full chatbot interface -->
<iframe src="/chatbot.html" width="100%" height="600px" frameborder="0"></iframe>
```

### 3. Configuration Options

```javascript
new PerfumeChatbotWidget({
    // API endpoint
    apiUrl: 'http://localhost:5080/api/chatbot',
    
    // Widget position
    position: 'bottom-right', // 'bottom-right', 'bottom-left'
    
    // Customization
    title: 'Perfume Assistant',
    subtitle: 'Ask me anything!',
    
    // Theme (future feature)
    theme: 'default'
});
```

## 📡 API Endpoints

### POST `/api/chatbot/chat`
Send a message to the chatbot and get a response.

**Request:**
```json
{
    "message": "Hello, I need help finding a men's perfume",
    "sessionId": "optional-session-id",
    "userId": "optional-user-id",
    "context": {
        "currentPage": "product-page",
        "currentProductId": 123
    }
}
```

**Response:**
```json
{
    "response": "I'd be happy to help you find the perfect men's fragrance!",
    "suggestions": [
        {
            "text": "Find men's fragrances",
            "action": "search",
            "value": "men"
        }
    ],
    "productRecommendations": [
        {
            "productId": 1,
            "name": "Dior Sauvage",
            "imageUrl": "/images/sauvage.jpg",
            "price": 120,
            "brandName": "Dior",
            "categoryName": "Men's Perfumes",
            "reason": "Popular men's fragrance"
        }
    ],
    "sessionId": "generated-session-id",
    "isTyping": false,
    "intent": 1
}
```

### GET `/api/chatbot/suggestions`
Get quick suggestion buttons for common queries.

### GET `/api/chatbot/recommendations?query=men&limit=5`
Get product recommendations based on a search query.

### GET `/api/chatbot/history/{sessionId}`
Get chat history for a specific session.

### DELETE `/api/chatbot/history/{sessionId}`
Clear chat history for a session.

### GET `/api/chatbot/analytics` (Admin only)
Get chatbot analytics and usage statistics.

## 🧠 How It Works

### Intent Recognition
The chatbot automatically categorizes customer messages into different intents:

- **ProductSearch**: "Find me a perfume", "Looking for men's fragrances"
- **OrderStatus**: "Where is my order?", "Track my package"
- **PaymentHelp**: "How do I pay?", "Payment methods"
- **ShippingInfo**: "Shipping costs", "Delivery time"
- **ReturnPolicy**: "Can I return this?", "Return policy"
- **AccountHelp**: "Forgot password", "Create account"
- **TechnicalSupport**: "Website not working", "Error message"

### Response Generation
1. **Intent Analysis**: Determines what the customer is asking about
2. **Context Processing**: Considers current page, previous messages, user preferences
3. **Knowledge Base Lookup**: Searches product catalog, FAQ, and policies
4. **Response Generation**: Creates personalized, helpful responses
5. **Product Matching**: Finds relevant products to recommend
6. **Suggestion Creation**: Generates helpful follow-up suggestions

### Product Recommendations
The chatbot can recommend products based on:
- **Direct mentions**: "Show me Chanel N°5"
- **Category preferences**: "Men's fragrances", "Women's perfumes"
- **Brand preferences**: "Dior perfumes", "Tom Ford fragrances"
- **Price range**: "Under $100", "Expensive perfumes"
- **Occasion**: "Gift for girlfriend", "Work fragrance"

## 🎯 Use Cases

### Customer Support
- **Shipping Questions**: "How long does shipping take?"
- **Return Inquiries**: "Can I return this perfume?"
- **Payment Issues**: "What payment methods do you accept?"
- **Account Problems**: "I forgot my password"

### Product Discovery
- **General Search**: "Find me a perfume"
- **Specific Requests**: "Show me men's fragrances under $100"
- **Brand Exploration**: "What Dior perfumes do you have?"
- **Gift Suggestions**: "Looking for a gift for my wife"

### Shopping Assistance
- **Product Information**: "Tell me about Chanel N°5"
- **Comparison Help**: "Which is better, Dior or Chanel?"
- **Size Questions**: "What sizes do you have?"
- **Availability**: "Is this perfume in stock?"

## 🔧 Customization

### Adding New Responses
Edit the `_faqResponses` dictionary in `ChatbotService.cs`:

```csharp
private readonly Dictionary<string, string> _faqResponses = new()
{
    { "shipping", "We offer free shipping on orders over $50..." },
    { "return", "We have a 30-day return policy..." },
    { "new-topic", "Your custom response here..." }
};
```

### Adding New Intents
Update the `_intentKeywords` dictionary:

```csharp
private readonly Dictionary<string, string[]> _intentKeywords = new()
{
    { "NewIntent", new[] { "keyword1", "keyword2", "keyword3" } }
};
```

### Customizing Product Logic
Modify the `GenerateProductResponseAsync` method to add custom product matching logic.

## 📊 Analytics & Monitoring

The chatbot provides analytics on:
- **Conversation Count**: Total number of chat sessions
- **Response Time**: Average time to generate responses
- **Intent Distribution**: Most common customer intents
- **Customer Satisfaction**: Success rate of conversations

Access analytics via the admin endpoint: `GET /api/chatbot/analytics`

## 🚀 Deployment

### Production Setup
1. **Update API URL**: Change `apiUrl` in the widget configuration to your production domain
2. **Enable HTTPS**: Ensure your API supports HTTPS for secure communication
3. **Configure CORS**: Set up proper CORS policies for your domain
4. **Database Storage**: Consider implementing persistent chat history storage
5. **Monitoring**: Set up logging and monitoring for chatbot performance

### Environment Variables
```json
{
  "ChatbotSettings": {
    "EnableAnalytics": true,
    "MaxResponseTime": 5000,
    "SessionTimeout": 3600,
    "MaxSuggestions": 6,
    "MaxRecommendations": 5
  }
}
```

## 🧪 Testing

### Manual Testing
1. Start the API: `dotnet run --project Perfumes.API`
2. Open the demo page: `http://localhost:5080/chatbot-demo.html`
3. Test various scenarios:
   - Greetings: "Hello", "Hi there"
   - Product queries: "Find men's perfumes", "Show me Chanel"
   - Support questions: "Shipping info", "Return policy"
   - Complex requests: "I need a gift for my girlfriend under $200"

### API Testing
```bash
# Test basic chat
curl -X POST http://localhost:5080/api/chatbot/chat \
  -H "Content-Type: application/json" \
  -d '{"message":"Hello"}'

# Test product recommendations
curl -X GET "http://localhost:5080/api/chatbot/recommendations?query=men&limit=3"

# Test suggestions
curl -X GET http://localhost:5080/api/chatbot/suggestions
```

## 🔮 Future Enhancements

### Planned Features
- **Multi-language Support**: Chat in different languages
- **Voice Integration**: Voice-to-text and text-to-speech
- **Advanced AI**: Integration with external AI services (OpenAI, Azure Cognitive Services)
- **Personalization**: User preference learning and personalized recommendations
- **Integration**: Connect with CRM, order management, and inventory systems
- **Advanced Analytics**: Sentiment analysis, conversation flow tracking

### AI Improvements
- **Machine Learning**: Train on customer conversations to improve responses
- **Natural Language Understanding**: Better understanding of complex queries
- **Context Memory**: Remember customer preferences across sessions
- **Predictive Responses**: Anticipate customer needs based on behavior

## 📞 Support

For technical support or questions about the chatbot:
- Check the API logs for error messages
- Review the conversation history for debugging
- Test individual endpoints for functionality
- Monitor performance metrics for optimization

---

**🎉 Your perfume store now has a professional, intelligent chatbot that will enhance customer experience and boost sales!**
