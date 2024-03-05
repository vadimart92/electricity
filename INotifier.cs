namespace electricity;

public enum LocationState
{
    Online, Offline
}

public interface INotifier
{
    Task Notify(Location location, LocationState state);
}
