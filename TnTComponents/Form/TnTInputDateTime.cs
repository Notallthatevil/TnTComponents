using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace TnTComponents;

public class TnTInputDateOnly : TnTInputBase<DateOnly?> {

    [Parameter]
    public string Format { get; set; } = "MM/dd/yyyy hh:mm:ss";

    public override InputType Type => InputType.Date;

    protected override string? FormatValueAsString(DateOnly? value) => BindConverter.FormatValue(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);

    protected override void OnInitialized() {
        base.OnInitialized();
        var attributes = AdditionalAttributes is null ? new Dictionary<string, object>() : new Dictionary<string, object>(AdditionalAttributes);
        attributes.Add("format", "YYYY-MM-DDThh:mm");
        AdditionalAttributes = attributes;
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out DateOnly? result, [NotNullWhen(false)] out string? validationErrorMessage) {
        if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out result)) {
            Debug.Assert(result != null);
            validationErrorMessage = null;
            return true;
        }
        else {
            validationErrorMessage = $"Failed to parse {value} into a {typeof(DateOnly).Name}";
            return false;
        }
    }
}

public class TnTInputDateTime : TnTInputBase<DateTime?> {

    [Parameter]
    public string Format { get; set; } = "MM/dd/yyyy hh:mm:ss";

    public override InputType Type => InputType.DateTime;

    protected override string? FormatValueAsString(DateTime? value) => BindConverter.FormatValue(value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);

    protected override void OnInitialized() {
        base.OnInitialized();
        var attributes = AdditionalAttributes is null ? new Dictionary<string, object>() : new Dictionary<string, object>(AdditionalAttributes);
        attributes.Add("format", "YYYY-MM-DDThh:mm");
        AdditionalAttributes = attributes;
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out DateTime? result, [NotNullWhen(false)] out string? validationErrorMessage) {
        if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out result)) {
            Debug.Assert(result != null);
            validationErrorMessage = null;
            return true;
        }
        else {
            validationErrorMessage = $"Failed to parse {value} into a {typeof(DateTime).Name}";
            return false;
        }
    }
}

public class TnTInputTimeOnly : TnTInputBase<TimeOnly?> {

    [Parameter]
    public string Format { get; set; } = "MM/dd/yyyy hh:mm:ss";

    public override InputType Type => InputType.Time;

    protected override string? FormatValueAsString(TimeOnly? value) => BindConverter.FormatValue(value, "HH:mm:ss", CultureInfo.InvariantCulture);

    protected override void OnInitialized() {
        base.OnInitialized();
        var attributes = AdditionalAttributes is null ? new Dictionary<string, object>() : new Dictionary<string, object>(AdditionalAttributes);
        attributes.Add("format", "YYYY-MM-DDThh:mm");
        AdditionalAttributes = attributes;
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TimeOnly? result, [NotNullWhen(false)] out string? validationErrorMessage) {
        if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out result)) {
            Debug.Assert(result != null);
            validationErrorMessage = null;
            return true;
        }
        else {
            validationErrorMessage = $"Failed to parse {value} into a {typeof(TimeOnly).Name}";
            return false;
        }
    }
}

public class TnTInputMonth : TnTInputBase<DateOnly?> {

    [Parameter]
    public string Format { get; set; } = "MM/dd/yyyy hh:mm:ss";

    public override InputType Type => InputType.Month;

    protected override string? FormatValueAsString(DateOnly? value) => BindConverter.FormatValue(value, "yyyy-MM", CultureInfo.InvariantCulture);

    protected override void OnInitialized() {
        base.OnInitialized();
        var attributes = AdditionalAttributes is null ? new Dictionary<string, object>() : new Dictionary<string, object>(AdditionalAttributes);
        attributes.Add("format", "YYYY-MM-DDThh:mm");
        AdditionalAttributes = attributes;
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out DateOnly? result, [NotNullWhen(false)] out string? validationErrorMessage) {
        if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out result)) {
            Debug.Assert(result != null);
            validationErrorMessage = null;
            return true;
        }
        else {
            validationErrorMessage = $"Failed to parse {value} into a {typeof(DateOnly).Name}";
            return false;
        }
    }
}