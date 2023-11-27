using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace TnTComponents.Forms;

public partial class TnTTimeOnlyInput {

    [Parameter]
    public TimeOnly? MinTime { get; set; }

    [Parameter]
    public TimeOnly? MaxTime { get; set; }

    [Parameter]
    public int? StepRate { get; set; }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TimeOnly? result, [NotNullWhen(false)] out string? validationErrorMessage)
        => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
}