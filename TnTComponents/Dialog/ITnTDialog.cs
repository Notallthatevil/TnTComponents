namespace TnTComponents.Dialog;

public interface ITnTDialog {
    TnTDialogOptions Options { get; init; }
    IReadOnlyDictionary<string, object>? Parameters { get; init; }
    Type Type { get; init; }
}