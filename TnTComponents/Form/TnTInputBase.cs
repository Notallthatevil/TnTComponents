using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using TnTComponents.Core;
using TnTComponents.Form;
using TnTComponents.Interfaces;

namespace TnTComponents;

/// <summary>
///     Base class for TnT input components.
/// </summary>
/// <typeparam name="TInputType">The type of the input value.</typeparam>
public abstract partial class TnTInputBase<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TInputType> : InputBase<TInputType>, ITnTComponentBase, ITnTInteractable {

    /// <summary>
    ///     Gets or sets the appearance of the form.
    /// </summary>
    [Parameter]
    public FormAppearance Appearance { get; set; }

    [Parameter]
    public bool? AutoFocus { get; set; }

    /// <summary>
    ///     Gets or sets the background color of the input.
    /// </summary>
    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainerHighest;

    /// <summary>
    ///     Gets or sets the event callback to be invoked after binding.
    /// </summary>
    [Parameter]
    public EventCallback<TInputType?> BindAfter { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether to bind on input.
    /// </summary>
    [Parameter]
    public bool BindOnInput { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether to disable the validation message.
    /// </summary>
    [Parameter]
    public bool DisableValidationMessage { get; set; } = false;

    public ElementReference Element { get; protected set; }

    public virtual string? ElementClass => CssClassBuilder.Create()
        .AddClass(CssClass)
        .AddClass("tnt-input")
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddFilled(_tntForm?.Appearance is not null ? _tntForm.Appearance == FormAppearance.Filled : Appearance == FormAppearance.Filled)
        .AddOutlined(_tntForm?.Appearance is not null ? _tntForm.Appearance == FormAppearance.Outlined : Appearance == FormAppearance.Outlined)
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .AddTintColor(TintColor)
        .AddDisabled(FieldDisabled)
        .AddClass("tnt-placeholder", !string.IsNullOrEmpty(Placeholder))
        .Build();

    [Parameter]
    public string? ElementId { get; set; }

    [Parameter]
    public string? ElementLang { get; set; }

    public string? ElementName => NameAttributeValue;

    public string? ElementStyle => CssStyleBuilder.Create()
            .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public string? ElementTitle { get; set; }

    public bool EnableRipple => false;

    /// <summary>
    ///     Gets or sets the end icon of the input.
    /// </summary>
    [Parameter]
    public TnTIcon? EndIcon { get; set; }

    /// <summary>
    ///     Gets a value indicating whether the input field is disabled.
    /// </summary>
    public bool FieldDisabled => _tntForm?.Disabled is not null ? _tntForm.Disabled : Disabled;

    /// <summary>
    ///     Gets a value indicating whether the input field is read-only.
    /// </summary>
    public bool FieldReadonly => _tntForm?.ReadOnly is not null ? _tntForm.ReadOnly : ReadOnly;

    /// <summary>
    ///     Gets or sets the label of the input.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    ///     Gets or sets the on-tint color of the input.
    /// </summary>
    [Parameter]
    public TnTColor? OnTintColor { get; set; }

    /// <summary>
    ///     Gets or sets the placeholder text of the input.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the input is read-only.
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; }

    /// <summary>
    ///     Gets or sets the start icon of the input.
    /// </summary>
    [Parameter]
    public TnTIcon? StartIcon { get; set; }

    /// <summary>
    ///     Gets or sets the text color of the input.
    /// </summary>
    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    /// <summary>
    ///     Gets or sets the tint color of the input.
    /// </summary>
    [Parameter]
    public TnTColor? TintColor { get; set; } = TnTColor.Primary;

    /// <summary>
    ///     Gets the type of the input.
    /// </summary>
    public abstract InputType Type { get; }

    /// <summary>
    ///     Gets or sets the cascading parameter for the form.
    /// </summary>
    [CascadingParameter]
    private ITnTForm? _tntForm { get; set; }

    /// <summary>
    ///     Sets the focus to the input element.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public ValueTask SetFocusAsync() {
        return Element.FocusAsync();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "label");
        builder.AddAttribute(10, "lang", ElementLang);
        builder.AddAttribute(20, "title", ElementTitle);
        builder.AddAttribute(30, "class", ElementClass);
        builder.AddAttribute(40, "id", ElementId);
        if (AdditionalAttributes?.TryGetValue("style", out var style) == true) {
            builder.AddAttribute(41, "style", style);
        }

        {
            {
                if (StartIcon is not null) {
                    builder.AddContent(50, StartIcon.Render());
                }
            }
            {
                if (Type == InputType.TextArea) {
                    builder.OpenElement(60, "textarea");
                }
                else if (Type == InputType.Select) {
                    builder.OpenElement(60, "select");
                    builder.AddAttribute(70, "multiple", typeof(TInputType).IsArray || Nullable.GetUnderlyingType(typeof(TInputType))?.IsArray == true);
                }
                else {
                    builder.OpenElement(60, "input");
                    builder.AddAttribute(70, "type", Type.ToInputTypeString());
                }
                builder.AddMultipleAttributes(80, AdditionalAttributes);
                builder.AddAttribute(90, "name", ElementName);

                if (Type == InputType.Tel) {
                    builder.AddAttribute(91, "onkeydown", "TnTComponents.enforcePhoneFormat(event)");
                    builder.AddAttribute(92, "onkeyup", "TnTComponents.formatToPhone(event)");
                }
                else if (Type == InputType.Currency) {
                    builder.AddAttribute(91, "onkeydown", "TnTComponents.enforceCurrencyFormat(event)");
                    builder.AddAttribute(92, "onkeyup", "TnTComponents.formatToCurrency(event)");
                }

                if (typeof(TInputType) == typeof(bool)) {
                    builder.AddAttribute(100, "value", bool.TrueString);
                    builder.AddAttribute(110, "checked", BindConverter.FormatValue(CurrentValue));
                }
                else if (Type == InputType.Select && (typeof(TInputType).IsArray || Nullable.GetUnderlyingType(typeof(TInputType))?.IsArray == true)) {
                    builder.AddAttribute(210, "value", BindConverter.FormatValue(CurrentValue)?.ToString());
                }
                else {
                    builder.AddAttribute(100, "value", CurrentValueAsString);
                }
                builder.AddAttribute(120, "style", ElementStyle);
                builder.AddAttribute(130, "readonly", FieldReadonly);
                builder.AddAttribute(140, "placeholder", string.IsNullOrEmpty(Placeholder) ? " " : Placeholder);
                builder.AddAttribute(150, "disabled", FieldDisabled || (Type == InputType.Select && FieldReadonly));
                builder.AddAttribute(160, "required", IsRequired());
                builder.AddAttribute(170, "minlength", GetMinLength());
                builder.AddAttribute(180, "maxlength", GetMaxLength());
                builder.AddAttribute(190, "min", GetMinValue());
                builder.AddAttribute(200, "max", GetMaxValue());

                if (BindOnInput && Type != InputType.Select) {
                    builder.AddAttribute(210, "oninput", EventCallback.Factory.CreateBinder(this, value => { CurrentValue = value; BindAfter.InvokeAsync(CurrentValue); }, CurrentValue));
                }
                else {
                    if (Type == InputType.Select && (typeof(TInputType).IsArray || Nullable.GetUnderlyingType(typeof(TInputType))?.IsArray == true)) {
                        builder.AddAttribute(210, "onchange", EventCallback.Factory.CreateBinder<string?[]?>(this, SetCurrentValueAsStringArray, default));
                    }
                    else if (typeof(TInputType) == typeof(bool)) {
                        builder.AddAttribute(210, "onchange", EventCallback.Factory.CreateBinder(this, __value => { CurrentValue = __value; BindAfter.InvokeAsync(CurrentValue); }, CurrentValue));
                    }
                    else {
                        builder.AddAttribute(210, "onchange", EventCallback.Factory.CreateBinder<string?>(this, OnChangeAsync, CurrentValueAsString));
                    }
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

                builder.OpenRegion(231);
                RenderChildContent(builder);
                builder.CloseRegion();

                builder.CloseElement();

                builder.OpenRegion(235);
                RenderCustomContent(builder);
                builder.CloseRegion();

                if (EditContext is not null && !DisableValidationMessage && ValueExpression is not null) {
                    builder.OpenComponent<ValidationMessage<TInputType>>(240);
                    builder.AddComponentParameter(250, nameof(ValidationMessage<TInputType>.For), ValueExpression);
                    builder.AddAttribute(260, "class", "tnt-components tnt-validation-message tnt-body-small");
                    builder.CloseComponent();
                }
            }
            {
                if (!string.IsNullOrWhiteSpace(Label)) {
                    builder.OpenElement(270, "span");
                    builder.AddAttribute(280, "class", CssClassBuilder.Create().AddClass("tnt-label").Build());
                    builder.AddContent(290, Label);
                    builder.CloseElement();
                }
            }
            {
                if (EndIcon is not null) {
                    builder.AddContent(300, EndIcon.Render());
                }
            }
        }

        builder.CloseElement();
    }

    /// <summary>
    ///     Determines whether the input is required.
    /// </summary>
    /// <returns><c>true</c> if the input is required; otherwise, <c>false</c>.</returns>
    protected bool IsRequired() {
        return AdditionalAttributes?.TryGetValue("required", out var _) == true || GetCustomAttributeIfExists<RequiredAttribute>() is not null;
    }

    /// <summary>
    ///     Renders child content for the input component.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected virtual void RenderChildContent(RenderTreeBuilder builder) { }

    /// <summary>
    ///     Renders custom content for the input component.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected virtual void RenderCustomContent(RenderTreeBuilder builder) { }

    /// <summary>
    ///     Gets the custom attribute if it exists.
    /// </summary>
    /// <typeparam name="TCustomAttr">The type of the custom attribute.</typeparam>
    /// <returns>The custom attribute if it exists; otherwise, <c>null</c>.</returns>
    private TCustomAttr? GetCustomAttributeIfExists<TCustomAttr>() where TCustomAttr : Attribute {
        if (ValueExpression is not null) {
            var body = ValueExpression.Body;

            // Unwrap casts to object
            if (body is UnaryExpression unaryExpression
                && unaryExpression.NodeType == ExpressionType.Convert
                && unaryExpression.Type == typeof(object)) {
                body = unaryExpression.Operand;
            }

            switch (body) {
                case MemberExpression memberExpression:
                    return Attribute.GetCustomAttribute(memberExpression.Member, typeof(TCustomAttr)) as TCustomAttr;

                case MethodCallExpression methodCallExpression:
                    return Attribute.GetCustomAttribute(methodCallExpression.Method, typeof(TCustomAttr)) as TCustomAttr;
            }
        }
        return null;
    }

    /// <summary>
    ///     Gets the maximum length of the input.
    /// </summary>
    /// <returns>The maximum length of the input.</returns>
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

    /// <summary>
    ///     Gets the maximum value of the input.
    /// </summary>
    /// <returns>The maximum value of the input.</returns>
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

    /// <summary>
    ///     Gets the minimum length of the input.
    /// </summary>
    /// <returns>The minimum length of the input.</returns>
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

    /// <summary>
    ///     Gets the minimum value of the input.
    /// </summary>
    /// <returns>The minimum value of the input.</returns>
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

    /// <summary>
    ///     Handles the change event asynchronously.
    /// </summary>
    /// <param name="value">The new value.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnChangeAsync(string? value) {
        CurrentValueAsString = value;
        await BindAfter.InvokeAsync(CurrentValue);
    }

    /// <summary>
    ///     Sets the current value as a string array.
    /// </summary>
    /// <param name="value">The value to set.</param>
    private void SetCurrentValueAsStringArray(string?[]? value) {
        CurrentValue = BindConverter.TryConvertTo<TInputType>(value, CultureInfo.CurrentCulture, out var result)
            ? result
            : default;
    }
}