using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Layout;

namespace TnTComponents.Infrastructure;
internal class TnTLayoutContext(TnTLayout layout) {

    internal readonly TnTLayout Layout = layout;

    internal TnTHeader? Header;
    internal TnTBody Body = default!;
    internal TnTSideNav? SideNav;
    internal TnTFooter? Footer;

}

