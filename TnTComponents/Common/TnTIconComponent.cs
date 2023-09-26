using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Enum;

namespace TnTComponents.Common {
    public class TnTIconComponent : TnTBaseComponent{

        [Parameter]
        public string Icon { get; set; } = default!;

        [Parameter]
        public IconType IconType { get; set; }

        internal MarkupString GetIcon() {
            if(!string.IsNullOrWhiteSpace(Icon)) {
                switch(IconType) {
                    case IconType.MaterialIcons:
                        return new MarkupString($@"<i class=""material-icons"">{Icon}</i>");

                    case IconType.FontAwesome:
                        return new MarkupString($@"<i class=""fa {Icon}""></i>");
                }
            }
            return default;
        }

    }
}
