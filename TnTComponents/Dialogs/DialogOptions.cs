using TnTComponents.Enum;

namespace TnTComponents.Dialogs;

public class DialogOptions {
    public CardType CardType { get; set; } = CardType.Filled;
    public string CloseIcon { get; set; } = "close";
    public bool CloseOnExternalClick { get; set; } = false;
    public IconType IconType { get; set; } = IconType.MaterialIcons;
    public bool ShowClose { get; set; } = true;
}