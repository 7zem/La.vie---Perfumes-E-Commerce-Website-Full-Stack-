// Perfume Store Chatbot Widget
// Add this script to any webpage to embed the chatbot

(function() {
    'use strict';

    class ChatbotWidget {
        constructor(options = {}) {
            this.options = {
                apiUrl: options.apiUrl || 'http://localhost:5036/api/chatbot',
                position: options.position || 'bottom-right',
                theme: options.theme || 'default',
                title: options.title || 'Perfume Assistant',
                subtitle: options.subtitle || 'Ask me anything!',
                ...options
            };

            this.isOpen = false;
            this.sessionId = this.generateSessionId();
            this.isTyping = false;
            this.messageHistory = [];

            this.init();
        }

        generateSessionId() {
            return 'widget_' + Date.now() + '_' + Math.random().toString(36).substr(2, 9);
        }

        init() {
            this.createStyles();
            this.createWidget();
            this.bindEvents();
            this.loadQuickSuggestions();
        }

        createStyles() {
            const style = document.createElement('style');
            style.textContent = `
                .chatbot-widget {
                    position: fixed;
                    z-index: 9999;
                    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
                }

                .chatbot-widget.bottom-right {
                    bottom: 20px;
                    right: 20px;
                }

                .chatbot-widget.bottom-left {
                    bottom: 20px;
                    left: 20px;
                }

                .chatbot-toggle {
                    width: 60px;
                    height: 60px;
                    border-radius: 50%;
                    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                    border: none;
                    color: white;
                    font-size: 24px;
                    cursor: pointer;
                    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
                    transition: all 0.3s ease;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                }

                .chatbot-toggle:hover {
                    transform: scale(1.1);
                    box-shadow: 0 6px 25px rgba(0, 0, 0, 0.2);
                }

                .chatbot-container {
                    position: absolute;
                    bottom: 80px;
                    right: 0;
                    width: 350px;
                    height: 500px;
                    background: white;
                    border-radius: 20px;
                    box-shadow: 0 10px 40px rgba(0, 0, 0, 0.15);
                    display: none;
                    flex-direction: column;
                    overflow: hidden;
                }

                .chatbot-container.open {
                    display: flex;
                }

                .chatbot-header {
                    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                    color: white;
                    padding: 15px 20px;
                    display: flex;
                    align-items: center;
                    justify-content: space-between;
                }

                .chatbot-header h3 {
                    margin: 0;
                    font-size: 16px;
                    font-weight: 600;
                }

                .chatbot-header p {
                    margin: 0;
                    font-size: 12px;
                    opacity: 0.9;
                }

                .chatbot-close {
                    background: none;
                    border: none;
                    color: white;
                    font-size: 20px;
                    cursor: pointer;
                    padding: 0;
                    width: 30px;
                    height: 30px;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    border-radius: 50%;
                    transition: background 0.3s ease;
                }

                .chatbot-close:hover {
                    background: rgba(255, 255, 255, 0.2);
                }

                .chatbot-messages {
                    flex: 1;
                    padding: 20px;
                    overflow-y: auto;
                    background: #f8f9fa;
                }

                .chatbot-message {
                    margin-bottom: 15px;
                    display: flex;
                    align-items: flex-start;
                }

                .chatbot-message.user {
                    justify-content: flex-end;
                }

                .chatbot-message.bot {
                    justify-content: flex-start;
                }

                .chatbot-message-content {
                    max-width: 80%;
                    padding: 10px 14px;
                    border-radius: 18px;
                    font-size: 14px;
                    line-height: 1.4;
                }

                .chatbot-message.user .chatbot-message-content {
                    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                    color: white;
                    border-bottom-right-radius: 4px;
                }

                .chatbot-message.bot .chatbot-message-content {
                    background: white;
                    color: #333;
                    border: 1px solid #e0e0e0;
                    border-bottom-left-radius: 4px;
                }

                .chatbot-suggestions {
                    display: flex;
                    flex-wrap: wrap;
                    gap: 6px;
                    margin-top: 8px;
                }

                .chatbot-suggestion {
                    background: white;
                    border: 1px solid #667eea;
                    color: #667eea;
                    padding: 6px 10px;
                    border-radius: 15px;
                    font-size: 11px;
                    cursor: pointer;
                    transition: all 0.3s ease;
                }

                .chatbot-suggestion:hover {
                    background: #667eea;
                    color: white;
                }

                .chatbot-product-card {
                    background: white;
                    border: 1px solid #e0e0e0;
                    border-radius: 8px;
                    padding: 10px;
                    margin-top: 10px;
                    display: flex;
                    align-items: center;
                    gap: 10px;
                    cursor: pointer;
                    transition: box-shadow 0.3s ease;
                }

                .chatbot-product-card:hover {
                    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
                }

                .chatbot-product-image {
                    width: 40px;
                    height: 40px;
                    border-radius: 6px;
                    background: #f0f0f0;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    font-size: 10px;
                    color: #999;
                }

                .chatbot-product-info {
                    flex: 1;
                }

                .chatbot-product-name {
                    font-weight: 600;
                    font-size: 12px;
                    margin-bottom: 2px;
                }

                .chatbot-product-details {
                    font-size: 10px;
                    color: #666;
                    margin-bottom: 2px;
                }

                .chatbot-product-price {
                    font-weight: 600;
                    color: #667eea;
                    font-size: 12px;
                }

                .chatbot-input {
                    padding: 15px 20px;
                    background: white;
                    border-top: 1px solid #e0e0e0;
                }

                .chatbot-input-container {
                    display: flex;
                    gap: 8px;
                    align-items: center;
                }

                .chatbot-input-field {
                    flex: 1;
                    padding: 10px 14px;
                    border: 1px solid #e0e0e0;
                    border-radius: 20px;
                    font-size: 14px;
                    outline: none;
                    transition: border-color 0.3s ease;
                }

                .chatbot-input-field:focus {
                    border-color: #667eea;
                }

                .chatbot-send-btn {
                    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                    color: white;
                    border: none;
                    width: 35px;
                    height: 35px;
                    border-radius: 50%;
                    cursor: pointer;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    transition: transform 0.3s ease;
                }

                .chatbot-send-btn:hover {
                    transform: scale(1.1);
                }

                .chatbot-send-btn:disabled {
                    opacity: 0.5;
                    cursor: not-allowed;
                    transform: none;
                }

                .chatbot-typing {
                    display: flex;
                    align-items: center;
                    gap: 4px;
                    padding: 10px 14px;
                    background: white;
                    border: 1px solid #e0e0e0;
                    border-radius: 18px;
                    border-bottom-left-radius: 4px;
                    margin-bottom: 15px;
                }

                .chatbot-typing-dots {
                    display: flex;
                    gap: 2px;
                }

                .chatbot-typing-dot {
                    width: 5px;
                    height: 5px;
                    background: #999;
                    border-radius: 50%;
                    animation: chatbotTyping 1.4s infinite ease-in-out;
                }

                .chatbot-typing-dot:nth-child(1) { animation-delay: -0.32s; }
                .chatbot-typing-dot:nth-child(2) { animation-delay: -0.16s; }

                @keyframes chatbotTyping {
                    0%, 80%, 100% { transform: scale(0); }
                    40% { transform: scale(1); }
                }

                .chatbot-welcome {
                    text-align: center;
                    color: #666;
                    font-style: italic;
                    margin-bottom: 15px;
                    font-size: 13px;
                }

                @media (max-width: 480px) {
                    .chatbot-container {
                        width: calc(100vw - 40px);
                        height: calc(100vh - 120px);
                        bottom: 80px;
                        right: 20px;
                    }
                }
            `;
            document.head.appendChild(style);
        }

        createWidget() {
            const widget = document.createElement('div');
            widget.className = `chatbot-widget ${this.options.position}`;
            widget.innerHTML = `
                <button class="chatbot-toggle" aria-label="Open chat assistant">
                    💬
                </button>
                <div class="chatbot-container">
                    <div class="chatbot-header">
                        <div>
                            <h3>${this.options.title}</h3>
                            <p>${this.options.subtitle}</p>
                        </div>
                        <button class="chatbot-close" aria-label="Close chat">×</button>
                    </div>
                    <div class="chatbot-messages">
                        <div class="chatbot-welcome">
                            👋 Welcome! I'm here to help you find the perfect fragrance.
                        </div>
                    </div>
                    <div class="chatbot-input">
                        <div class="chatbot-input-container">
                            <input type="text" class="chatbot-input-field" placeholder="Type your message..." maxlength="500">
                            <button class="chatbot-send-btn" aria-label="Send message">
                                <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                                    <line x1="22" y1="2" x2="11" y2="13"></line>
                                    <polygon points="22,2 15,22 11,13 2,9"></polygon>
                                </svg>
                            </button>
                        </div>
                    </div>
                </div>
            `;
            document.body.appendChild(widget);

            this.widget = widget;
            this.toggleBtn = widget.querySelector('.chatbot-toggle');
            this.container = widget.querySelector('.chatbot-container');
            this.closeBtn = widget.querySelector('.chatbot-close');
            this.messagesContainer = widget.querySelector('.chatbot-messages');
            this.inputField = widget.querySelector('.chatbot-input-field');
            this.sendBtn = widget.querySelector('.chatbot-send-btn');
        }

        bindEvents() {
            this.toggleBtn.addEventListener('click', () => this.toggle());
            this.closeBtn.addEventListener('click', () => this.close());
            this.inputField.addEventListener('keypress', (e) => {
                if (e.key === 'Enter' && !e.shiftKey) {
                    e.preventDefault();
                    this.sendMessage();
                }
            });
            this.inputField.addEventListener('input', () => {
                this.sendBtn.disabled = !this.inputField.value.trim();
            });
            this.sendBtn.addEventListener('click', () => this.sendMessage());
        }

        toggle() {
            if (this.isOpen) {
                this.close();
            } else {
                this.open();
            }
        }

        open() {
            this.isOpen = true;
            this.container.classList.add('open');
            this.inputField.focus();
        }

        close() {
            this.isOpen = false;
            this.container.classList.remove('open');
        }

        async sendMessage() {
            const message = this.inputField.value.trim();
            if (!message || this.isTyping) return;

            this.addMessage(message, 'user');
            this.inputField.value = '';
            this.sendBtn.disabled = true;

            this.showTyping();

            try {
                const response = await fetch(`${this.options.apiUrl}/chat`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        message: message,
                        sessionId: this.sessionId
                    })
                });

                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }

                const data = await response.json();
                
                this.hideTyping();
                this.addMessage(data.response, 'bot', data.suggestions, data.productRecommendations);
                
            } catch (error) {
                console.error('Error sending message:', error);
                this.hideTyping();
                this.addMessage('Sorry, I\'m having trouble connecting right now. Please try again in a moment.', 'bot');
            }
        }

        addMessage(content, type, suggestions = [], productRecommendations = []) {
            const messageDiv = document.createElement('div');
            messageDiv.className = `chatbot-message ${type}`;

            const messageContent = document.createElement('div');
            messageContent.className = 'chatbot-message-content';
            messageContent.textContent = content;

            messageDiv.appendChild(messageContent);

            if (suggestions && suggestions.length > 0) {
                const suggestionsDiv = document.createElement('div');
                suggestionsDiv.className = 'chatbot-suggestions';
                
                suggestions.forEach(suggestion => {
                    const btn = document.createElement('button');
                    btn.className = 'chatbot-suggestion';
                    btn.textContent = suggestion.text;
                    btn.addEventListener('click', () => {
                        this.inputField.value = suggestion.text;
                        this.sendMessage();
                    });
                    suggestionsDiv.appendChild(btn);
                });
                
                messageDiv.appendChild(suggestionsDiv);
            }

            if (productRecommendations && productRecommendations.length > 0) {
                const recommendationsDiv = document.createElement('div');
                
                productRecommendations.forEach(product => {
                    const productCard = document.createElement('div');
                    productCard.className = 'chatbot-product-card';
                    productCard.innerHTML = `
                        <div class="chatbot-product-image">${product.imageUrl ? `<img src="${product.imageUrl}" alt="${product.name}">` : '🫙'}</div>
                        <div class="chatbot-product-info">
                            <div class="chatbot-product-name">${product.name}</div>
                            <div class="chatbot-product-details">${product.brandName} • ${product.categoryName}</div>
                            <div class="chatbot-product-price">$${product.price}</div>
                        </div>
                    `;
                    productCard.addEventListener('click', () => {
                        window.open(`/product/${product.productId}`, '_blank');
                    });
                    recommendationsDiv.appendChild(productCard);
                });
                
                messageDiv.appendChild(recommendationsDiv);
            }

            this.messagesContainer.appendChild(messageDiv);
            this.scrollToBottom();
        }

        showTyping() {
            this.isTyping = true;
            const typingDiv = document.createElement('div');
            typingDiv.className = 'chatbot-typing';
            typingDiv.id = 'chatbotTyping';
            typingDiv.innerHTML = `
                <div class="chatbot-typing-dots">
                    <div class="chatbot-typing-dot"></div>
                    <div class="chatbot-typing-dot"></div>
                    <div class="chatbot-typing-dot"></div>
                </div>
                <span style="font-size: 12px; color: #666;">Typing...</span>
            `;
            this.messagesContainer.appendChild(typingDiv);
            this.scrollToBottom();
        }

        hideTyping() {
            this.isTyping = false;
            const typingIndicator = document.getElementById('chatbotTyping');
            if (typingIndicator) {
                typingIndicator.remove();
            }
        }

        async loadQuickSuggestions() {
            try {
                const response = await fetch(`${this.options.apiUrl}/suggestions`);
                if (response.ok) {
                    const suggestions = await response.json();
                    this.addWelcomeSuggestions(suggestions);
                }
            } catch (error) {
                console.error('Error loading suggestions:', error);
            }
        }

        addWelcomeSuggestions(suggestions) {
            const suggestionsDiv = document.createElement('div');
            suggestionsDiv.className = 'chatbot-suggestions';
            suggestionsDiv.style.marginTop = '15px';
            
            suggestions.forEach(suggestion => {
                const btn = document.createElement('button');
                btn.className = 'chatbot-suggestion';
                btn.textContent = suggestion.text;
                btn.addEventListener('click', () => {
                    this.inputField.value = suggestion.text;
                    this.sendMessage();
                });
                suggestionsDiv.appendChild(btn);
            });
            
            this.messagesContainer.appendChild(suggestionsDiv);
        }

        scrollToBottom() {
            this.messagesContainer.scrollTop = this.messagesContainer.scrollHeight;
        }
    }

    // Make it globally available
    window.PerfumeChatbotWidget = ChatbotWidget;

    // Auto-initialize if data attributes are present
    document.addEventListener('DOMContentLoaded', () => {
        const script = document.currentScript || document.querySelector('script[src*="chatbot-widget.js"]');
        if (script) {
            const apiUrl = script.getAttribute('data-api-url');
            const position = script.getAttribute('data-position');
            const title = script.getAttribute('data-title');
            const subtitle = script.getAttribute('data-subtitle');

            if (apiUrl || position || title || subtitle) {
                new ChatbotWidget({
                    apiUrl: apiUrl,
                    position: position,
                    title: title,
                    subtitle: subtitle
                });
            }
        }
    });

})();
