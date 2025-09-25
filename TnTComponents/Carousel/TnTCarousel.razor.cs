using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
///     A Material 3 compliant carousel component for displaying a sequence of items with navigation and indicators.
/// </summary>
/// <remarks>
///     The component expects child content consisting of <see cref="TnTCarouselItem" /> elements. It exposes appearance and behavior options such as snapping, autoplay, background color, and
///     dragging. JavaScript interop is used for lifecycle hooks via the module path returned by <see cref="JsModulePath" />.
/// </remarks>
public partial class TnTCarousel {

    /// <summary>
    ///     Controls whether pointer dragging is allowed to navigate the carousel.
    /// </summary>
    [Parameter]
    public bool AllowDragging { get; set; } = true;

    /// <summary>
    ///     Visual appearance options for the carousel. Flags can be combined.
    /// </summary>
    [Parameter]
    public CarouselAppearance Appearance { get; set; } = CarouselAppearance.Default;

    /// <summary>
    ///     Optional interval in milliseconds for autoplay. When null, autoplay is disabled.
    /// </summary>
    [Parameter]
    public int? AutoPlayInterval { get; set; }

    /// <summary>
    ///     Optional background color for the carousel expressed as a <see cref="TnTColor" />. When not specified, no inline background variable will be emitted.
    /// </summary>
    [Parameter]
    public TnTColor? BackgroundColor { get; set; }

    /// <summary>
    ///     The content (child items) to render inside the carousel.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; }

    /// <summary>
    ///     Computed CSS class string for the carousel element. Includes classes controlled by parameters and merges additional attributes that provide classes.
    /// </summary>
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-carousel")
        .AddClass("tnt-carousel-hero", Appearance.HasFlag(CarouselAppearance.Hero))
        .AddClass("tnt-carousel-centered", Appearance.HasFlag(CarouselAppearance.Centered) && EnableSnapping)
        .AddClass("tnt-carousel-snapping", EnableSnapping)
        .Build();

    /// <summary>
    ///     Computed inline style string for the carousel element. Exposes CSS variables (for example background color) when appropriate parameters are provided.
    /// </summary>
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("tnt-carousel-bg-color", BackgroundColor.GetValueOrDefault(), BackgroundColor.HasValue)
        .Build();

    /// <summary>
    ///     Enables CSS snapping behavior for carousel items when true. When enabled, additional classes are applied to support centered and snapping layouts.
    /// </summary>
    [Parameter]
    public bool EnableSnapping { get; set; } = true;

    /// <summary>
    ///     Path to the JavaScript module that implements lifecycle interop for the carousel. The module is expected to export <c>onLoad</c>, <c>onUpdate</c>, and <c>onDispose</c>.
    /// </summary>
    public override string? JsModulePath => "./_content/TnTComponents/Carousel/TnTCarousel.razor.js";

    /// <summary>
    ///     Invoked when the currently displayed index changes. The callback receives the new index (zero-based).
    /// </summary>
    [Parameter]
    public EventCallback<int> OnIndexChanged { get; set; }

    /// <summary>
    ///     Enumerates the currently registered carousel items in display order (first by <see cref="TnTCarouselItem.Order" />, then by internal id to provide a stable ordering).
    /// </summary>
    private IEnumerable<TnTCarouselItem> _carouselItems => _items.Values.OrderBy(item => item.Order).ThenBy(item => item.InternalId);

    /// <summary>
    ///     Internal storage of child items keyed by their internal id.
    /// </summary>
    private Dictionary<int, TnTCarouselItem> _items = new();

    /// <summary>
    ///     Monotonic counter used to assign internal ids to child items when they are added.
    /// </summary>
    private int _nextId;

    /// <summary>
    ///     Registers a child <see cref="TnTCarouselItem" /> with this carousel. Assigns an internal id if one isn't present.
    /// </summary>
    /// <param name="item">The carousel item being added.</param>
    internal void AddChild(TnTCarouselItem item) {
        item.InternalId ??= System.Threading.Interlocked.Increment(ref _nextId);
        if (_items.TryAdd(item.InternalId.Value, item)) {
            StateHasChanged();
        }
    }

    /// <summary>
    ///     Removes a previously registered <see cref="TnTCarouselItem" /> from this carousel. If the item was present it will trigger a re-render.
    /// </summary>
    /// <param name="item">The carousel item to remove.</param>
    internal void RemoveChild(TnTCarouselItem item) {
        if (item.InternalId is not null && _items.Remove(item.InternalId.Value)) {
            StateHasChanged();
        }
    }
}

/// <summary>
///     Flags representing the available appearance variants for the <see cref="TnTCarousel" />.
/// </summary>
[Flags]
public enum CarouselAppearance {

    /// <summary>
    ///     Default appearance with standard sizing and behavior.
    /// </summary>
    Default = 0,

    /// <summary>
    ///     Hero appearance: larger, more prominent presentation intended for hero sections.
    /// </summary>
    Hero = 1,

    /// <summary>
    ///     Centered appearance: centers the active item and is typically used together with snapping.
    /// </summary>
    Centered = 2
}