using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents;
public partial class TnTCarouselItem {
    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; }

    internal int? InternalId { get; set; }

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-carousel-item")
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddStyle("background-image", $"url('{BackgroundImageSrc}')", !string.IsNullOrWhiteSpace(BackgroundImageSrc))
        .Build();

    [Parameter]
    public int Order { get; set; }

    [CascadingParameter]
    private TnTCarousel _carousel { get; set; }

    [Parameter]
    public string? BackgroundImageSrc { get; set; }

    public override string? JsModulePath => "./_content/TnTComponents/Carousel/TnTCarouselItem.razor.js";

    protected override void OnInitialized() {
        base.OnInitialized();
        if (_carousel is null) {
            throw new InvalidOperationException($"{nameof(TnTCarouselItem)} must be a decendant of {nameof(TnTCarousel)}");
        }

        _carousel.AddChild(this);
    }

    protected override void Dispose(bool disposing) {
        base.Dispose(disposing);
        if (disposing) {
            _carousel?.RemoveChild(this);
        }
    }
}
