using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Common;
public abstract class TnTComponentBase : ComponentBase {

    public ElementReference Element { get; protected set; }

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

    [Parameter(CaptureUnmatchedValues = true)]
    public virtual IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    protected override void OnParametersSet() {
        if(string.IsNullOrWhiteSpace(Id)) {
            Id = Guid.NewGuid().ToString();
        }
        base.OnParametersSet();
    }


    protected virtual string GetClass() {
        var strBuilder = new StringBuilder(Class ?? string.Empty);

        if (AdditionalAttributes?.TryGetValue("class", out var @class) == true) {
            strBuilder.Append(' ').AppendJoin(' ', @class);
        }

        return strBuilder.ToString();
    }
}

