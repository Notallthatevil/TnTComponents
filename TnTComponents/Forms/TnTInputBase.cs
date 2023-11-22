using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Common;
using TnTComponents.Enum;

namespace TnTComponents.Forms;
public abstract class TnTInputBase<TInputType> : InputBase<TInputType>, ITnTComponentBase {
    [CascadingParameter(Name = TnTForm.ParentFormTypeName)]
    private FormType? _parentFormType { get; set; }

    [Parameter]
    public FormType FormType { get; set; }

    public ElementReference Element { get; protected set; }

    [Parameter]
    public string ContainerClass { get; set; } = "tnt-input-container";

    [Parameter]
    public string LabelClass { get; set; } = "tnt-input-label";

    [Parameter]
    public string LabelTextClass { get; set; } = "tnt-input-label-text";

    [Parameter]
    public string SupportingTextClass { get; set; } = "tnt-input-supporting-text";

    [Parameter]
    public string ValidationClass { get; set; } = "tnt-input-validation";

    [Parameter]
    public virtual string? Id { get; set; }

    [Parameter]
    public virtual string? Class { get; set; }

    [Parameter]
    public virtual string? Theme { get; set; }

    [Parameter]
    public virtual string? Style { get; set; }

    [Parameter]
    public virtual object? Data { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public bool AutoFocus { get; set; }

    [Parameter]
    public string? Placeholder { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public bool ShowValidation { get; set; } = true;

    [Parameter]
    public string? AriaLabel { get; set; }

    [Parameter]
    public string? Label { get; set; }
    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public string? StartIcon { get; set; }

    [Parameter]
    public string? EndIcon { get; set; }

    [Parameter]
    public IconType IconType { get; set; }

    [Parameter]
    public string? SupportingText { get; set; }

    protected int? MaxLength { get; private set; }
    protected int? MinLength { get; private set; }
    protected TInputType? MinValue { get; private set; }
    protected TInputType? MaxValue { get; private set; }
    protected string? Pattern { get; private set; }
    protected bool Required { get; private set; }

    [Inject]
    private ILogger<TnTInputBase<TInputType>>? _logger { get; set; }

    public string GetClass() => string.Join(' ', this.GetClassDefault(), CssClass);


    protected override void OnParametersSet() {

        var modelType = EditContext.Model.GetType();
        var fieldName = FieldIdentifier.FieldName;

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

        AriaLabel ??= FieldIdentifier.FieldName;

        if (_parentFormType.HasValue) {
            FormType = _parentFormType.Value;
        }

        base.OnParametersSet();
    }
}



