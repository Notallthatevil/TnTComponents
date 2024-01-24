using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;
using TnTComponents.Enum;

namespace TnTComponents;
public class TnTInputText : TnTInputBase<string?> {
    public override InputType Type => InputType.ToInputType();

    [Parameter]
    public TextInputType InputType { get; set; } = TextInputType.Text;


    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out string? result, [NotNullWhen(false)] out string? validationErrorMessage) {
        result = value;
        validationErrorMessage = null;
        return true;
    }
}

