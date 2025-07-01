using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Runtime.InteropServices;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

/// <summary>
///     Represents a child item within a TnTAccordion component.
/// </summary>
public partial class TnTAccordionChild {

    /// <summary>
    ///     The content to be rendered inside the accordion child.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <summary>
    ///     The body color of the content.
    /// </summary>
    [Parameter]
    public TnTColor? ContentBodyColor { get; set; }

    /// <summary>
    ///     The text color of the content.
    /// </summary>
    [Parameter]
    public TnTColor? ContentTextColor { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool Disabled { get; set; }

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-accordion-child")
        .AddBackgroundColor(ContentBodyColor ?? _parent.ContentBodyColor)
        .AddForegroundColor(ContentTextColor ?? _parent.ContentTextColor)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public string? ElementName { get; set; }

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public bool EnableRipple { get; set; } = true;

    /// <summary>
    ///     The body color of the header.
    /// </summary>
    [Parameter]
    public TnTColor? HeaderBodyColor { get; set; }

    /// <summary>
    ///     The text color of the header.
    /// </summary>
    [Parameter]
    public TnTColor? HeaderTextColor { get; set; }

    /// <summary>
    ///     The tint color of the header.
    /// </summary>
    [Parameter]
    public TnTColor? HeaderTintColor { get; set; }

    /// <summary>
    ///     The label of the accordion child.
    /// </summary>
    [Parameter, EditorRequired]
    public string Label { get; set; } = default!;

    /// <summary>
    ///     Callback invoked after the accordion has closed.
    /// </summary>
    [Parameter]
    public EventCallback OnCloseCallback { get; set; }

    /// <summary>
    ///     Callback invoked after the accordion has opened.
    /// </summary>
    [Parameter]
    public EventCallback OnOpenCallback { get; set; }

    /// <inheritdoc />
    [Parameter]
    public TnTColor? OnTintColor { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the accordion child is open by default.
    /// </summary>
    [Parameter]
    public bool OpenByDefault { get; set; }

    /// <summary>
    /// Specifies the sorting order of the accordion child. Lower numbers are rendered first.
    /// </summary>
    [Parameter]
    public int? Order { get; set; }

    /// <summary>
    ///     When set, removes the child content from the DOM when the accordion is closed. When opened, the content is added back to the DOM. Only applies in interactive mode.
    /// </summary>
    [Parameter]
    public bool RemoveContentOnClose { get; set; }

    /// <inheritdoc />
    [Parameter]
    public TnTColor? TintColor { get; set; }

    internal int _elementId = int.MinValue;

    internal bool _open;

    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    [CascadingParameter]
    private TnTAccordion _parent { get; set; } = default!;

    /// <summary>
    ///     Closes the accordion child asynchronously.
    /// </summary>
    public async Task CloseAsync() => await _jsRuntime.InvokeVoidAsync("TnTComponents.addHidden", Element);

    /// <inheritdoc />
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
            builder.AddAttribute(90, "element-key", _elementId);
            builder.SetKey(this);

            {
                builder.OpenElement(90, "h3");
                builder.AddAttribute(95, "onclick", "TnTComponents.toggleAccordionHeader(event)");
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

                if (EnableRipple) {
                    builder.OpenComponent<TnTRippleEffect>(125);
                    builder.CloseComponent();
                }

                {
                    builder.OpenComponent<MaterialIcon>(130);
                    builder.AddComponentParameter(144, nameof(MaterialIcon.Icon), MaterialIcon.ArrowDropDown.Icon);
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

#if NET9_0_OR_GREATER
                if(!RendererInfo.IsInteractive || !RemoveContentOnClose || (RendererInfo.IsInteractive && _open)) {
                    builder.AddContent(170, ChildContent);
                }
#else
                builder.AddContent(170, ChildContent);
#endif

                builder.CloseElement();
            }

            builder.CloseElement();
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized() {
        base.OnInitialized();
        _parent.RegisterChild(this);
        _open = _parent.AllowOpenByDefault && ((OpenByDefault && _parent.LimitToOneExpanded && !_parent.FoundExpanded) || (OpenByDefault && !_parent.LimitToOneExpanded));
    }
}