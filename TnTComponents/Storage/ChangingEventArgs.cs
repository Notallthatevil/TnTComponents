using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Storage;
public class ChangingEventArgs {
    public required string Key { get; init; }
    public object? OldValue { get; init; }
    public object? NewValue { get; init; }
    public bool Cancel { get; init; }
}

