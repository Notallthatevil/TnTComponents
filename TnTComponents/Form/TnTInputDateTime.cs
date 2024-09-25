using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace TnTComponents;

public class TnTInputDateTime<DateTimeType> : TnTInputBase<DateTimeType> {

    [Parameter]
    public string Format { get; set; } = default!;

    [Parameter]
    public bool MonthOnly { get; set; }

    public override InputType Type => _type;

    private InputType _type;

    private DateTimeType _value;

    protected override string? FormatValueAsString(DateTimeType? value) {
        var result = value switch {
            DateTime dateTimeValue => BindConverter.FormatValue(dateTimeValue, _format, CultureInfo.InvariantCulture),
            DateTimeOffset dateTimeOffsetValue => BindConverter.FormatValue(dateTimeOffsetValue, _format, CultureInfo.InvariantCulture),
            DateOnly dateOnlyValue => BindConverter.FormatValue(dateOnlyValue, _format, CultureInfo.InvariantCulture),
            TimeOnly timeOnlyValue => BindConverter.FormatValue(timeOnlyValue, _format, CultureInfo.InvariantCulture),
            _ => string.Empty, // Handles null for Nullable<DateTime>, etc.
        };

        return result;
    }

    private string _format = default!;

    protected override void OnInitialized() {
        base.OnInitialized();

        _format = (Nullable.GetUnderlyingType(typeof(DateTimeType)) ?? typeof(DateTimeType)) switch {
            var t when t == typeof(DateTime) => "yyyy-MM-ddTHH:mm:ss",
            var t when t == typeof(DateTimeOffset) => "yyyy-MM-ddTHH:mm:ss",
            var t when t == typeof(TimeOnly) => "HH:mm:ss",
            var t when t == typeof(DateOnly) => MonthOnly ? "yyyy-MM" : "yyyy-MM-dd",
            _ => throw new InvalidOperationException($"The type '{typeof(DateTimeType)}' is not a supported DateTime type.")
        };

        _type = (Nullable.GetUnderlyingType(typeof(DateTimeType)) ?? typeof(DateTimeType)) switch {
            var t when t == typeof(DateTime) => InputType.DateTime,
            var t when t == typeof(DateTimeOffset) => InputType.DateTime,
            var t when t == typeof(TimeOnly) => InputType.Time,
            var t when t == typeof(DateOnly) => MonthOnly ? InputType.Month : InputType.Date,
            _ => throw new InvalidOperationException($"The type '{typeof(DateTimeType)}' is not a supported DateTime type.")
        };

        var attributes = AdditionalAttributes is null ? new Dictionary<string, object>() : new Dictionary<string, object>(AdditionalAttributes);
        attributes.Add("format", _format);
        AdditionalAttributes = attributes;
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (string.IsNullOrWhiteSpace(Format)) {
            Format = _format;
        }
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out DateTimeType result, [NotNullWhen(false)] out string? validationErrorMessage) {
        Console.WriteLine("Try Parse Value From String");
        if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out result)) {
            validationErrorMessage = null;
            return true;
        }
        else {
            validationErrorMessage = $"Failed to parse {value} into a {typeof(DateTimeType).Name}";
            return false;
        }
    }
}