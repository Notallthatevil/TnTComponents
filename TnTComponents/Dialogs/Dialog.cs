namespace TnTComponents.Dialogs;

public class Dialog {
    public Type Type { get; set; }
    public string Title { get; set; }
    public IReadOnlyDictionary<string, object>? Parameters { get; set; }
    public DialogOptions Options { get; set; } = new();
}