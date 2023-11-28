using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace TnTComponents.Forms;

public class TnTTimeOnlyInput : TnTInputBase<TimeOnly?> {

    [Parameter]
    public TimeOnly? MaxTime { get; set; }

    [Parameter]
    public TimeOnly? MinTime { get; set; }

    protected override TimeOnly? BindValue { get => CurrentValue; set => CurrentValue = value; }
    protected override string InputType => "time";
    protected override string? Max => MaxTime?.ToString("t", CultureInfo.InvariantCulture);
    protected override string? Min => MinTime?.ToString("t", CultureInfo.InvariantCulture);

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TimeOnly? result, [NotNullWhen(false)] out string? validationErrorMessage)
                => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
}