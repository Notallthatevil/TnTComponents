namespace TnTComponents.Storage;

public class ChangingEventArgs {
    public bool Cancel { get; init; }
    public required string Key { get; init; }
    public object? NewValue { get; init; }
    public object? OldValue { get; init; }
}