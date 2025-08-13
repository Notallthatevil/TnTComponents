using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
///     A Material 3 compliant carousel component for displaying a sequence of items with navigation and indicators.
/// </summary>
public partial class TnTCarousel {
    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public EventCallback<int> OnIndexChanged { get; set; }

    [Parameter]
    public CarouselAppearance Appearance { get; set; } = CarouselAppearance.Default;

    [Parameter]
    public TnTColor? BackgroundColor { get; set; }

    [Parameter]
    public bool EnableSnapping { get; set; } = true;

    [Parameter]
    public int? AutoPlayInterval { get; set; }

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-carousel")
        .AddClass("tnt-carousel-hero", Appearance is CarouselAppearance.Hero)
        .AddClass("tnt-carousel-hero-centered", Appearance is CarouselAppearance.CenteredHero)
        .AddClass("tnt-carousel-snapping", EnableSnapping)
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("tnt-carousel-bg-color", BackgroundColor.GetValueOrDefault(), BackgroundColor.HasValue)
        .Build();

    public override string? JsModulePath => "./_content/TnTComponents/Carousel/TnTCarousel.razor.js";
    private IEnumerable<TnTCarouselItem> _carouselItems => _items.Values.OrderBy(item => item.Order).ThenBy(item => item.InternalId);
    private Dictionary<int, TnTCarouselItem> _items = [];
    private int _nextId;

    internal void AddChild(TnTCarouselItem item) {
        item.InternalId ??= Interlocked.Increment(ref _nextId);
        if (_items.TryAdd(item.InternalId.Value, item)) {
            StateHasChanged();
        }
    }

    internal void RemoveChild(TnTCarouselItem item) {
        if (item.InternalId is not null && _items.Remove(item.InternalId.Value)) {
            StateHasChanged();
        }
    }
}

public enum CarouselAppearance {
    Default,
    Hero,
    CenteredHero
}