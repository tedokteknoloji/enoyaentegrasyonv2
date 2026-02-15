using System;
using System.IO;
using System.Text;
using ENOYAEntegrasyonV2.Services.Interfaces;

namespace ENOYAEntegrasyonV2.Services.Infrastructure
{
    /// <summary>
    /// Dosya tabanlı logger servisi
    /// </summary>
    public class FileLoggerService : ILoggerService
    {
        private readonly string _logDirectory;
        private readonly object _lockObject = new object();

        public FileLoggerService()
        {
            _logDirectory = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "LogFiles"
            );

            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }
        
        public FileLoggerService(string logDirectory)
        {
            _logDirectory = logDirectory ?? Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "LogFiles"
            );

            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        private void WriteLog(string level, string message, Exception exception = null)
        {
            try
            {
                lock (_lockObject)
                {
                    var logFile = Path.Combine(
                        _logDirectory,
                        $"ENOYAEntegrasyonV2-{Environment.MachineName}-{Environment.UserName}.log"
                    );

                    var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    var logEntry = new StringBuilder();
                    logEntry.AppendLine($"[{timestamp}] [{level}] {message}");

                    if (exception != null)
                    {
                        logEntry.AppendLine($"Exception: {exception.Message}");
                        logEntry.AppendLine($"StackTrace: {exception.StackTrace}");
                        if (exception.InnerException != null)
                        {
                            logEntry.AppendLine($"InnerException: {exception.InnerException.Message}");
                        }
                    }

                    File.AppendAllText(logFile, logEntry.ToString(), Encoding.UTF8);
                }
            }
            catch
            {
                // Logger hatası uygulamayı durdurmamalı
            }
        }

        public void LogDebug(string message)
        {
            WriteLog("DEBUG", message);
        }

        public void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }

        public void LogWarning(string message)
        {
            WriteLog("WARNING", message);
        }

        public void LogError(string message, Exception exception = null)
        {
            WriteLog("ERROR", message, exception);
        }
    }
}

