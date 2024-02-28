namespace TnTComponents.Storage;

public readonly struct ChangedEventArgs {
    public string Key { get; init; }
    public object? NewValue { get; init; }
    public object? OldValue { get; init; }
}