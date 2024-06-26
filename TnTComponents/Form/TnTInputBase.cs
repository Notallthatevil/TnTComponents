﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TnTComponents.Core;
using TnTComponents.Form;

namespace TnTComponents;

public abstract partial class TnTInputBase<TInputType> : InputBase<TInputType>, IFormField {

    [Parameter]
    public FormAppearance Appearance { get; set; }

    [Parameter]
    public TnTColor? BackgroundColor { get; set; } = TnTColor.SurfaceContainer;

    [Parameter]
    public EventCallback<TInputType?> BindAfter { get; set; }

    [Parameter]
    public bool BindOnInput { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public bool DisableValidationMessage { get; set; } = false;

    public ElementReference Element { get; protected set; }

    [Parameter]
    public TnTIcon? EndIcon { get; set; }

    public virtual string FormCssClass => CssClassBuilder.Create()
        .AddClass(CssClass)
        .AddClass("tnt-input")
        .AddOutlined((ParentFormAppearance ?? Appearance) == FormAppearance.Outlined)
        .AddFilled((ParentFormAppearance ?? Appearance) == FormAppearance.Filled)
        .AddBackgroundColor((ParentFormAppearance ?? Appearance) == FormAppearance.Filled ? BackgroundColor : null)
        .AddForegroundColor(TextColor)
        .AddClass("tnt-input-placeholder", !string.IsNullOrWhiteSpace(Placeholder))
        .AddBorderRadius((ParentFormAppearance ?? Appearance) == FormAppearance.Filled ? new TnTBorderRadius() { StartStart = 1, StartEnd = 1 } : new TnTBorderRadius(1))
        .Build();

    public virtual string? FormCssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [CascadingParameter]
    public TnTLabel? Label { get; set; }

    [Parameter]
    public EventCallback<TInputType?> OnChanged { get; set; }

    [CascadingParameter]
    public TnTForm? ParentForm { get; set; }

    [CascadingParameter(Name = nameof(ParentFormAppearance))]
    public FormAppearance? ParentFormAppearance { get; set; }

    [CascadingParameter(Name = nameof(ParentFormDisabled))]
    public bool? ParentFormDisabled { get; set; }

    [CascadingParameter(Name = nameof(ParentFormReadOnly))]
    public bool? ParentFormReadOnly { get; set; }

    [Parameter]
    public string? Placeholder { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public TnTIcon? StartIcon { get; set; }

    [Parameter]
    public TnTColor? TextColor { get; set; } = TnTColor.OnSurfaceVariant;

    public abstract InputType Type { get; }

    public ValueTask SetFocusAsync() {
        return Element.FocusAsync();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "span");
        builder.AddAttribute(10, "class", FormCssClass);
        builder.AddAttribute(11, "style", FormCssStyle);
        {
            {
                if (StartIcon is not null) {
                    StartIcon.AdditionalClass = "tnt-start";
                    builder.AddContent(20, StartIcon.Render());
                }
            }
            {
                if (Type == InputType.TextArea) {
                    builder.OpenElement(100, "textarea");
                }
                else {
                    builder.OpenElement(100, "input");
                    builder.AddAttribute(110, "type", Type.ToInputTypeString());
                }
                builder.AddMultipleAttributes(120, AdditionalAttributes);
                builder.AddAttribute(121, "name", NameAttributeValue);

                if (CurrentValue is bool) {
                    builder.AddAttribute(125, "value", bool.TrueString);
                    builder.AddAttribute(126, "checked", BindConverter.FormatValue(CurrentValue));
                }
                else {
                    builder.AddAttribute(125, "value", CurrentValueAsString);
                }
                builder.AddAttribute(140, "style", FormCssStyle);
                builder.AddAttribute(150, "readonly", ParentFormReadOnly.HasValue ? ReadOnly || ParentFormReadOnly.Value : ReadOnly);
                builder.AddAttribute(160, "placeholder", Placeholder);
                builder.AddAttribute(170, "disabled", ParentFormDisabled.HasValue ? Disabled || ParentFormDisabled.Value : Disabled);
                builder.AddAttribute(171, "required", IsRequired());
                builder.AddAttribute(172, "minlength", GetMinLength());
                builder.AddAttribute(173, "maxlength", GetMaxLength());
                builder.AddAttribute(174, "min", GetMinValue());
                builder.AddAttribute(175, "max", GetMaxValue());
                if (BindOnInput) {
                    builder.AddAttribute(180, "oninput", EventCallback.Factory.CreateBinder(this, value => { CurrentValue = value; BindAfter.InvokeAsync(CurrentValue); }, CurrentValue));
                }
                else {
                    builder.AddAttribute(180, "onchange", EventCallback.Factory.CreateBinder(this, value => { CurrentValue = value; BindAfter.InvokeAsync(CurrentValue); }, CurrentValue));
                }
                if (CurrentValue is bool) {
                    builder.SetUpdatesAttributeName("checked");
                }
                else {
                    builder.SetUpdatesAttributeName("value");
                }

                if (EditContext is not null) {
                    builder.AddAttribute(190, "onblur", EventCallback.Factory.Create<FocusEventArgs>(this, args => {
                        EditContext.NotifyFieldChanged(FieldIdentifier);
                    }));
                }

                builder.AddElementReferenceCapture(200, e => Element = e);
                builder.CloseElement();

                if (EditContext is not null && !DisableValidationMessage && ValueExpression is not null) {
                    builder.OpenComponent<ValidationMessage<TInputType>>(210);
                    builder.AddComponentParameter(220, nameof(ValidationMessage<TInputType>.For), ValueExpression);
                    builder.AddAttribute(230, "class", "tnt-components tnt-validation-message tnt-body-small");
                    builder.CloseComponent();
                }
            }
            {
                if (EndIcon is not null) {
                    EndIcon.AdditionalClass = "tnt-end";
                    builder.AddContent(240, EndIcon.Render());
                }
            }
        }

        builder.CloseElement();
    }

    protected bool IsRequired() {
        return AdditionalAttributes?.TryGetValue("required", out var _) == true || GetCustomAttributeIfExists<RequiredAttribute>() is not null;
    }

    protected override void OnInitialized() {
        base.OnInitialized();

        Label?.SetChildField(this);

        if (string.IsNullOrWhiteSpace(Placeholder)) {
            Placeholder = " ";
        }
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