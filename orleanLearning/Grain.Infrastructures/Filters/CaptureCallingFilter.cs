using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;

namespace Grain.Infrastructures.Filters
{
    public class CaptureCallingFilter : IIncomingGrainCallFilter, IOutgoingGrainCallFilter
    {
        readonly ILogger<CaptureCallingFilter> _logger;

        public CaptureCallingFilter(ILogger<CaptureCallingFilter> logger)
        {
            _logger = logger;
        }
        public Task Invoke(IIncomingGrainCallContext context)
        {
            _logger.LogInformation($"Entry [{context.Grain.ToString()}]:{context.ImplementationMethod.Name}: {DateTime.Now}");
            return Task.CompletedTask;
        }

        [Obsolete]
        public Task Invoke(IOutgoingGrainCallContext context)
        {
            _logger.LogInformation($"Exit {context.Method.Name}: {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
