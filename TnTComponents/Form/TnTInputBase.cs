using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Form;

namespace TnTComponents;
public abstract class TnTInputBase<TInputType> : IFormField {
    public abstract InputType Type { get; }

    [CascadingParameter(Name = nameof(ParentFormDisabled))]
    public bool ParentFormDisabled { get; }

    [CascadingParameter(Name = nameof(ParentFormReadOnly))]
    public bool ParentFormReadOnly { get; }

    [CascadingParameter(Name = nameof(ParentFormAppearance))]
    public FormAppearance ParentFormAppearance { get; }

    [Parameter]
    public bool Disabled { get; set; }
    [Parameter]
    public bool ReadOnly { get; set; }
    [Parameter]
    public FormAppearance Appearance { get; set; }
    [Parameter]
    public TnTForm? ParentForm { get; set; }
}

