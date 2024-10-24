using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents;
public enum DialogResult {
    Pending,
    Failed,
    Closed,
    Cancelled = Closed,
    Confirmed,
    Succeeded = Confirmed,
    Deleted
}

