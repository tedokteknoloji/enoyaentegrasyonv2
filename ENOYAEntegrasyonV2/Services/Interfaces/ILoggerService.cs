using System;

namespace ENOYAEntegrasyonV2.Services.Interfaces
{
    /// <summary>
    /// Logger servisi interface
    /// </summary>
    public interface ILoggerService
    {
        void LogDebug(string message);
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message, Exception exception = null);
    }
}

