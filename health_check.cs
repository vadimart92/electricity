using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace electricity
{
    public class health_check
    {
        private readonly IStateProcessor _stateProcessor;
        private readonly ILogger<health_check> _logger;

        public health_check(ILoggerFactory loggerFactory, IStateProcessor stateProcessor) {
            _stateProcessor = stateProcessor;
            _logger = loggerFactory.CreateLogger<health_check>();
        }
        
        [Function("health_check")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req) {
            if (!Guid.TryParse(req.Query["locationId"].ToString(), out var locationId)) {
                _logger.LogDebug("Location id not provided");
               return new OkObjectResult("Provide locationId");
            }
            var marked = await _stateProcessor.MarkOnline(locationId);
            if (!marked) {
                _logger.LogDebug("Location {Location} not found", locationId);
                return new OkObjectResult($"Location with id {locationId} not found");
            } else {
                _logger.LogDebug("Location {Location} marked as live", locationId);
                return new OkObjectResult($"Success: {DateTime.UtcNow}");
            }
        }
    }
}
