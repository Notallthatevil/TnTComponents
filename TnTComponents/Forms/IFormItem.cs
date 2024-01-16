using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Forms;
public interface IFormItem {
    bool ParentFormDisabled { get; }
    bool ParentFormReadOnly { get; }
    FormAppearance ParentFormAppearance { get; }

    bool Disabled { get; set; }

    bool ReadOnly { get; set; }

    FormAppearance Appearance { get; set; };


    TnTForm? ParentForm { get; set; }
}

