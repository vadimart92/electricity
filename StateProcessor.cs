using Microsoft.EntityFrameworkCore;

namespace electricity;

class StateProcessor : IStateProcessor
{
    private readonly INotifier _notifier;
    private readonly DatabaseContext _databaseContext;

    public StateProcessor(INotifier notifier, DatabaseContext databaseContext) {
        _notifier = notifier;
        _databaseContext = databaseContext;
    }

    public async Task<bool> MarkOnline(Guid locationId) {
        var location = await _databaseContext.Locations.FindAsync(locationId);
        if (location == null) {
            return false;
        }
        if (!location.IsOnline) {
            location.IsOnline = true;
            await _notifier.Notify(location, LocationState.Online);
        }
        location.LastAlive = DateTime.UtcNow;
        await _databaseContext.SaveChangesAsync();
        return true;
    }

    public async Task MarkOfflineWhereNeeded() {
        var limitDate = DateTime.UtcNow.AddMinutes(-1 * 5);
        var locationsToMark = await _databaseContext.Locations
            .Where(h => h.IsOnline && h.LastAlive < limitDate).ToListAsync();
        if (!locationsToMark.Any()) {
            return;
        }
        foreach (var location in locationsToMark) {
            location.IsOnline = false;
            await _notifier.Notify(location, LocationState.Offline);
        }
        await _databaseContext.SaveChangesAsync();
    }
}
