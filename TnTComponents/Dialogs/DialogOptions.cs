using TnTComponents.Enum;

namespace TnTComponents.Dialogs;

public class DialogOptions {
    public CardAppearance CardType { get; set; } = CardAppearance.Filled;
    public string CloseIcon { get; set; } = "close";
    public bool CloseOnExternalClick { get; set; } = false;
    public IconType IconType { get; set; } = IconType.MaterialIcons;
    public bool ShowClose { get; set; } = true;
}