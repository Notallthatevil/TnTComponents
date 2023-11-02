using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Dialogs;

public class Dialog {
    public required Type Type { get; set; }
    public required string Title { get; set; }
    public IReadOnlyDictionary<string, object>? Parameters { get; set; }
    public DialogOptions Options { get; set; } = new();
}

