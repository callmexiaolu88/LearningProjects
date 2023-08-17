using System;
using NLog;

namespace nlogToFluentdbit
{
    class Program
    {
        static ILogger _logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            _logger.Info("this is a test message");
            _logger.Info("this is a {info} message", "Info");
            _logger.Debug("this is a {debug} message", "Debug");
            _logger.Warn("this is a {warn} message", "Warn");
            _logger.Error("this is a {error} message", "Error");
        }
    }
}
