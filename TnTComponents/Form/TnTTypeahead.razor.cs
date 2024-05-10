using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTTypeahead<TItem> {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public int DebounceMilliseconds { get; set; } = 300;

    [Parameter, EditorRequired]
    public EventCallback<TItem> ItemSelected { get; set; }

    [Parameter, EditorRequired]
    public Func<string?, CancellationToken, Task<IEnumerable<TItem>>> ItemsLookupFunc { get; set; } = default!;

    [Parameter]
    public EventCallback ItemsSetCallback { get; set; }

    [Parameter]
    public string? Label { get; set; }

    [Parameter]
    public TnTColor? LabelBackgroundColor { get; set; }

    [Parameter]
    public TnTColor? ResultsViewBackgroundColor { get; set; } = TnTColor.Surface;

    [Parameter]
    public TnTBorderRadius? ResultsViewBorderRadius { get; set; } = new(2);

    [Parameter]
    public int ResultsViewElevation { get; set; } = 2;

    [Parameter]
    public TnTColor? ResultsViewTextColor { get; set; } = TnTColor.OnSurface;

    [Parameter]
    public RenderFragment<TItem>? ResultTemplate { get; set; }

    [Parameter]
    public TnTColor SearchProgressColor { get; set; } = TnTColor.Secondary;

    [Parameter]
    public TnTIcon StartIcon { get; set; } = new MaterialIcon(MaterialIcon.Search);

    private TnTDebouncer _debouncer = new();
    private TnTInputText? _inputTextBox;
    private IEnumerable<TItem> _items = [];

    private bool _searching;
    private string? _searchText;

    private async Task ItemSelectedAsync(TItem item) {
        _searchText = "";
        _items = [];
        _searching = false;
        if (_inputTextBox is not null) {
            await _inputTextBox.Element.FocusAsync();
        }
        await ItemSelected.InvokeAsync(item);
    }

    private async Task KeyPressedAsync(KeyboardEventArgs args) {
        if (args.Key == "Escape") {
            _items = [];
            _searching = false;
        }
        else if (args.Key == "Enter" && _items.Any()) {
            await ItemSelectedAsync(_items.First());
        }
    }

    private async Task SearchAsync(string value) {
        if (!string.IsNullOrWhiteSpace(value)) {
            _searching = true;
            await _debouncer.DebounceAsync(DebounceMilliseconds, async token => {
                _items = await ItemsLookupFunc(value, token);
                _searching = false;
            });
        }
        else {
            _searching = false;
            await _debouncer.CancelAsync();
            _items = [];
        }
        StateHasChanged();
        await ItemsSetCallback.InvokeAsync();
    }
}