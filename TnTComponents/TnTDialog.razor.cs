using TnTComponents.Dialogs;

namespace TnTComponents;

public partial class TnTDialog {
    private readonly List<Dialog> _dialogs = [];

    public void Dispose() {
        DialogService.OnOpen -= OnOpen;
        DialogService.OnClose -= OnClose;
    }

    protected override void OnInitialized() {
        DialogService.OnOpen += OnOpen;
        DialogService.OnClose += OnClose;
    }

    private async Task OnClose(Dialog dialog) {
        _dialogs.Remove(dialog);
        await Task.Run(() => StateHasChanged());
    }

    private async Task OnOpen(Dialog dialog) {
        _dialogs.Add(dialog);
        await Task.Run(() => StateHasChanged());
    }
}