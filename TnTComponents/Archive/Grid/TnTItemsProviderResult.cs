using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Archive.Grid;
public class TnTItemsProviderResult<TGridItem> {
    public required ICollection<TGridItem> Items { get; set; }
    public required int Total { get; set; }

}

