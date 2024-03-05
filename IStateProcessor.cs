namespace electricity;

public interface IStateProcessor
{
    Task<bool> MarkOnline(Guid locationId);
    Task MarkOfflineWhereNeeded();
}
