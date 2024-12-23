using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents;

/// <summary>
/// Represents different sizes that can be used in the application.
/// </summary>
public enum Size {
    /// <summary>
    /// The smallest size.
    /// </summary>
    Smallest,

    /// <summary>
    /// A small size.
    /// </summary>
    Small,

    /// <summary>
    /// A medium size.
    /// </summary>
    Medium,

    /// <summary>
    /// The default size, which is set to Medium.
    /// </summary>
    Default = Medium,

    /// <summary>
    /// A large size.
    /// </summary>
    Large,

    /// <summary>
    /// The largest size.
    /// </summary>
    Largest
}
