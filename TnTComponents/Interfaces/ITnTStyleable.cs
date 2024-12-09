using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;

namespace TnTComponents.Interfaces;

/// <summary>
///     Interface representing styleable components with various styling properties.
/// </summary>
public interface ITnTStyleable {

    /// <summary>
    ///     Gets the background color.
    /// </summary>
    TnTColor BackgroundColor { get; }

    /// <summary>
    ///     Gets the border radius.
    /// </summary>
    TnTBorderRadius? BorderRadius { get; }

    /// <summary>
    ///     Gets the elevation level.
    /// </summary>
    int Elevation { get; }

    /// <summary>
    ///     Gets the text alignment.
    /// </summary>
    TextAlign? TextAlignment { get; }

    /// <summary>
    ///     Gets the text color.
    /// </summary>
    TnTColor TextColor { get; }
}
