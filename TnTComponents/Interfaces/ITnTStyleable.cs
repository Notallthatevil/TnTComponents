﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Interfaces;
public interface ITnTStyleable {
    TextAlign? TextAlignment { get; }
    TnTColor BackgroundColor { get; }

    TnTColor TintColor { get; }

    TnTColor TextColor { get; }
}

