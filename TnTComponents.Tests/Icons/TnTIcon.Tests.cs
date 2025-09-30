﻿using TnTComponents.Core;

namespace TnTComponents.Tests.Icons;

/// <summary>
///     Test implementation of TnTIcon for testing purposes.
/// </summary>
internal sealed class TestIcon : TnTIcon {

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("test-icon")
        .AddClass("test-small", Size == IconSize.Small)
        .AddClass("test-medium", Size == IconSize.Medium)
        .AddClass("test-large", Size == IconSize.Large)
        .AddClass("test-extra-large", Size == IconSize.ExtraLarge)
        .AddClass("test-outlined", Appearance == IconAppearance.Outlined)
        .AddClass("test-round", Appearance == IconAppearance.Round)
        .AddClass("test-sharp", Appearance == IconAppearance.Sharp)
        .AddClass(AdditionalClass, !string.IsNullOrWhiteSpace(AdditionalClass))
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("test-icon-color", Color, Color != TnTColor.OnSurface)
        .Build();

    public TestIcon() {
    }

    public TestIcon(string icon) : base(icon) {
    }
}

public class TnTIcon_Tests : BunitContext {

    [Fact]
    public void Constructor_Parameterless_InitializesWithDefaults() {
        // Arrange & Act
        var icon = new TestIcon();

        // Assert
        icon.Appearance.Should().Be(IconAppearance.Default);
        icon.Color.Should().Be(TnTColor.OnSurface);
        icon.Size.Should().Be(IconSize.Medium);
        icon.Icon.Should().Be(default(string));
    }

    [Fact]
    public void Constructor_WithIcon_SetsIconProperty() {
        // Arrange & Act
        var icon = new TestIcon("test-icon");

        // Assert
        icon.Icon.Should().Be("test-icon");
    }

    [Fact]
    public void HasTnTIdAttribute() {
        // Arrange & Act
        var cut = Render<TestIcon>(p => p.Add(c => c.Icon, "dashboard"));
        var root = cut.Find("span.test-icon");

        // Assert
        root.HasAttribute("tntid").Should().BeTrue();
        root.GetAttribute("tntid").Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void ImplicitOperator_ReturnsIconString() {
        // Arrange
        var icon = new TestIcon("account_circle");

        // Act
        string iconString = icon;

        // Assert
        iconString.Should().Be("account_circle");
    }

    [Fact]
    public void Render_CapturesElementReference() {
        // Arrange & Act
        var icon = new TestIcon("notifications");
        var cut = Render(icon.Render());

        // Assert Element reference should be captured (testing that the capture method is called)
        cut.Find("span").Should().NotBeNull();
    }

    [Fact]
    public void Render_MultipleTimesWithDifferentAdditionalClass_UpdatesAdditionalClass() {
        // Arrange
        var icon = new TestIcon("refresh");

        // Act
        var cut1 = Render(icon.Render("first-class"));
        var cut2 = Render(icon.Render("second-class"));

        // Assert
        cut1.Find("span").GetAttribute("class")!.Should().Contain("first-class");
        cut2.Find("span").GetAttribute("class")!.Should().Contain("second-class");
    }

    [Fact]
    public void Render_WithAdditionalAttributes_MergesClassAttribute() {
        // Arrange
        var attrs = new Dictionary<string, object> { { "class", "extra-class" } };

        // Act
        var cut = Render<TestIcon>(p => p.Add(c => c.AdditionalAttributes, attrs).Add(c => c.Icon, "edit"));

        // Assert
        var cls = cut.Find("span").GetAttribute("class")!;
        cls.Should().Contain("extra-class");
        cls.Should().Contain("test-icon");
    }

    [Fact]
    public void Render_WithAdditionalAttributes_MergesStyleAttribute() {
        // Arrange
        var attrs = new Dictionary<string, object> { { "style", "margin: 5px;" } };

        // Act
        var cut = Render<TestIcon>(p => p.Add(c => c.AdditionalAttributes, attrs).Add(c => c.Icon, "delete"));

        // Assert
        var style = cut.Find("span").GetAttribute("style")!;
        style.Should().Contain("margin: 5px;");
    }

    [Fact]
    public void Render_WithAdditionalClass_IncludesAdditionalClass() {
        // Arrange
        var icon = new TestIcon("close");

        // Act
        var cut = Render(icon.Render("custom-class"));

        // Assert
        cut.Find("span").GetAttribute("class")!.Should().Contain("custom-class");
    }

    [Theory]
    [InlineData(IconAppearance.Default)]
    [InlineData(IconAppearance.Outlined)]
    [InlineData(IconAppearance.Round)]
    [InlineData(IconAppearance.Sharp)]
    public void Render_WithAllAppearances_RendersCorrectly(IconAppearance appearance) {
        // Arrange & Act
        var cut = Render<TestIcon>(p => p.Add(c => c.Appearance, appearance).Add(c => c.Icon, "test"));
        var cls = cut.Find("span").GetAttribute("class")!;

        // Assert
        switch (appearance) {
            case IconAppearance.Default:
                cls.Should().NotContain("test-outlined");
                cls.Should().NotContain("test-round");
                cls.Should().NotContain("test-sharp");
                break;

            case IconAppearance.Outlined:
                cls.Should().Contain("test-outlined");
                break;

            case IconAppearance.Round:
                cls.Should().Contain("test-round");
                break;

            case IconAppearance.Sharp:
                cls.Should().Contain("test-sharp");
                break;
        }
    }

    [Theory]
    [InlineData(IconSize.Small)]
    [InlineData(IconSize.Medium)]
    [InlineData(IconSize.Large)]
    [InlineData(IconSize.ExtraLarge)]
    public void Render_WithAllSizes_RendersCorrectly(IconSize size) {
        // Arrange & Act
        var cut = Render<TestIcon>(p => p.Add(c => c.Size, size).Add(c => c.Icon, "test"));

        // Assert
        var expectedClass = size switch {
            IconSize.Small => "test-small",
            IconSize.Medium => "test-medium",
            IconSize.Large => "test-large",
            IconSize.ExtraLarge => "test-extra-large",
            _ => throw new ArgumentOutOfRangeException(nameof(size))
        };
        cut.Find("span").GetAttribute("class")!.Should().Contain(expectedClass);
    }

    [Fact]
    public void Render_WithAppearance_Outlined_AddsOutlinedClass() {
        // Arrange & Act
        var cut = Render<TestIcon>(p => p.Add(c => c.Appearance, IconAppearance.Outlined).Add(c => c.Icon, "mail"));

        // Assert
        cut.Find("span").GetAttribute("class")!.Should().Contain("test-outlined");
    }

    [Fact]
    public void Render_WithAppearance_Round_AddsRoundClass() {
        // Arrange & Act
        var cut = Render<TestIcon>(p => p.Add(c => c.Appearance, IconAppearance.Round).Add(c => c.Icon, "person"));

        // Assert
        cut.Find("span").GetAttribute("class")!.Should().Contain("test-round");
    }

    [Fact]
    public void Render_WithAppearance_Sharp_AddsSharpClass() {
        // Arrange & Act
        var cut = Render<TestIcon>(p => p.Add(c => c.Appearance, IconAppearance.Sharp).Add(c => c.Icon, "location"));

        // Assert
        cut.Find("span").GetAttribute("class")!.Should().Contain("test-sharp");
    }

    [Fact]
    public void Render_WithColor_AddsColorVariable() {
        // Arrange & Act
        var cut = Render<TestIcon>(p => p.Add(c => c.Color, TnTColor.Primary).Add(c => c.Icon, "heart"));

        // Assert
        cut.Find("span").GetAttribute("style")!.Should().Contain("--test-icon-color:var(--tnt-color-primary)");
    }

    [Fact]
    public void Render_WithDefaultColor_DoesNotAddColorVariable() {
        // Arrange & Act
        var cut = Render<TestIcon>(p => p.Add(c => c.Color, TnTColor.OnSurface).Add(c => c.Icon, "menu"));

        // Assert
        var style = cut.Find("span").GetAttribute("style");
        if (style != null) {
            style.Should().NotContain("--test-icon-color");
        }
    }

    [Fact]
    public void Render_WithDefaultParameters_RendersSpanWithDefaultClasses() {
        // Arrange
        var icon = new TestIcon("home");

        // Act
        var cut = Render(icon.Render());
        var root = cut.Find("span.test-icon");

        // Assert
        root.Should().NotBeNull();
        root.GetAttribute("class")!.Should().Contain("test-icon");
        root.GetAttribute("class")!.Should().Contain("tnt-components");
        root.GetAttribute("class")!.Should().Contain("test-medium"); // default size
        root.TextContent.Should().Be("home");
    }

    [Fact]
    public void Render_WithElementId_SetsIdAttribute() {
        // Arrange & Act
        var cut = Render<TestIcon>(p => p.Add(c => c.ElementId, "my-icon").Add(c => c.Icon, "check"));

        // Assert
        cut.Find("span").GetAttribute("id")!.Should().Be("my-icon");
    }

    [Fact]
    public void Render_WithElementTitle_SetsTitleAttribute() {
        // Arrange & Act
        var cut = Render<TestIcon>(p => p.Add(c => c.ElementTitle, "Custom tooltip").Add(c => c.Icon, "info"));

        // Assert
        cut.Find("span").GetAttribute("title")!.Should().Be("Custom tooltip");
    }

    [Fact]
    public void Render_WithIcon_RendersIconContent() {
        // Arrange
        var icon = new TestIcon("search");

        // Act
        var cut = Render(icon.Render());

        // Assert
        cut.Find("span").TextContent.Should().Be("search");
    }

    [Fact]
    public void Render_WithoutElementTitle_UsesIconAsTitle() {
        // Arrange & Act
        var cut = Render<TestIcon>(p => p.Add(c => c.Icon, "warning"));

        // Assert
        cut.Find("span").GetAttribute("title")!.Should().Be("warning");
    }

    [Fact]
    public void Render_WithSize_ExtraLarge_AddsExtraLargeClass() {
        // Arrange & Act
        var cut = Render<TestIcon>(p => p.Add(c => c.Size, IconSize.ExtraLarge).Add(c => c.Icon, "settings"));

        // Assert
        cut.Find("span").GetAttribute("class")!.Should().Contain("test-extra-large");
    }

    [Fact]
    public void Render_WithSize_Large_AddsLargeClass() {
        // Arrange & Act
        var cut = Render<TestIcon>(p => p.Add(c => c.Size, IconSize.Large).Add(c => c.Icon, "favorite"));

        // Assert
        cut.Find("span").GetAttribute("class")!.Should().Contain("test-large");
    }

    [Fact]
    public void Render_WithSize_Small_AddsSmallClass() {
        // Arrange & Act
        var cut = Render<TestIcon>(p => p.Add(c => c.Size, IconSize.Small).Add(c => c.Icon, "star"));

        // Assert
        cut.Find("span").GetAttribute("class")!.Should().Contain("test-small");
    }
}