using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTTypeahead<TItem> {

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-typeahead")
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public string? Placeholder { get; set; }

    [Parameter]
    public string? Label { get; set; }

    [Parameter]
    public TnTColor ProgressColor { get; set; } = TnTColor.Primary;

    [Parameter]
    public FormAppearance Appearance { get; set; }

    [Parameter]
    public RenderFragment<TItem>? ResultTemplate { get; set; }

    [Parameter]
    public EventCallback<TItem> ItemSelectedCallback { get; set; }
    [Parameter, EditorRequired]
    public Func<string?, CancellationToken, Task<IEnumerable<TItem>>> ItemsLookupFunc { get; set; } = default!;

    private TnTDebouncer _debouncer = new();

    [Parameter]
    public int DebounceMilliseconds { get; set; } = 300;

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainerHighest;

    private TItem? _focusedItem { get; set; }

    private string? _searchText;
    private bool _searching;

    private IEnumerable<TItem> _items = [];
    private async Task SearchAsync(string? searchValue) {
        _searching = true;
        _items = [];
        _focusedItem = default;
        await _debouncer.DebounceAsync(DebounceMilliseconds, async token => {
            var result = await ItemsLookupFunc.Invoke(searchValue, token);
            _items = result;
            _focusedItem = _items.FirstOrDefault();
            _searching = false;
        });
    }

    private async Task ItemSelectedAsync(TItem item) {
        _searchText = null;
        _items = [];
        _searching = false;
        _focusedItem = default;
        await ItemSelectedCallback.InvokeAsync(item);
    }

    private async Task SelectOrShiftFocusAsync(KeyboardEventArgs args) {
        if (_focusedItem is not null && !_focusedItem.Equals(default)) {
            switch (args.Key) {
                case "Enter":
                    if (_focusedItem != null) {
                        await ItemSelectedAsync(_focusedItem);
                    }
                    break;
                case "ArrowDown": {
                        var index = _items.ToList().IndexOf(_focusedItem);
                        if (index < _items.Count() - 1) {
                            _focusedItem = _items.ElementAt(index + 1);
                        }
                        else {
                            _focusedItem = _items.FirstOrDefault();
                        }
                        break;
                    }
                case "ArrowUp": {
                        var index = _items.ToList().IndexOf(_focusedItem);
                        if (index > 0) {
                            _focusedItem = _items.ElementAt(index - 1);
                        }
                        else {
                            _focusedItem = _items.LastOrDefault();
                        }
                        break;
                    }
                default:
                    break;
            }
        }
    }
}