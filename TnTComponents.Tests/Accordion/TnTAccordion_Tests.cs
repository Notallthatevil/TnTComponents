using Microsoft.AspNetCore.Components;

namespace TnTComponents.Tests.Accordion;
public class TnTAccordionTests : Bunit.TestContext {
    public TnTAccordionTests() {
        // Setup JSInterop for Accordion JS module
        JSInterop.SetupModule("./_content/TnTComponents/Accordion/TnTAccordion.razor.js")
        .SetupVoid();
        // Setup JSInterop for RippleEffect JS module (used by child components)
        JSInterop.SetupModule("./_content/TnTComponents/Core/TnTRippleEffect.razor.js")
        .SetupVoid();
        // Setup JSInterop for onLoad void call (used by TnTAccordion and TnTRippleEffect)
        JSInterop.SetupVoid("onLoad", _ => true);
        Renderer.SetRendererInfo(new RendererInfo("Server", true));
    }

    [Fact]
    public void RendersAccordionWithNoChildren() {
        // Arrange & Act
        var cut = RenderComponent<TnTAccordion>(parameters => parameters
            .Add(p => p.ChildContent, (RenderFragment)(builder => { }))
        );

        // Assert: Should render a tnt-accordion with no h3 children
        var accordion = cut.Find("tnt-accordion");
        Assert.NotNull(accordion);
        Assert.Empty(cut.FindAll("h3"));
    }

    [Fact]
    public void RendersAccordionWithSingleChild() {
        // Arrange & Act
        var cut = RenderComponent<TnTAccordion>(parameters => parameters
            .Add(p => p.ChildContent, (RenderFragment)(builder => {
                builder.OpenComponent<TnTAccordionChild>(0);
                builder.AddAttribute(1, "Label", "Test Child");
                builder.CloseComponent();
            }))
        );

        // Assert: Wait for header to appear and check label
        cut.WaitForAssertion(() => {
            var headers = cut.FindAll("h3");
            Assert.Single(headers);
            Assert.Contains("Test Child", headers[0].TextContent);
        });
    }

    [Fact]
    public void RendersAccordionWithMultipleChildrenInOrder() {
        // Arrange & Act
        var cut = RenderComponent<TnTAccordion>(parameters => parameters
            .Add(p => p.ChildContent, (RenderFragment)(builder => {
                builder.OpenComponent<TnTAccordionChild>(0);
                builder.AddAttribute(1, "Label", "First");
                builder.AddAttribute(2, "Order", 2);
                builder.CloseComponent();
                builder.OpenComponent<TnTAccordionChild>(3);
                builder.AddAttribute(4, "Label", "Second");
                builder.AddAttribute(5, "Order", 1);
                builder.CloseComponent();
            }))
        );

        // Assert
        var headers = cut.FindAll("h3");
        Assert.True(headers.Count == 2);
        Assert.Contains("Second", headers[0].TextContent);
        Assert.Contains("First", headers[1].TextContent);
    }

    [Fact]
    public void AccordionRendersWithCustomAttributes() {
        // Arrange & Act
        var cut = RenderComponent<TnTAccordion>(parameters => parameters
            .AddUnmatched("data-test", "my-accordion")
        );

        // Assert: Should have the custom attribute
        var accordion = cut.Find("tnt-accordion");
        Assert.True(accordion.HasAttribute("data-test"));
        Assert.Equal("my-accordion", accordion.GetAttribute("data-test"));
    }

    [Fact(Skip = "Cannot test JS-handled click events in Bunit. Only Blazor @onclick events are supported.")]
    public void AccordionChildOpensAndClosesOnHeaderClick() {
        // Arrange
        var cut = RenderComponent<TnTAccordion>(parameters => parameters
            .Add(p => p.ChildContent, (RenderFragment)(builder => {
                builder.OpenComponent<TnTAccordionChild>(0);
                builder.AddAttribute(1, "Label", "Openable");
                builder.CloseComponent();
            }))
        );

        var header = cut.Find("h3[role='button']");
        var panel = header.NextElementSibling;
        Assert.NotNull(panel);
        // Initially closed
        Assert.Contains("aria-expanded=\"false\"", header.OuterHtml);
        // Panel is hidden if it has 'hidden' attribute or style (adjust as needed)
        // For now, just check the markup for 'hidden' or empty content
        // Act: Click header to open
        header.Click();
        cut.WaitForAssertion(() => {
            Assert.Contains("aria-expanded=\"true\"", header.OuterHtml);
            // Panel should now be visible (not hidden)
            // Optionally check panel content or attributes
        });

        // Act: Click header to close
        header.Click();
        cut.WaitForAssertion(() => {
            Assert.Contains("aria-expanded=\"false\"", header.OuterHtml);
            // Panel should be hidden again
        });
    }

    [Fact(Skip = "Cannot test JS-handled click events in Bunit. Only Blazor @onclick events are supported.")]
    public void AccordionChildDisabledDoesNotOpenOnClick() {
        // Arrange
        var cut = RenderComponent<TnTAccordion>(parameters => parameters
            .Add(p => p.ChildContent, (RenderFragment)(builder => {
                builder.OpenComponent<TnTAccordionChild>(0);
                builder.AddAttribute(1, "Label", "Disabled");
                builder.AddAttribute(2, "Disabled", true);
                builder.CloseComponent();
            }))
        );

        var header = cut.Find("h3[role='button']");
        var panel = header.NextElementSibling;
        Assert.NotNull(panel);
        // Initially closed
        Assert.Contains("aria-expanded=\"false\"", header.OuterHtml);
        // Act: Click header
        header.Click();
        // Should remain closed
        cut.WaitForAssertion(() => {
            Assert.Contains("aria-expanded=\"false\"", header.OuterHtml);
        });
    }

    [Fact]
    public void AccordionHandlesDynamicChildren() {
        // Arrange
        var children = new List<(string label, int order)>
        {
                ("A", 1),
                ("B", 2)
            };
        var cut = RenderComponent<TnTAccordion>(parameters => parameters
            .Add(p => p.ChildContent, (RenderFragment)(builder => {
                int seq = 0;
                foreach (var child in children) {
                    builder.OpenComponent<TnTAccordionChild>(seq++);
                    builder.AddAttribute(seq++, "Label", child.label);
                    builder.AddAttribute(seq++, "Order", child.order);
                    builder.CloseComponent();
                }
            }))
        );

        // Assert initial
        var headers = cut.FindAll("h3");
        Assert.Equal(2, headers.Count);
        Assert.Contains("A", headers[0].TextContent);
        Assert.Contains("B", headers[1].TextContent);

        // Act: Remove first child and re-render
        children.RemoveAt(0);
        cut.Render();
        headers = cut.FindAll("h3");
        Assert.Single(headers);
        Assert.Contains("B", headers[0].TextContent);
    }

    [Fact]
    public void AccordionChildHasCorrectAccessibilityAttributes() {
        // Arrange
        var cut = RenderComponent<TnTAccordion>(parameters => parameters
            .Add(p => p.ChildContent, (RenderFragment)(builder => {
                builder.OpenComponent<TnTAccordionChild>(0);
                builder.AddAttribute(1, "Label", "Accessible");
                builder.CloseComponent();
            }))
        );

        var header = cut.Find("h3[role='button']");
        var panel = header.NextElementSibling;
        Assert.NotNull(panel);
        Assert.True(header.HasAttribute("aria-controls"));
        Assert.True(header.HasAttribute("aria-expanded"));
        Assert.True(panel.HasAttribute("id"));
        Assert.True(panel.HasAttribute("role"));
        Assert.Equal("region", panel.GetAttribute("role"));
    }

    [Fact(Skip = "Cannot test JS-handled click events in Bunit. Only Blazor @onclick events are supported.")]
    public void AccordionChildOnOpenAndOnCloseCallbacksAreInvoked() {
        // Arrange
        bool opened = false, closed = false;
        var cut = RenderComponent<TnTAccordion>(parameters => parameters
            .Add(p => p.ChildContent, (RenderFragment)(builder => {
                builder.OpenComponent<TnTAccordionChild>(0);
                builder.AddAttribute(1, "Label", "CallbackTest");
                builder.AddAttribute(2, "OnOpen", EventCallback.Factory.Create(this, () => opened = true));
                builder.AddAttribute(3, "OnClose", EventCallback.Factory.Create(this, () => closed = true));
                builder.CloseComponent();
            }))
        );

        var header = cut.Find("h3[role='button']");
        // Act: Open
        header.Click();
        cut.WaitForAssertion(() => Assert.True(opened));
        // Act: Close
        header.Click();
        cut.WaitForAssertion(() => Assert.True(closed));
    }
}
