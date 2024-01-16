using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TnTComponents.Common;
using TnTComponents.Enum;

namespace TnTComponents.Forms;

public abstract partial class TnTInputBase<TInputType> {

    [Parameter]
    public string? AriaLabel { get; set; }

    [Parameter]
    public bool AutoFocus { get; set; }

    [Parameter]
    public virtual string? Class { get; set; }

    [Parameter]
    public string ContainerClass { get; set; } = DefaultContainerClass;

    [Parameter]
    public virtual object? Data { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    public ElementReference Element { get; protected set; }

    [Parameter]
    public string? EndIcon { get; set; }

    [Parameter]
    public FormType FormType { get; set; }

    [Parameter]
    public IconType IconType { get; set; }

    [Parameter]
    public virtual string? Id { get; set; }

    [Parameter]
    public string? Label { get; set; }

    [Parameter]
    public string LabelClass { get; set; } = DefaultLabelClass;

    [Parameter]
    public string LabelTextClass { get; set; } = DefaultLabelTextClass;

    [Parameter]
    public string? Placeholder { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public bool ShowValidation { get; set; } = true;

    [Parameter]
    public string? StartIcon { get; set; }

    [Parameter]
    public double? StepRate { get; set; }

    [Parameter]
    public virtual string? Style { get; set; }

    [Parameter]
    public string? SupportingText { get; set; }

    [Parameter]
    public string SupportingTextClass { get; set; } = DefaultSupportingTextClass;

    [Parameter]
    public virtual string? Theme { get; set; }

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public string ValidationClass { get; set; } = DefaultValidationClass;

    protected abstract TInputType BindValue { get; set; }
    protected abstract string InputType { get; }
    protected virtual object? InputValue { get; set; }
    protected virtual string? Max => MaxValue?.ToString();
    protected int? MaxLength { get; private set; }
    protected TInputType? MaxValue { get; private set; }
    protected virtual string? Min => MinValue?.ToString();
    protected int? MinLength { get; private set; }
    protected TInputType? MinValue { get; private set; }
    protected string? Pattern { get; private set; }
    protected bool Required { get; private set; }

    [Inject]
    private ILogger<TnTInputBase<TInputType>>? _logger { get; set; }

    [CascadingParameter(Name = TnTForm.ParentFormTypeName)]
    private FormType? _parentFormType { get; set; }

    internal const string DefaultContainerClass = "tnt-input-container";
    internal const string DefaultLabelClass = "tnt-input-label";
    internal const string DefaultLabelTextClass = "tnt-input-label-text";
    internal const string DefaultSupportingTextClass = "tnt-input-supporting-text";
    internal const string DefaultValidationClass = "tnt-input-validation";

    public virtual string GetClass() => string.Join(' ', this.GetClassDefault(), CssClass);

    protected virtual string GetFormType() => FormType.ToString().ToLower();

    protected override void OnParametersSet() {
        var fieldName = FieldIdentifier.FieldName;

        if (EditContext is not null) {

            var modelType = EditContext.Model.GetType();
            var property = modelType.GetProperty(fieldName) ?? throw new InvalidOperationException($"Unable to bind {fieldName}. It is not a valid property of {modelType}!");

            var maxLengthAttribute = property.GetCustomAttribute<MaxLengthAttribute>();
            var minLengthAttribute = property.GetCustomAttribute<MinLengthAttribute>();
            var stringLengthAttribute = property.GetCustomAttribute<StringLengthAttribute>();
            var rangeAttribute = property.GetCustomAttribute<RangeAttribute>();
            var requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();
            var regularExpressionAttribute = property.GetCustomAttribute<RegularExpressionAttribute>();

            if (maxLengthAttribute is not null) {
                MaxLength = maxLengthAttribute.Length;
            }

            if (minLengthAttribute is not null) {
                MinLength = minLengthAttribute.Length;
            }

            if (property.PropertyType == typeof(string)) {
                if (stringLengthAttribute is not null) {
                    if (stringLengthAttribute.MinimumLength != 0) {
                        if (minLengthAttribute is not null) {
                            _logger?.LogWarning($"Overriding {nameof(MinLengthAttribute)}.{nameof(MinLengthAttribute.Length)} value with {nameof(StringLengthAttribute.MinimumLength)} since it was also set.");
                        }

                        MinLength = stringLengthAttribute.MinimumLength;
                    }
                    if (minLengthAttribute is not null) {
                        _logger?.LogWarning($"Overriding {nameof(MaxLengthAttribute)}.{nameof(MaxLengthAttribute.Length)} value with {nameof(StringLengthAttribute.MaximumLength)} since it was also set.");
                    }
                    MaxLength = stringLengthAttribute.MaximumLength;
                }
            }

            if (rangeAttribute is not null) {
                if (rangeAttribute.OperandType != typeof(TInputType) && rangeAttribute.OperandType != typeof(TInputType?)) {
                    throw new InvalidOperationException($"{rangeAttribute.OperandType.GetType().Name} must be the same type as {typeof(TInputType).Name}!");
                }
                MinValue = (TInputType?)rangeAttribute.Minimum;
                MaxValue = (TInputType?)rangeAttribute.Maximum;
            }

            if (requiredAttribute is not null) {
                Required = true;
            }

            if (regularExpressionAttribute is not null) {
                Pattern = regularExpressionAttribute.Pattern;
            }
        }

        AriaLabel ??= FieldIdentifier.FieldName;

        if (_parentFormType.HasValue) {
            FormType = _parentFormType.Value;
        }
        base.OnParametersSet();
    }
}