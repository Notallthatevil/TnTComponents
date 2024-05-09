namespace TnTComponents.Core;

public interface ITnTFlexBox {
    AlignContent? AlignContent { get; set; }
    AlignItems? AlignItems { get; set; }
    LayoutDirection? Direction { get; set; }
    JustifyContent? JustifyContent { get; set; }
}