using TnTComponents.Enum;

namespace TnTComponents.Dialogs;

public class DialogOptions {
    public bool CloseOnExternalClick { get; set; } = false;
    public bool ShowClose { get; set; } = true;
    public string CloseIcon { get; set; } = "close";

    public CardType CardType { get; set; } = CardType.Filled;
    public IconType IconType { get; set; } = IconType.MaterialIcons;
}