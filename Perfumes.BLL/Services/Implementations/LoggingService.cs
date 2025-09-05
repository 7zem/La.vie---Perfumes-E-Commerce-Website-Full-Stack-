using Microsoft.Extensions.Logging;
using Perfumes.BLL.Services.Interfaces;

namespace Perfumes.BLL.Services.Implementations
{
    public class LoggingService(ILogger<LoggingService> logger) : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger = logger;

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogError(string message, Exception? exception = null)
        {
            if (exception != null)
                _logger.LogError(exception, message);
            else
                _logger.LogError(message);
        }

        public void LogDebug(string message)
        {
            _logger.LogDebug(message);
        }

        public void LogCritical(string message, Exception? exception = null)
        {
            if (exception != null)
                _logger.LogCritical(exception, message);
            else
                _logger.LogCritical(message);
        }
    }
} 