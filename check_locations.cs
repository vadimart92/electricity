using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace electricity;

public class check_locations
{
    private readonly IStateProcessor _stateProcessor;

    public check_locations(IStateProcessor stateProcessor) {
        _stateProcessor = stateProcessor;
    }

    [Function("check_locations")]
    public async Task Run([TimerTrigger("0 */10 * * * *")]FunctionContext context) {
        try {
            await _stateProcessor.MarkOfflineWhereNeeded();
        } catch (Exception e) {
            context.GetLogger<check_locations>().LogError(e, "Error checking locations");
            throw;
        }
    }
}
