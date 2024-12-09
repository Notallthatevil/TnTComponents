using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
///     Represents a typeahead component that provides suggestions as the user types.
/// </summary>
/// <typeparam name="TItem">The type of the items to be suggested.</typeparam>
public partial class TnTTypeahead<TItem> {

    /// <summary>
    ///     Gets or sets the appearance of the form.
    /// </summary>
    [Parameter]
    public FormAppearance Appearance { get; set; }

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainerHighest;

    /// <summary>
    ///     Gets or sets the debounce delay in milliseconds.
    /// </summary>
    [Parameter]
    public int DebounceMilliseconds { get; set; } = 300;

    [Parameter]
    public bool Disabled { get; set; }

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-typeahead")
        .Build();

    [Parameter]
    public string? ElementName { get; set; }

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public bool EnableRipple { get; set; }

    /// <summary>
    ///     Gets or sets the callback to be invoked when an item is selected.
    /// </summary>
    [Parameter]
    public EventCallback<TItem> ItemSelectedCallback { get; set; }

    /// <summary>
    ///     Gets or sets the function to lookup items based on the search text.
    /// </summary>
    [Parameter, EditorRequired]
    public Func<string?, CancellationToken, Task<IEnumerable<TItem>>> ItemsLookupFunc { get; set; } = default!;

    /// <summary>
    ///     Gets or sets the label for the input.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    [Parameter]
    public TnTColor? OnTintColor { get; set; }

    /// <summary>
    ///     Gets or sets the placeholder text for the input.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    ///     Gets or sets the color of the progress indicator.
    /// </summary>
    [Parameter]
    public TnTColor ProgressColor { get; set; } = TnTColor.Primary;

    /// <summary>
    ///     Gets or sets the template for rendering each result item.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? ResultTemplate { get; set; }

    /// <summary>
    ///     Gets or sets the tint color of the component.
    /// </summary>
    [Parameter]
    public TnTColor? TintColor { get; set; }

    private TItem? _focusedItem { get; set; }
    private TnTDebouncer _debouncer = default!;
    private TnTInputText _inputBox = default!;
    private IEnumerable<TItem> _items = [];
    private bool _searching;
    private string? _searchText;

    public void Dispose() {
        _debouncer?.Dispose();
        _debouncer = null!;

        GC.SuppressFinalize(this);
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();
        _debouncer = new TnTDebouncer(DebounceMilliseconds);
    }

    /// <summary>
    ///     Handles the selection of an item asynchronously.
    /// </summary>
    /// <param name="item">The selected item.</param>
    private async Task ItemSelectedAsync(TItem item) {
        _searchText = null;
        _items = [];
        _searching = false;
        _focusedItem = default;
        await ItemSelectedCallback.InvokeAsync(item);
        await _inputBox.SetFocusAsync();
    }

    /// <summary>
    ///     Performs a search asynchronously based on the provided search value.
    /// </summary>
    /// <param name="searchValue">The search value.</param>
    private async Task SearchAsync(string? searchValue) {
        _searching = true;
        _items = [];
        _focusedItem = default;
        await _debouncer.DebounceAsync(async token => {
            var result = await ItemsLookupFunc.Invoke(searchValue, token);
            _items = result;
            _focusedItem = _items.FirstOrDefault();
            _searching = false;
        });
    }

    /// <summary>
    ///     Handles keyboard events to select or shift focus.
    /// </summary>
    /// <param name="args">The keyboard event arguments.</param>
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
                case "Escape": {
                        _items = [];
                        _searching = false;
                        _searchText = null;
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
