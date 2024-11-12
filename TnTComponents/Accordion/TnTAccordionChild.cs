using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Runtime.InteropServices;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

public class TnTAccordionChild : ComponentBase, ITnTComponentBase, ITnTInteractable, IDisposable {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public bool? AutoFocus { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public TnTColor? ContentBodyColor { get; set; }

    [Parameter]
    public TnTColor? ContentTextColor { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    public ElementReference Element { get; internal set; }

    public string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-accordion-child")
        .AddBackgroundColor(ContentBodyColor ?? _parent.ContentBodyColor)
        .AddForegroundColor(ContentTextColor ?? _parent.ContentTextColor)
        .AddFilled()
        .Build();

    [Parameter]
    public string? ElementId { get; set; }

    [Parameter]
    public string? ElementLang { get; set; }

    [Parameter]
    public string? ElementName { get; set; }

    public string? ElementStyle => CssStyleBuilder.Create().Build();

    [Parameter]
    public string? ElementTitle { get; set; }

    [Parameter]
    public bool EnableRipple { get; set; }

    [Parameter]
    public TnTColor? HeaderBodyColor { get; set; }

    [Parameter]
    public TnTColor? HeaderTextColor { get; set; }

    [Parameter]
    public TnTColor? HeaderTintColor { get; set; }

    [Parameter, EditorRequired]
    public string Label { get; set; } = default!;

    [Parameter]
    public TnTColor? OnTintColor { get; set; }

    [Parameter]
    public bool OpenByDefault { get; set; }

    [Parameter]
    public TnTColor? TintColor { get; set; }

    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    [CascadingParameter]
    private TnTAccordion _parent { get; set; } = default!;

    public async Task CloseAsync() {
        await _jsRuntime.InvokeVoidAsync("TnTComponents.addHidden", Element);
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
        _parent.RemoveChild(this);
    }

    public RenderFragment RenderChild() {
        return new RenderFragment(builder => {
            builder.OpenElement(0, "div");
            builder.AddMultipleAttributes(10, AdditionalAttributes);
            builder.AddAttribute(20, "class", ElementClass);
            builder.AddAttribute(30, "style", ElementStyle);
            builder.AddAttribute(40, "id", ElementId);
            builder.AddAttribute(50, "lang", ElementLang);
            builder.AddAttribute(60, "title", ElementTitle);
            builder.AddAttribute(70, "name", ElementName);
            builder.AddAttribute(80, "disabled", Disabled);

            {
                builder.OpenElement(90, "h3");
                builder.AddAttribute(100, "class", CssClassBuilder.Create()
                        .AddRipple()
                        .AddBackgroundColor(HeaderBodyColor ?? _parent.HeaderBodyColor)
                        .AddForegroundColor(HeaderTextColor ?? _parent.HeaderTextColor)
                        .AddTintColor(HeaderTintColor ?? _parent.HeaderTintColor)
                        .AddFilled()
                        .AddTnTInteractable(this)
                        .AddDisabled(Disabled)
                        .Build());
                builder.AddAttribute(110, "data-permanent", true);
                builder.AddContent(120, Label);

                {
                    builder.OpenComponent<MaterialIcon>(130);
                    builder.AddComponentParameter(144, nameof(MaterialIcon.Icon), MaterialIcon.ArrowDropDown);
                    builder.CloseComponent();
                }
                builder.CloseElement();
            }

            {
                builder.OpenElement(150, "div");
                builder.AddAttribute(160, "class", CssClassBuilder.Create()
                    .AddClass("tnt-expanded", (OpenByDefault && _parent.LimitToOneExpanded && !_parent.FoundExpanded) || (OpenByDefault && !_parent.LimitToOneExpanded))
                    .Build());

                if(OpenByDefault) {
                    _parent.FoundExpanded = true;
                }

                builder.AddContent(170, ChildContent);

                builder.CloseElement();
            }

            builder.CloseElement();
        });
    }

    protected override void OnInitialized() {
        base.OnInitialized();
        _parent.RegisterChild(this);
    }
}