using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Forms;
internal sealed class TnTRadioButtonContext(TnTRadioButtonContext? parentContext, EventCallback<ChangeEventArgs> changeEventCallback, string groupName) {
    public TnTRadioButtonContext? ParentContext { get; set; } = parentContext;
    public EventCallback<ChangeEventArgs> ChangeEventCallback { get; set; } = changeEventCallback;

    public string GroupName { get; private set; } = groupName;

    public object? CurrentValue { get; set; }

    public string? FieldClass { get; set; }

    public TnTRadioButtonContext? FindContextInAncestors(string groupName) {
        if (!GroupName.Equals(groupName)) {
            return ParentContext?.FindContextInAncestors(groupName);
        }
        return null;
    }
}

