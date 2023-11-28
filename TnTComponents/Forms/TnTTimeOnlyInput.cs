using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace TnTComponents.Forms;

public class TnTTimeOnlyInput : TnTInputBase<TimeOnly?> {

    [Parameter]
    public TimeOnly? MaxTime { get; set; }

    [Parameter]
    public TimeOnly? MinTime { get; set; }

    [Parameter]
    public int? StepRate { get; set; }

    protected override TimeOnly? BindValue { get => CurrentValue; set => CurrentValue = value; }

    protected override string GetInputType() => "time";

    protected override string? MaxString() => MaxTime?.ToString("t", CultureInfo.InvariantCulture);

    protected override string? MinString() => MinTime?.ToString("t", CultureInfo.InvariantCulture);

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TimeOnly? result, [NotNullWhen(false)] out string? validationErrorMessage)
                => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
}