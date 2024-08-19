namespace TnTComponents.Dialog;

public interface ITnTDialog {
    DialogResult DialogResult { get; set; }
    TnTDialogOptions Options { get; init; }
    IReadOnlyDictionary<string, object?>? Parameters { get; init; }
    Type Type { get; init; }
    Task CloseAsync();
    string ElementId { get; init; }
}