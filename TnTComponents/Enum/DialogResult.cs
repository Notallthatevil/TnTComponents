using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Enum;
public enum DialogResult {
    Closed,
    Cancelled = Closed,
    Confirmed,
    Succeeded = Confirmed
}

