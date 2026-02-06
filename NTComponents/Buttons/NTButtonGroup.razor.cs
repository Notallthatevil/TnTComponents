using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using NTComponents.Core;

namespace NTComponents;

/// <summary>
///     Represents a Material button group built on top of the existing TnT button primitives.
/// </summary>
public partial class NTButtonGroup<TObjectType> : TnTComponentBase {

    /// <summary>
    ///     The default appearance used by buttons that are not selected.
    /// </summary>
    [Parameter]
    public ButtonAppearance Appearance { get; set; } = ButtonAppearance.Filled;

    /// <summary>
    ///     Provides an accessible label for the group container.
    /// </summary>
    [Parameter]
    public string? AriaLabel { get; set; }

    /// <summary>
    ///     The background color drawn behind every button.
    /// </summary>
    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceVariant;

    /// <summary>
    ///     The size applied to every button inside the group.
    /// </summary>
    [Parameter]
    public Size ButtonSize { get; set; } = Size.Small;

    /// <summary>
    ///     Disables every button inside the group.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    ///     When true the ripple animation is suppressed on the child buttons.
    /// </summary>
    [Parameter]
    public bool DisableRipple { get; set; }

    /// <summary>
    ///     Determines how the buttons relate to one another visually.
    /// </summary>
    [Parameter]
    public NTButtonGroupDisplayType DisplayType { get; set; } = NTButtonGroupDisplayType.Connected;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create("nt-button-group")
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("nt-button-group--connected", DisplayType == NTButtonGroupDisplayType.Connected)
        .AddClass("nt-button-group--disconnected", DisplayType == NTButtonGroupDisplayType.Disconnected)
        .AddClass("nt-button-group-size-xs", ButtonSize is Size.Smallest or Size.XS)
        .AddClass("nt-button-group-size-small", ButtonSize == Size.Small)
        .AddClass("nt-button-group-size-medium", ButtonSize == Size.Medium)
        .AddClass("nt-button-group-size-large", ButtonSize == Size.Large)
        .AddClass("nt-button-group-size-xl", ButtonSize is Size.Largest or Size.XL)
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    ///     Indicates whether ripples should render on each button.
    /// </summary>
    [Parameter]
    public bool EnableRipple { get; set; } = true;

    /// <summary>
    ///     The child content that defines the group items.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private readonly List<NTButtonGroupItem<TObjectType>> _items = [];

    /// <summary>
    ///     Invoked whenever selection toggles and passes the impacted item.
    /// </summary>
    [Parameter]
    public EventCallback<NTButtonGroupItem<TObjectType>> OnSelectionChanged { get; set; }

    /// <summary>
    ///     The on-tint color applied when a button is not selected.
    /// </summary>
    [Parameter]
    public TnTColor? OnTintColor { get; set; }

    /// <summary>
    ///     Gets or sets the background color that is applied when the user interface element is selected.
    /// </summary>
    [Parameter]
    public TnTColor SelectedBackgroundColor { get; set; } = TnTColor.Primary;

    /// <summary>
    ///     The key that represents the currently selected button.
    /// </summary>
    [Parameter]
    public TObjectType? SelectedKey { get; set; }

    /// <summary>
    ///     Invoked when the selected key changes.
    /// </summary>
    [Parameter]
    public EventCallback<TObjectType?> SelectedKeyChanged { get; set; }

    /// <summary>
    ///     The on-tint color used when a button is selected.
    /// </summary>
    [Parameter]
    public TnTColor? SelectedOnTintColor { get; set; } = TnTColor.OnPrimary;

    /// <summary>
    ///     The optional text color used when a button is selected.
    /// </summary>
    [Parameter]
    public TnTColor SelectedTextColor { get; set; } = TnTColor.OnPrimary;

    /// <summary>
    ///     The tint color applied to the buttons when they are selected.
    /// </summary>
    [Parameter]
    public TnTColor? SelectedTintColor { get; set; } = TnTColor.SurfaceTint;

    /// <summary>
    ///     The shape applied to all buttons.
    /// </summary>
    [Parameter]
    public ButtonShape Shape { get; set; } = ButtonShape.Round;

    /// <summary>
    ///     Stops bubbling on every button click inside the group.
    /// </summary>
    [Parameter]
    public bool StopPropagation { get; set; } = true;

    /// <summary>
    ///     The text color applied to the buttons when they are not selected.
    /// </summary>
    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurfaceVariant;

    /// <summary>
    ///     The tint color applied to the buttons when they are not selected.
    /// </summary>
    [Parameter]
    public TnTColor? TintColor { get; set; } = TnTColor.SurfaceTint;

    /// <summary>
    ///     Tracks a registered item so the group can render it.
    /// </summary>
    internal void RegisterItem(NTButtonGroupItem<TObjectType> item) {
        if (item is not null && !_items.Contains(item)) {
            _items.Add(item);
            EnsureDefaultSelectionIsSet();
            StateHasChanged();
        }
    }

    /// <summary>
    ///     Unregisters an item when it is removed from the group.
    /// </summary>
    internal void UnregisterItem(NTButtonGroupItem<TObjectType> item) {
        if (item is not null && _items.Remove(item)) {

            if (SelectedKey is not null && EqualityComparer<TObjectType>.Default.Equals(SelectedKey, item.Key)) {
                SelectedKey = default;
            }
            StateHasChanged();
        }
    }

    /// <summary>
    ///     Ensures a default item is selected when no explicit selection exists.
    /// </summary>
    private void EnsureDefaultSelectionIsSet() {
        if (SelectedKey is not null) {
            return;
        }

        var defaultItem = _items.FirstOrDefault(item => item.IsDefaultSelected);
        if (defaultItem is not null) {
            SelectedKey = defaultItem.Key;
        }
    }

    private TnTColor GetItemBackgroundColor(NTButtonGroupItem<TObjectType> item) => IsSelectedItem(item) ? SelectedBackgroundColor : BackgroundColor;

    private TnTColor? GetItemOnTintColor(NTButtonGroupItem<TObjectType> item) => IsSelectedItem(item) ? SelectedOnTintColor ?? OnTintColor : OnTintColor;

    private ButtonShape GetItemShape(NTButtonGroupItem<TObjectType> item) => DisplayType == NTButtonGroupDisplayType.Disconnected ? IsSelectedItem(item) ? ButtonShape.Square : ButtonShape.Round : ButtonShape.Round;

    private TnTColor GetItemTextColor(NTButtonGroupItem<TObjectType> item) => IsSelectedItem(item) ? SelectedTextColor : TextColor;

    private TnTColor? GetItemTintColor(NTButtonGroupItem<TObjectType> item) => IsSelectedItem(item) ? SelectedTintColor ?? TintColor : TintColor;

    private async Task HandleItemClickAsync(NTButtonGroupItem<TObjectType> item) {
        if (Disabled || item.Disabled || IsSelectedItem(item)) {
            return;
        }

        await UpdateSelectionAsync(item.Key, item);
    }

    private bool IsButtonDisabled(NTButtonGroupItem<TObjectType> item) => Disabled || item.Disabled;

    private bool IsSelectedItem(NTButtonGroupItem<TObjectType> item) => item.Key is not null && SelectedKey?.Equals(item.Key) == true;

    private async Task UpdateSelectionAsync(TObjectType nextKey, NTButtonGroupItem<TObjectType>? item = null) {
        if ((nextKey is null && SelectedKey is null) || nextKey?.Equals(SelectedKey) == true) {
            return;
        }

        SelectedKey = nextKey;

        await SelectedKeyChanged.InvokeAsync(nextKey);

        if (item is not null) {
            await OnSelectionChanged.InvokeAsync(item);
        }

        await InvokeAsync(StateHasChanged);
    }
}

/// <summary>
///     Controls how the buttons in an <see cref="NTButtonGroup{TObjectType}" /> relate to each other.
/// </summary>
public enum NTButtonGroupDisplayType {

    /// <summary>
    ///     Buttons appear visually connected, sharing borders to look like a single control.
    /// </summary>
    Connected,

    /// <summary>
    ///     Buttons are rendered with spacing and visual separation.
    /// </summary>
    Disconnected
}