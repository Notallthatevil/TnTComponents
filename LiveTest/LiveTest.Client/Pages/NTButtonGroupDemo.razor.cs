using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using NTComponents;

namespace LiveTest.Client.Pages;

/// <summary>
///     Demonstrates the <see cref="NTButtonGroup" /> component within the LiveTest client app.
/// </summary>
public partial class NTButtonGroupDemo : ComponentBase {
    private static readonly IReadOnlyList<TnTColor> ColorSwatch = new[] {
        TnTColor.Surface,
        TnTColor.SurfaceVariant,
        TnTColor.Primary,
        TnTColor.Secondary,
        TnTColor.Success,
        TnTColor.Error
    };

    private static readonly IReadOnlyList<NTButtonGroupDemoItem> BaseButtonItems = new[] {
        new NTButtonGroupDemoItem {
            Key = "mail",
            Label = "Mail",
            StartIcon = MaterialIcon.Mail
        },
        new NTButtonGroupDemoItem {
            Key = "calendar",
            Label = "Calendar",
            StartIcon = MaterialIcon.CalendarMonth
        },
        new NTButtonGroupDemoItem {
            Key = "spaces",
            Label = "Spaces",
            StartIcon = MaterialIcon.Groups
        },
        new NTButtonGroupDemoItem {
            Key = "image1",
            StartIcon = MaterialIcon.QrCode
        },
        new NTButtonGroupDemoItem {
            Key = "image2",
            StartIcon = MaterialIcon.Radar
        },
    };

    private static readonly IReadOnlyList<string> ImageSources = new[] {
        BuildImageData("M", "#2563eb"),
        BuildImageData("C", "#9333ea"),
        BuildImageData("S", "#047857")
    };

    private IReadOnlyList<NTButtonGroupDemoItem> DisplayItems => BaseButtonItems;

    private bool UseImages { get; set; }

    private NTButtonGroupDisplayType DisplayType { get; set; } = NTButtonGroupDisplayType.Disconnected;


    private Size ButtonSize { get; set; } = Size.Medium;

    private ButtonAppearance Appearance { get; set; } = ButtonAppearance.Filled;

    private ButtonAppearance SelectedAppearance { get; set; } = ButtonAppearance.Filled;

    private TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceVariant;

    private bool DisableRipple { get; set; }

    private bool Disabled { get; set; }

    private bool StopPropagation { get; set; } = true;

    private bool UseTintColors { get; set; } = true;

    private bool UseSelectedTintColors { get; set; } = true;

    private bool UseSelectedTextColor { get; set; } = true;

    private TnTColor SelectedTintColor { get; set; } = TnTColor.Primary;

    private TnTColor SelectedOnTintColor { get; set; } = TnTColor.OnPrimary;

    private TnTColor SelectedTextColor { get; set; } = TnTColor.OnPrimary;

    private string SelectedKey { get; set; } = BaseButtonItems[0].Key;

    private string SelectionMessage { get; set; } = $"Selected {BaseButtonItems[0].Label}";


    private string? SelectedKeyValue => string.IsNullOrWhiteSpace(SelectedKey) ? null : SelectedKey;

    private Task HandleSelectionChanged(NTButtonGroupItem<string> item) {
        SelectionMessage = $"Selection changed to {item.Label ?? item.Key}";
        return Task.CompletedTask;
    }

    private Task HandleSelectedKeyChanged(string? newKey) {
        SelectedKey = newKey ?? string.Empty;
        SelectionMessage = string.IsNullOrWhiteSpace(newKey)
            ? "Selection cleared"
            : $"Explicitly selected {newKey}";
        return Task.CompletedTask;
    }

    private static string BuildImageData(string glyph, string fillColor) {
        var svg = $"<svg xmlns='http://www.w3.org/2000/svg' width='28' height='28'><rect width='28' height='28' rx='8' fill='{fillColor}'/><text x='50%' y='55%' font-size='14' font-weight='600' fill='white' text-anchor='middle' dominant-baseline='middle'>{glyph}</text></svg>";
        return $"data:image/svg+xml,{Uri.EscapeDataString(svg)}";
    }

    private sealed record NTButtonGroupDemoItem {
        public required string Key { get; init; }
        public string? Label { get; init; }
        public TnTIcon? StartIcon { get; init; }
        public TnTIcon? EndIcon { get; init; }
        public bool Disabled { get; init; }
        public bool IsDefaultSelected { get; init; }
    }
}
