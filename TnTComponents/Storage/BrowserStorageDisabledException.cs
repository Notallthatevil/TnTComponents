using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Storage;
public class BrowserStorageDisabledException : Exception {
    public BrowserStorageDisabledException() {
    }

    public BrowserStorageDisabledException(string message) : base(message) {
    }

    public BrowserStorageDisabledException(string message, Exception inner) : base(message, inner) {
    }
}

