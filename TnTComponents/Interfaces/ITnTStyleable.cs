using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;

namespace TnTComponents.Interfaces;
public interface ITnTStyleable {
    TextAlign? TextAlignment { get; }
    TnTColor BackgroundColor { get; }
    TnTColor TextColor { get; }
    int Elevation { get; }
    TnTBorderRadius? BorderRadius { get; }
}

