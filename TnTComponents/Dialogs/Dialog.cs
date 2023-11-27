namespace TnTComponents.Dialogs;

public class Dialog {
    public DialogOptions Options { get; set; } = new();
    public IReadOnlyDictionary<string, object>? Parameters { get; set; }
    public string Title { get; set; }
    public Type Type { get; set; }
}