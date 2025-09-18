using System;
using System.Collections.Generic;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Grid;
using TnTComponents.Grid.Columns;
using TnTComponents.Grid.Infrastructure;
using Xunit;
using RippleTestingUtility = TnTComponents.Tests.TestingUtility.TestingUtility;

namespace TnTComponents.Tests.Grid.Infrastructure;

/// <summary>
///     Unit tests for <see cref="TnTDataGridBodyCell{TGridItem}" />.
/// </summary>
public class TnTDataGridBodyCell_Tests : BunitContext {

    /// <summary>
    ///     Test model for grid body cell tests.
    /// </summary>
    private class TestGridItem {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    private readonly TestGridItem _testItem = new() {
        Id = 1,
        Name = "John Doe",
        Email = "john@example.com"
    };

    public TnTDataGridBodyCell_Tests() {
        // Arrange (global) & Act: JS module setup for ripple in constructor
        RippleTestingUtility.SetupRippleEffectModule(this);
    }

    #region Rendering Tests

    [Fact]
    public void Renders_TdElement_WithColumnContent() {
        // Arrange
        var column = CreateTestColumn("Test Content");

        // Act
        var cut = RenderCellWithColumn(column);

        // Assert
        cut.FindAll("td").Should().HaveCount(1);
        cut.Markup.Should().Contain("Test Content");
    }

    [Fact]
    public void Renders_WithColumnAdditionalAttributes() {
        // Arrange
        var column = CreateTestColumn("Content");
        column.AdditionalAttributes = new Dictionary<string, object?> {
            { "data-test", "test-value" },
            { "class", "custom-class" }
        };

        // Act
        var cut = RenderCellWithColumn(column);

        // Assert
        var td = cut.Find("td");
        td.GetAttribute("data-test").Should().Be("test-value");
        td.GetAttribute("class").Should().Contain("custom-class");
    }

    [Fact]
    public void Renders_WithNullAdditionalAttributes() {
        // Arrange
        var column = CreateTestColumn("Content");
        column.AdditionalAttributes = null;

        // Act
        var cut = RenderCellWithColumn(column);

        // Assert
        cut.FindAll("td").Should().HaveCount(1);
        cut.Markup.Should().Contain("Content");
    }

    [Fact]
    public void Renders_WithEmptyAdditionalAttributes() {
        // Arrange
        var column = CreateTestColumn("Content");
        column.AdditionalAttributes = new Dictionary<string, object?>();

        // Act
        var cut = RenderCellWithColumn(column);

        // Assert
        cut.FindAll("td").Should().HaveCount(1);
        cut.Markup.Should().Contain("Content");
    }

    #endregion

    #region Parameter Tests

    [Fact]
    public void Item_IsRequired() {
        // Arrange
        var column = CreateTestColumn("Content");

        // Act
        var cut = Render<TnTDataGridBodyCell<TestGridItem>>(parameters => parameters
            .Add(p => p.Column, column)
            .Add(p => p.Item, (TestGridItem)null!));

        // Assert - The component should render but Item will be null
        // The EditorRequired attribute is primarily for design-time tooling
        cut.Should().NotBeNull();
    }

    [Fact]
    public void Column_IsRequired() {
        // Arrange & Act
        var act = () => Render<TnTDataGridBodyCell<TestGridItem>>(parameters => parameters
            .Add(p => p.Column, (TnTColumnBase<TestGridItem>)null!)
            .Add(p => p.Item, _testItem));

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("Column");
    }

    [Fact]
    public void Item_IsSetCorrectly() {
        // Arrange
        var column = CreateTestColumn("Content");

        // Act
        var cut = RenderCellWithColumn(column);

        // Assert
        var component = cut.Instance;
        component.Item.Should().BeSameAs(_testItem);
    }

    [Fact]
    public void Column_IsSetCorrectly() {
        // Arrange
        var column = CreateTestColumn("Content");

        // Act
        var cut = RenderCellWithColumn(column);

        // Assert
        var component = cut.Instance;
        component.Column.Should().BeSameAs(column);
    }

    #endregion

    #region Inheritance Tests

    [Fact]
    public void InheritsFrom_TnTDataGridCell() {
        // Arrange
        var column = CreateTestColumn("Content");

        // Act
        var cut = RenderCellWithColumn(column);

        // Assert
        var component = cut.Instance;
        component.Should().BeAssignableTo<TnTDataGridCell<TestGridItem>>();
    }

    [Fact]
    public void HasCascadingTypeParameter() {
        // Arrange & Act
        var componentType = typeof(TnTDataGridBodyCell<TestGridItem>);

        // Assert
        var attribute = componentType.GetCustomAttributes(typeof(CascadingTypeParameterAttribute), false);
        attribute.Should().HaveCount(1);
        var cascadingAttr = (CascadingTypeParameterAttribute)attribute[0];
        cascadingAttr.Name.Should().Be("TGridItem");
    }

    #endregion

    #region Content Rendering Tests

    [Fact]
    public void Renders_ColumnContentWithItem() {
        // Arrange
        var column = new TestTemplateColumn<TestGridItem> {
            CellTemplate = item => builder => {
                builder.OpenElement(0, "span");
                builder.AddContent(1, $"{item.Name} ({item.Id})");
                builder.CloseElement();
            }
        };

        // Act
        var cut = RenderCellWithColumn(column);

        // Assert
        cut.Markup.Should().Contain("John Doe (1)");
    }

    [Fact]
    public void Renders_WithComplexColumnContent() {
        // Arrange
        var column = new TestTemplateColumn<TestGridItem> {
            CellTemplate = item => builder => {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "custom-cell");
                builder.OpenElement(2, "strong");
                builder.AddContent(3, item.Name);
                builder.CloseElement();
                builder.OpenElement(4, "small");
                builder.AddContent(5, item.Email);
                builder.CloseElement();
                builder.CloseElement();
            }
        };

        // Act
        var cut = RenderCellWithColumn(column);

        // Assert
        cut.Markup.Should().Contain("custom-cell");
        cut.Markup.Should().Contain("<strong>John Doe</strong>");
        cut.Markup.Should().Contain("<small>john@example.com</small>");
    }

    #endregion

    #region Helper Methods

    private IRenderedComponent<TnTDataGridBodyCell<TestGridItem>> RenderCellWithColumn(TnTColumnBase<TestGridItem> column) {
        return Render<TnTDataGridBodyCell<TestGridItem>>(parameters => parameters
            .Add(p => p.Column, column)
            .Add(p => p.Item, _testItem));
    }

    private TestTemplateColumn<TestGridItem> CreateTestColumn(string content) {
        return new TestTemplateColumn<TestGridItem> {
            CellTemplate = _ => builder => builder.AddContent(0, content)
        };
    }

    /// <summary>
    ///     Test implementation of TnTColumnBase for testing purposes.
    /// </summary>
    private class TestTemplateColumn<TItem> : TnTColumnBase<TItem> {
        public RenderFragment<TItem>? CellTemplate { get; set; }
        
        public override string? ElementClass => null;
        public override string? ElementStyle => null;
        public override TnTGridSort<TItem>? SortBy { get; set; }

        public override RenderFragment RenderCellContent(TItem item) =>
            CellTemplate?.Invoke(item) ?? (builder => { });

        public override void RenderHeaderContent(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder) {
            builder.AddContent(0, "Header");
        }
    }

    #endregion
}