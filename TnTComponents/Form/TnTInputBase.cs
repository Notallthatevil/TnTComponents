using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TnTComponents.Core;
using TnTComponents.Form;
using TnTComponents.Interfaces;

namespace TnTComponents;

public abstract partial class TnTInputBase<TInputType> : InputBase<TInputType>, ITnTComponentBase, ITnTInteractable {

    [Parameter]
    public FormAppearance Appearance { get; set; }

    [Parameter]
    public bool? AutoFocus { get; set; }

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainerHighest;

    [Parameter]
    public EventCallback<TInputType?> BindAfter { get; set; }

    [Parameter]
    public bool BindOnInput { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public bool DisableValidationMessage { get; set; } = false;

    public ElementReference Element { get; private set; }

    public virtual string? ElementClass => CssClassBuilder.Create()
        .AddClass(CssClass)
        .AddClass("tnt-input")
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddFilled(_tntForm?.Appearance is not null ? _tntForm.Appearance == FormAppearance.Filled : Appearance == FormAppearance.Filled)
        .AddOutlined(_tntForm?.Appearance is not null ? _tntForm.Appearance == FormAppearance.Outlined : Appearance == FormAppearance.Outlined)
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .AddTintColor(TintColor)
        .AddDisabled(_disabled)
        .AddClass("tnt-placeholder", !string.IsNullOrEmpty(Placeholder))
        .Build();

    [Parameter]
    public string? ElementId { get; set; }

    [Parameter]
    public string? ElementLang { get; set; }

    public string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public string? ElementTitle { get; set; }

    public bool EnableRipple => false;

    [Parameter]
    public TnTIcon? EndIcon { get; set; }

    [Parameter]
    public string? Label { get; set; }

    public string? Name => NameAttributeValue;

    [Parameter]
    public string? Placeholder { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public TnTIcon? StartIcon { get; set; }

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    [Parameter]
    public TnTColor? TintColor { get; set; } = TnTColor.Primary;

    public abstract InputType Type { get; }
    private bool _disabled => _tntForm?.Disabled is not null ? _tntForm.Disabled : Disabled;

    private bool _readOnly => _tntForm?.ReadOnly is not null ? _tntForm.ReadOnly : ReadOnly;

    [CascadingParameter]
    private ITnTForm? _tntForm { get; set; }

    public ValueTask SetFocusAsync() {
        return Element.FocusAsync();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "label");
        builder.AddAttribute(10, "lang", ElementLang);
        builder.AddAttribute(20, "title", ElementTitle);
        builder.AddAttribute(30, "class", ElementClass);
        builder.AddAttribute(40, "id", ElementId);

        {
            {
                if (StartIcon is not null) {
                    StartIcon.AdditionalClass = "tnt-start-icon";
                    builder.AddContent(50, StartIcon.Render());
                }
            }
            {
                if (Type == InputType.TextArea) {
                    builder.OpenElement(60, "textarea");
                }
                else {
                    builder.OpenElement(60, "input");
                    builder.AddAttribute(70, "type", Type.ToInputTypeString());
                }
                builder.AddMultipleAttributes(80, AdditionalAttributes);
                builder.AddAttribute(90, "name", Name);

                if(Type == InputType.Tel) {
                    builder.AddAttribute(91, "onkeydown", "TnTComponents.enforcePhoneFormat(event)");
                    builder.AddAttribute(92, "onkeyup", "TnTComponents.formatToPhone(event)");
                }

                if (typeof(TInputType) == typeof(bool)) {
                    builder.AddAttribute(100, "value", bool.TrueString);
                    builder.AddAttribute(110, "checked", BindConverter.FormatValue(CurrentValue));
                }
                else {
                    builder.AddAttribute(100, "value", CurrentValueAsString);
                }
                builder.AddAttribute(120, "style", ElementStyle);
                builder.AddAttribute(130, "readonly", _readOnly);
                builder.AddAttribute(140, "placeholder", string.IsNullOrEmpty(Placeholder) ? " " : Placeholder);
                builder.AddAttribute(150, "disabled", _disabled);
                builder.AddAttribute(160, "required", IsRequired());
                builder.AddAttribute(170, "minlength", GetMinLength());
                builder.AddAttribute(180, "maxlength", GetMaxLength());
                builder.AddAttribute(190, "min", GetMinValue());
                builder.AddAttribute(200, "max", GetMaxValue());
                if (BindOnInput) {
                    builder.AddAttribute(210, "oninput", EventCallback.Factory.CreateBinder(this, value => { CurrentValue = value; BindAfter.InvokeAsync(CurrentValue); }, CurrentValue));
                }
                else {
                    builder.AddAttribute(210, "onchange", EventCallback.Factory.CreateBinder(this, value => { CurrentValue = value; BindAfter.InvokeAsync(CurrentValue); }, CurrentValue));
                }
                if (typeof(TInputType) == typeof(bool)) {
                    builder.SetUpdatesAttributeName("checked");
                }
                else {
                    builder.SetUpdatesAttributeName("value");
                }

                if (EditContext is not null) {
                    builder.AddAttribute(220, "onblur", EventCallback.Factory.Create<FocusEventArgs>(this, args => {
                        EditContext.NotifyFieldChanged(FieldIdentifier);
                    }));
                }

                builder.AddElementReferenceCapture(230, e => Element = e);
                builder.CloseElement();

                if (EditContext is not null && !DisableValidationMessage && ValueExpression is not null) {
                    builder.OpenComponent<ValidationMessage<TInputType>>(240);
                    builder.AddComponentParameter(250, nameof(ValidationMessage<TInputType>.For), ValueExpression);
                    builder.AddAttribute(260, "class", "tnt-components tnt-validation-message tnt-body-small");
                    builder.CloseComponent();
                }
            }
            {
                if (EndIcon is not null) {
                    EndIcon.AdditionalClass = "tnt-end-icon";
                    builder.AddContent(270, EndIcon.Render());
                }
            }
            {
                if (!string.IsNullOrWhiteSpace(Label)) {
                    builder.OpenElement(280, "span");
                    builder.AddAttribute(290, "class", CssClassBuilder.Create().AddClass("tnt-label").Build());
                    builder.AddContent(300, Label);
                    builder.CloseElement();
                }
            }
        }

        builder.CloseElement();
    }

    protected bool IsRequired() {
        return AdditionalAttributes?.TryGetValue("required", out var _) == true || GetCustomAttributeIfExists<RequiredAttribute>() is not null;
    }

    private TCustomAttr? GetCustomAttributeIfExists<TCustomAttr>() where TCustomAttr : Attribute {
        if (ValueExpression is not null) {
            var property = FieldIdentifier.Model.GetType().GetProperty(FieldIdentifier.FieldName);
            if (property is not null) {
                return property.GetCustomAttribute<TCustomAttr>();
            }
        }
        return null;
    }

    private int? GetMaxLength() {
        if (AdditionalAttributes?.TryGetValue("maxlength", out var maxLength) == true && int.TryParse(maxLength?.ToString(), out var result)) {
            return result;
        }
        var maxLengthAttr = GetCustomAttributeIfExists<MaxLengthAttribute>();
        if (maxLengthAttr is not null) {
            return maxLengthAttr.Length;
        }

        if (typeof(TInputType) == typeof(string)) {
            var strLengthAttr = GetCustomAttributeIfExists<StringLengthAttribute>();
            if (strLengthAttr is not null) {
                return strLengthAttr.MaximumLength;
            }
        }

        return null;
    }

    private string? GetMaxValue() {
        if (AdditionalAttributes?.TryGetValue("max", out var max) == true) {
            return max?.ToString();
        }
        var rangeAttr = GetCustomAttributeIfExists<RangeAttribute>();
        if (rangeAttr is not null) {
            return rangeAttr.Maximum.ToString();
        }

        return null;
    }

    private int? GetMinLength() {
        if (AdditionalAttributes?.TryGetValue("minlength", out var minLength) == true && int.TryParse(minLength?.ToString(), out var result)) {
            return result;
        }
        var minLengthAttr = GetCustomAttributeIfExists<MinLengthAttribute>();
        if (minLengthAttr is not null) {
            return minLengthAttr.Length;
        }

        if (typeof(TInputType) == typeof(string)) {
            var strLengthAttr = GetCustomAttributeIfExists<StringLengthAttribute>();
            if (strLengthAttr is not null) {
                return strLengthAttr.MinimumLength;
            }
        }

        return null;
    }

    private string? GetMinValue() {
        if (AdditionalAttributes?.TryGetValue("min", out var min) == true) {
            return min?.ToString();
        }
        var rangeAttr = GetCustomAttributeIfExists<RangeAttribute>();
        if (rangeAttr is not null) {
            return rangeAttr.Minimum.ToString();
        }

        return null;
    }
}