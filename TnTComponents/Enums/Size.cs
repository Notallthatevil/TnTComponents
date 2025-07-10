using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents;

/// <summary>
///     Represents different sizes that can be used in the application.
/// </summary>
public enum Size {

    /// <summary>
    ///     The smallest size.
    /// </summary>
    Smallest,

    /// <summary>
    ///     Alias for the smallest size.
    /// </summary>
    XS = Smallest,

    /// <summary>
    ///     A small size.
    /// </summary>
    Small,

    /// <summary>
    ///     A medium size.
    /// </summary>
    Medium,

    /// <summary>
    ///     A large size.
    /// </summary>
    Large,

    /// <summary>
    ///     The largest size.
    /// </summary>
    Largest,

    /// <summary>
    ///     Alias for the largest size.
    /// </summary>
    XL = Largest
}