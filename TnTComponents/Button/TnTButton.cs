using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Button;
public class TnTButton : TnTButtonBase {
    protected override string GetClass() {
        return "tnt-btn " + Class;
    }
}
