using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;

namespace TnTComponents;
public class TnTInputText : TnTInputBase<string?> {
    public override string Class => CssBuilder.Create().Build();
    public override InputType Type { get; protected set; } = InputType.Text;
    protected override string? CurrentValueAsString => Value;
}

