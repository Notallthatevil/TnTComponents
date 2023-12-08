﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Common;
public abstract class TnTLayoutBase : LayoutComponentBase, ITnTComponentBase {
    [Parameter] public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter] public object? Data { get; set; }
    [Parameter] public string? Id { get; set; }
    [Parameter] public string? Style { get; set; }
    [Parameter] public string? Theme { get; set; }
    public ElementReference Element { get; }

    public virtual string GetClass() => this.GetClassDefault();
}
