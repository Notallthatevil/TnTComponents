using System.Collections.Generic;
using System.Linq;
using AwesomeAssertions;
using Microsoft.AspNetCore.Components;

namespace NTComponents.Tests.Buttons.NTButtonGroup;

/// <summary>
///     Tests the <c>HandleItemClickAsync</c> behavior for the button group.
/// </summary>
public sealed class HandleItemClick_Tests : NTButtonGroupTestContext {
    /// <summary>
    ///     Validates that clicking an unselected button updates the selection and raises the selection changed event.
    /// </summary>
    [Fact]
    public void GivenUnselectedItem_WhenClicked_RaisesSelectionChangedAndMarksButton() {
        // Arrange
        var items = CreateItems();
        var recordedKeys = new List<string?>();
        var cut = Render<NTButtonGroup<string>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.SelectedKeyChanged, EventCallback.Factory.Create<string?>(this, key => recordedKeys.Add(key))));
        var buttonElements = cut.FindAll("button.btn-group-btn");

        // Act
        buttonElements[1].Click();
        var updatedButtons = cut.FindAll("button.btn-group-btn");

        // Assert
        recordedKeys.Should().Equal(items.Last().Key);
        updatedButtons[1].ClassList.Should().Contain("btn-group-selected");
        updatedButtons[0].ClassList.Should().NotContain("btn-group-selected");
    }

    /// <summary>
    ///     Validates that clicking the already selected button does not clear selection nor fire the event.
    /// </summary>
    [Fact]
    public async Task GivenSelectedItem_WhenClickedAgain_DoesNotClearSelection() {
        // Arrange
        var items = CreateItems();
        var recordedKeys = new List<string?>();
        var cut = Render<NTButtonGroup<string>>(parameters => parameters
            .Add(p => p.Items, items)
            .Add(p => p.SelectedKey, items.First().Key)
            .Add(p => p.SelectedKeyChanged, EventCallback.Factory.Create<string?>(this, key => recordedKeys.Add(key))));
        var buttonElements = cut.FindAll("button.btn-group-btn");

        // Act
        await buttonElements[0].ClickAsync();
        buttonElements = cut.FindAll("button.btn-group-btn");
        await buttonElements[0].ClickAsync();
        var updatedButtons = cut.FindAll("button.btn-group-btn");

        // Assert
        recordedKeys.Should().BeEmpty();
        updatedButtons[0].ClassList.Should().Contain("btn-group-selected");
    }
}
