using System.Net;
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
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
            FunctionContext executionContext) {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            if (!executionContext.BindingContext.BindingData.TryGetValue("locationId", out var locationIdValue) ||
                    !Guid.TryParse(locationIdValue?.ToString(), out var locationId)) {
                _logger.LogDebug("Location id not provided");
               await response.WriteStringAsync("Provide locationId");
               return response;
            }
            var marked = await _stateProcessor.MarkOnline(locationId);
            if (!marked) {
                _logger.LogDebug("Location {Location} not found", locationId);
                await response.WriteStringAsync($"Location with id {locationId} not found");
            } else {
                _logger.LogDebug("Location {Location} marked as live", locationId);
                await response.WriteStringAsync($"Success: {DateTime.UtcNow}");
            }
            return response;
        }
    }
}
