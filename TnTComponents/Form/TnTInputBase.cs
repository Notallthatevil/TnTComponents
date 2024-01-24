using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using TnTComponents.Core;
using TnTComponents.Form;

namespace TnTComponents;

public abstract partial class TnTInputBase<TInputType> : InputBase<TInputType>, IFormField {

    [Parameter]
    public FormAppearance Appearance { get; set; }

    [Parameter]
    public TnTColor? BackgroundColor { get; set; }

    [Parameter]
    public EventCallback<TInputType?> BindAfter { get; set; }

    [Parameter]
    public bool BindOnInput { get; set; }

    public virtual string Class => CssBuilder.Create()
                    .AddOutlined(Appearance == FormAppearance.Outlined)
        .AddFilled(Appearance == FormAppearance.Filled)
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .Build();

    [Parameter]
    public bool Disabled { get; set; }

    public ElementReference Element { get; protected set; }

    [Parameter]
    public MaterialIcons? EndIcon { get; set; }

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
    public MaterialIcons? StartIcon { get; set; }

    [Parameter]
    public string? Style { get; set; }

    [Parameter]
    public TnTColor? TextColor { get; set; }

    public abstract InputType Type { get; protected set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        // TODO add start icon

        {
            builder.OpenElement(100, "input");
            builder.AddMultipleAttributes(110, AdditionalAttributes);
            builder.AddAttribute(120, "type", Type.ToInputTypeString());
            builder.AddAttribute(125, "value", CurrentValueAsString);
            builder.AddAttribute(130, "class", Class);
            builder.AddAttribute(140, "style", Style);
            builder.AddAttribute(150, "readonly", ReadOnly);
            builder.AddAttribute(160, "placeholder", Placeholder);
            builder.AddAttribute(170, "disabled", Disabled);
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
            builder.SetUpdatesAttributeName("value");


            builder.AddElementReferenceCapture(200, e => Element = e);
            builder.CloseElement();
        }
        //  TODO add end icon
    }

    protected override void OnInitialized() {
        base.OnInitialized();
        if (ParentFormAppearance.HasValue) {
            Appearance = ParentFormAppearance.Value;
        }

        if (ParentFormReadOnly.HasValue) {
            ReadOnly = ParentFormReadOnly.Value;
        }

        if (ParentFormDisabled.HasValue) {
            Disabled = ParentFormDisabled.Value;
        }

        Label?.SetChildField(this);

        if (ValueExpression is not null) {
            FieldIdentifier = FieldIdentifier.Create(ValueExpression);
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
            return max.ToString();
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
            return min.ToString();
        }
        var rangeAttr = GetCustomAttributeIfExists<RangeAttribute>();
        if (rangeAttr is not null) {
            return rangeAttr.Minimum.ToString();
        }

        return null;
    }

    private bool IsRequired() {
        if (AdditionalAttributes?.TryGetValue("required", out var _) == true || GetCustomAttributeIfExists<RequiredAttribute>() is not null) {
            return true;
        }

        return false;
    }

    private async Task OnChangedCallback(ChangeEventArgs args) {
        var tValue = (TInputType?)args.Value;
        //CurrentValue = tValue;
        //await BindAfter.InvokeAsync(Value);
    }
}