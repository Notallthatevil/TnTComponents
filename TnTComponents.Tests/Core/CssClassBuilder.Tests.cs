﻿using TnTComponents.Core;

namespace TnTComponents.Tests.Core;

/// <summary>
///     Unit tests for <see cref="CssClassBuilder" />.
/// </summary>
public class CssClassBuilder_Tests {

    [Fact]
    public void AddBackgroundColor_WithMultipleColors_AddsAllColors() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder
            .AddBackgroundColor(TnTColor.Primary)
            .AddBackgroundColor(TnTColor.Secondary)
            .Build();

        // Assert
        result.Should().Be("tnt-bg-color-primary tnt-bg-color-secondary");
    }

    [Fact]
    public void AddBackgroundColor_WithNoneColor_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");
        var color = TnTColor.None;

        // Act
        var result = builder.AddBackgroundColor(color).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddBackgroundColor_WithNullColor_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddBackgroundColor(null).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddBackgroundColor_WithValidColor_AddsBackgroundClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");
        var color = TnTColor.Primary;

        // Act
        var result = builder.AddBackgroundColor(color).Build();

        // Assert
        result.Should().Be("tnt-bg-color-primary");
    }

    [Fact]
    public void AddClass_WithConditionFalse_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddClass("conditional-class", false).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddClass_WithConditionNull_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddClass("conditional-class", null).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddClass_WithConditionTrue_AddsClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddClass("conditional-class", true).Build();

        // Assert
        result.Should().Be("conditional-class");
    }

    [Fact]
    public void AddClass_WithDuplicateClasses_AddsSingleInstance() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder
            .AddClass("duplicate-class")
            .AddClass("duplicate-class")
            .AddClass("unique-class")
            .Build();

        // Assert
        result.Should().Be("duplicate-class unique-class");
    }

    [Fact]
    public void AddClass_WithEmptyClassName_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddClass("").Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddClass_WithMultipleClasses_AddsAllClasses() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder
            .AddClass("class-1")
            .AddClass("class-2")
            .AddClass("class-3")
            .Build();

        // Assert
        result.Should().Be("class-1 class-2 class-3");
    }

    [Fact]
    public void AddClass_WithNullClassName_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddClass(null).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddClass_WithValidClassName_AddsClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddClass("test-class").Build();

        // Assert
        result.Should().Be("test-class");
    }

    [Fact]
    public void AddClass_WithWhitespaceClassName_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddClass("   ").Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddDisabled_WithFalse_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddDisabled(false).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddDisabled_WithTrue_AddsDisabledClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddDisabled(true).Build();

        // Assert
        result.Should().Be("tnt-disabled");
    }

    [Fact]
    public void AddElevation_WithElevationAboveMax_ClampsToTen() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddElevation(15).Build();

        // Assert
        result.Should().Be("tnt-elevation-10");
    }

    [Fact]
    public void AddElevation_WithExactMaxElevation_AddsCorrectClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddElevation(10).Build();

        // Assert
        result.Should().Be("tnt-elevation-10");
    }

    [Fact]
    public void AddElevation_WithNegativeElevation_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddElevation(-5).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddElevation_WithValidElevation_AddsElevationClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddElevation(5).Build();

        // Assert
        result.Should().Be("tnt-elevation-5");
    }

    [Fact]
    public void AddElevation_WithZeroElevation_AddsElevationClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddElevation(0).Build();

        // Assert
        result.Should().Be("tnt-elevation-0");
    }

    [Fact]
    public void AddFilled_WithDefaultParameter_AddsFilledClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddFilled().Build();

        // Assert
        result.Should().Be("tnt-filled");
    }

    [Fact]
    public void AddFilled_WithFalse_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddFilled(false).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddFilled_WithTrue_AddsFilledClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddFilled(true).Build();

        // Assert
        result.Should().Be("tnt-filled");
    }

    [Fact]
    public void AddForegroundColor_WithNoneColor_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");
        var color = TnTColor.None;

        // Act
        var result = builder.AddForegroundColor(color).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddForegroundColor_WithNullColor_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddForegroundColor(null).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddForegroundColor_WithValidColor_AddsForegroundClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");
        var color = TnTColor.OnPrimary;

        // Act
        var result = builder.AddForegroundColor(color).Build();

        // Assert
        result.Should().Be("tnt-fg-color-on-primary");
    }

    [Fact]
    public void AddFromAdditionalAttributes_WithClassAttribute_AddsClasses() {
        // Arrange
        var builder = CssClassBuilder.Create("");
        var attributes = new Dictionary<string, object> { ["class"] = "custom-class" };

        // Act
        var result = builder.AddFromAdditionalAttributes(attributes).Build();

        // Assert
        result.Should().Be("custom-class");
    }

    [Fact]
    public void AddFromAdditionalAttributes_WithMultipleClassesInAttribute_AddsAllClasses() {
        // Arrange
        var builder = CssClassBuilder.Create("");
        var attributes = new Dictionary<string, object> { ["class"] = "class-1 class-2 class-3" };

        // Act
        var result = builder.AddFromAdditionalAttributes(attributes).Build();

        // Assert
        result.Should().Be("class-1 class-2 class-3");
    }

    [Fact]
    public void AddFromAdditionalAttributes_WithNonStringClassValue_ConvertsToString() {
        // Arrange
        var builder = CssClassBuilder.Create("");
        var attributes = new Dictionary<string, object> { ["class"] = 123 };

        // Act
        var result = builder.AddFromAdditionalAttributes(attributes).Build();

        // Assert
        result.Should().Be("123");
    }

    [Fact]
    public void AddFromAdditionalAttributes_WithNullAttributes_DoesNotAddClasses() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddFromAdditionalAttributes(null).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddFromAdditionalAttributes_WithNullClassValue_DoesNotAddClasses() {
        // Arrange
        var builder = CssClassBuilder.Create("");
        var attributes = new Dictionary<string, object> { ["class"] = null! };

        // Act
        var result = builder.AddFromAdditionalAttributes(attributes).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddFromAdditionalAttributes_WithoutClassKey_DoesNotAddClasses() {
        // Arrange
        var builder = CssClassBuilder.Create("");
        var attributes = new Dictionary<string, object> { ["id"] = "test-id" };

        // Act
        var result = builder.AddFromAdditionalAttributes(attributes).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddOnTintColor_WithNoneColor_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");
        var color = TnTColor.None;

        // Act
        var result = builder.AddOnTintColor(color).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddOnTintColor_WithNullColor_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddOnTintColor(null).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddOnTintColor_WithValidColor_AddsOnTintClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");
        var color = TnTColor.OnTertiary;

        // Act
        var result = builder.AddOnTintColor(color).Build();

        // Assert
        result.Should().Be("tnt-on-tint-color-on-tertiary");
    }

    [Fact]
    public void AddRipple_WithDefaultParameter_AddsRippleClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddRipple().Build();

        // Assert
        result.Should().Be("tnt-ripple");
    }

    [Fact]
    public void AddRipple_WithFalse_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddRipple(false).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddRipple_WithTrue_AddsRippleClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddRipple(true).Build();

        // Assert
        result.Should().Be("tnt-ripple");
    }

    [Fact]
    public void AddSize_WithLargeSize_AddsSizeClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddSize(Size.Large).Build();

        // Assert
        result.Should().Be("tnt-size-l");
    }

    [Fact]
    public void AddSize_WithLargestSize_AddsSizeClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddSize(Size.Largest).Build();

        // Assert
        result.Should().Be("tnt-size-xl");
    }

    [Fact]
    public void AddSize_WithMediumSize_AddsSizeClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddSize(Size.Medium).Build();

        // Assert
        result.Should().Be("tnt-size-m");
    }

    [Fact]
    public void AddSize_WithNullSize_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddSize(null).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddSize_WithSmallestSize_AddsSizeClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddSize(Size.Smallest).Build();

        // Assert
        result.Should().Be("tnt-size-xs");
    }

    [Fact]
    public void AddSize_WithSmallSize_AddsSizeClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddSize(Size.Small).Build();

        // Assert
        result.Should().Be("tnt-size-s");
    }

    [Fact]
    public void AddTextAlign_WithCenterAlignment_AddsTextAlignClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddTextAlign(TextAlign.Center).Build();

        // Assert
        result.Should().Be("tnt-text-align-center");
    }

    [Fact]
    public void AddTextAlign_WithJustifyAlignment_AddsTextAlignClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddTextAlign(TextAlign.Justify).Build();

        // Assert
        result.Should().Be("tnt-text-align-justify");
    }

    [Fact]
    public void AddTextAlign_WithLeftAlignment_AddsTextAlignClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddTextAlign(TextAlign.Left).Build();

        // Assert
        result.Should().Be("tnt-text-align-left");
    }

    [Fact]
    public void AddTextAlign_WithNullAlignment_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddTextAlign(null).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddTextAlign_WithRightAlignment_AddsTextAlignClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddTextAlign(TextAlign.Right).Build();

        // Assert
        result.Should().Be("tnt-text-align-right");
    }

    [Fact]
    public void AddTintColor_WithNoneColor_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");
        var color = TnTColor.None;

        // Act
        var result = builder.AddTintColor(color).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddTintColor_WithNullColor_DoesNotAddClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");

        // Act
        var result = builder.AddTintColor(null).Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void AddTintColor_WithValidColor_AddsTintClass() {
        // Arrange
        var builder = CssClassBuilder.Create("");
        var color = TnTColor.Tertiary;

        // Act
        var result = builder.AddTintColor(color).Build();

        // Assert
        result.Should().Be("tnt-tint-color-tertiary");
    }

    [Fact]
    public void Builder_ChainedCalls_ReturnsBuilderInstance() {
        // Arrange & Act
        var builder = CssClassBuilder.Create("");
        var chainedBuilder = builder
            .AddClass("test")
            .AddBackgroundColor(TnTColor.Primary)
            .AddSize(Size.Medium);

        // Assert
        builder.Should().BeSameAs(chainedBuilder);
    }

    [Fact]
    public void Builder_WithConditionalClasses_AddsOnlyWhenConditionsMet() {
        // Arrange
        bool isEnabled = true;
        bool isHighlighted = false;

        // Act
        var result = CssClassBuilder.Create("base")
            .AddClass("enabled-class", isEnabled)
            .AddClass("highlighted-class", isHighlighted)
            .AddDisabled(!isEnabled)
            .Build();

        // Assert
        result.Should().Contain("base");
        result.Should().Contain("enabled-class");
        result.Should().NotContain("highlighted-class");
        result.Should().NotContain("tnt-disabled");
    }

    [Fact]
    public void Builder_WithDuplicateClassesFromDifferentMethods_DeduplicatesClasses() {
        // Arrange
        var attributes = new Dictionary<string, object> { ["class"] = "duplicate-class" };

        // Act
        var result = CssClassBuilder.Create("")
            .AddClass("duplicate-class")
            .AddFromAdditionalAttributes(attributes)
            .Build();

        // Assert
        result.Should().Be("duplicate-class");
    }

    [Fact]
    public void Builder_WithEmptyBuild_ReturnsEmptyString() {
        // Arrange
        var builder = CssClassBuilder.Create("")
            .AddClass(null)
            .AddClass("", false)
            .AddBackgroundColor(null)
            .AddSize(null);

        // Act
        var result = builder.Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Builder_WithMultipleMethodCalls_CombinesAllClasses() {
        // Arrange & Act
        var result = CssClassBuilder.Create("base-class")
            .AddClass("additional-class")
            .AddBackgroundColor(TnTColor.Primary)
            .AddForegroundColor(TnTColor.OnPrimary)
            .AddSize(Size.Large)
            .AddElevation(3)
            .AddDisabled(false)
            .AddFilled()
            .AddRipple(true)
            .AddTextAlign(TextAlign.Center)
            .Build();

        // Assert
        result.Should().Contain("base-class");
        result.Should().Contain("additional-class");
        result.Should().Contain("tnt-bg-color-primary");
        result.Should().Contain("tnt-fg-color-on-primary");
        result.Should().Contain("tnt-size-l");
        result.Should().Contain("tnt-elevation-3");
        result.Should().Contain("tnt-filled");
        result.Should().Contain("tnt-ripple");
        result.Should().Contain("tnt-text-align-center");
        result.Should().NotContain("tnt-disabled");
    }

    [Fact]
    public void Create_WithCustomClass_ReturnsBuilderWithCustomClass() {
        // Arrange & Act
        var builder = CssClassBuilder.Create("custom-class");
        var result = builder.Build();

        // Assert
        result.Should().Be("custom-class");
    }

    [Fact]
    public void Create_WithDefaultParameter_ReturnsBuilderWithDefaultClass() {
        // Arrange & Act
        var builder = CssClassBuilder.Create();
        var result = builder.Build();

        // Assert
        result.Should().Be("tnt-components");
    }

    [Fact]
    public void Create_WithEmptyString_ReturnsBuilderWithoutClass() {
        // Arrange & Act
        var builder = CssClassBuilder.Create("");
        var result = builder.Build();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithWhitespaceString_ReturnsBuilderWithoutClass() {
        // Arrange & Act
        var builder = CssClassBuilder.Create("   ");
        var result = builder.Build();

        // Assert
        result.Should().BeEmpty();
    }
}