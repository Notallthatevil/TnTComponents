using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Scheduler;

public class TnTEvent {
    [Required]
    public DateTimeOffset? Start { get; set; }

    [Required]
    public DateTimeOffset? End { get; set; }

    [Required]
    public string? Title { get; set; }
}
