using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Form;
public interface IFormField : IFormItem {
    InputType Type { get; }

    string? Placeholder { get; }
    
    TnTIcon? StartIcon { get;  }
    TnTIcon? EndIcon { get;  }
}

