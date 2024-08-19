using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents.Toast;

public interface ITnTToast : ITnTStyleable {
    string? Message { get; set; }
    bool ShowClose { get; set; }
    double Timeout { get; set; }
    string Title { get; set; }
    bool Closing { get; }
}
