using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Interfaces;
public interface ITnTInteractable {
    /// <summary>
    /// Gets or sets a value indicating whether the component is disabled.
    /// </summary>
    bool Disabled { get; set; }
}

