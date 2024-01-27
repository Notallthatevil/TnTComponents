using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;

namespace TnTComponents;
public class TnTInputCheckbox : TnTInputBase<bool> {
    public override InputType Type => InputType.Checkbox;

    [Parameter]
    public InputCheckboxAppearance CheckboxAppearance { get; set; }

    protected override void OnInitialized() {
        base.OnInitialized();
        if (Label is null) {
            throw new InvalidOperationException($"{GetType().Name} must be a child of {nameof(TnTLabel)}");
        }
    }

    public override string Class => CssBuilder.Create(base.Class).AddClass(AppearanceClass()).Build();

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out bool result, [NotNullWhen(false)] out string? validationErrorMessage) {
        throw new NotSupportedException();
    }

    private string AppearanceClass() {
        return CheckboxAppearance switch {
            InputCheckboxAppearance.Checkbox => string.Empty,
            InputCheckboxAppearance.Switch => "tnt-alternative",
            _ => throw new InvalidOperationException($"{CheckboxAppearance} is not a valid value of {nameof(InputCheckboxAppearance)}"),
        };
    }
}

