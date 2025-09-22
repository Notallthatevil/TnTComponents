using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Storage;
/// <summary>
/// Exception thrown when browser storage is disabled.
/// </summary>
[ExcludeFromCodeCoverage]
public class BrowserStorageDisabledException : Exception {
    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserStorageDisabledException"/> class.
    /// </summary>
    public BrowserStorageDisabledException() {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserStorageDisabledException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public BrowserStorageDisabledException(string message) : base(message) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserStorageDisabledException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public BrowserStorageDisabledException(string message, Exception inner) : base(message, inner) {
    }
}

