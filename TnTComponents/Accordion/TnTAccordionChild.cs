using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Runtime.InteropServices;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

/// <summary>
///     Represents a child item within a TnTAccordion component.
/// </summary>
public class TnTAccordionChild : TnTComponentBase, ITnTInteractable, IDisposable {

    /// <summary>
    ///     Gets or sets the content to be rendered inside the accordion child.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <summary>
    ///     Gets or sets the body color of the content.
    /// </summary>
    [Parameter]
    public TnTColor? ContentBodyColor { get; set; }

    /// <summary>
    ///     Gets or sets the text color of the content.
    /// </summary>
    [Parameter]
    public TnTColor? ContentTextColor { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-accordion-child")
        .AddBackgroundColor(ContentBodyColor ?? _parent.ContentBodyColor)
        .AddForegroundColor(ContentTextColor ?? _parent.ContentTextColor)
        .AddFilled()
        .Build();

    [Parameter]
    public string? ElementName { get; set; }

    public override string? ElementStyle => CssStyleBuilder.Create()
            .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public bool EnableRipple { get; set; }

    /// <summary>
    ///     Gets or sets the body color of the header.
    /// </summary>
    [Parameter]
    public TnTColor? HeaderBodyColor { get; set; }

    /// <summary>
    ///     Gets or sets the text color of the header.
    /// </summary>
    [Parameter]
    public TnTColor? HeaderTextColor { get; set; }

    /// <summary>
    ///     Gets or sets the tint color of the header.
    /// </summary>
    [Parameter]
    public TnTColor? HeaderTintColor { get; set; }

    /// <summary>
    ///     Gets or sets the label of the accordion child.
    /// </summary>
    [Parameter, EditorRequired]
    public string Label { get; set; } = default!;

    [Parameter]
    public TnTColor? OnTintColor { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the accordion child is open by default.
    /// </summary>
    [Parameter]
    public bool OpenByDefault { get; set; }

    [Parameter]
    public TnTColor? TintColor { get; set; }

    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    [CascadingParameter]
    private TnTAccordion _parent { get; set; } = default!;

    /// <summary>
    ///     Closes the accordion child asynchronously.
    /// </summary>
    public async Task CloseAsync() => await _jsRuntime.InvokeVoidAsync("TnTComponents.addHidden", Element);

    /// <summary>
    ///     Disposes the accordion child.
    /// </summary>
    public void Dispose() {
        GC.SuppressFinalize(this);
        _parent.RemoveChild(this);
    }

    /// <summary>
    ///     Renders the child content.
    /// </summary>
    /// <returns>A <see cref="RenderFragment" /> representing the child content.</returns>
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
                    .AddClass("tnt-expanded", _parent.AllowOpenByDefault && ((OpenByDefault && _parent.LimitToOneExpanded && !_parent.FoundExpanded) || (OpenByDefault && !_parent.LimitToOneExpanded)))
                    .Build());

                if (OpenByDefault) {
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