using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents;
/// <summary>
/// Represents the possible results of a dialog operation.
/// </summary>
public enum DialogResult {
    /// <summary>
    /// The dialog is pending.
    /// </summary>
    Pending,
    /// <summary>
    /// The dialog has failed.
    /// </summary>
    Failed,
    /// <summary>
    /// The dialog has been closed.
    /// </summary>
    Closed,
    /// <summary>
    /// The dialog has been cancelled. This is equivalent to <see cref="Closed"/>.
    /// </summary>
    Cancelled = Closed,
    /// <summary>
    /// The dialog has been confirmed.
    /// </summary>
    Confirmed,
    /// <summary>
    /// The dialog has succeeded. This is equivalent to <see cref="Confirmed"/>.
    /// </summary>
    Succeeded = Confirmed,
    /// <summary>
    /// The dialog has been deleted.
    /// </summary>
    Deleted
}

