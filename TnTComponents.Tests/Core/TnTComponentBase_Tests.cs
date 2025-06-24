using System.Collections.Generic;
using Xunit;
using Bunit;
using TnTComponents.Core;

namespace TnTComponents.Tests.Core;

public class TnTComponentBase_Tests : Bunit.TestContext
{

    [Fact]
    public void OnParametersSet_ShouldAddTnTId_WhenMissing()
    {
        var cut = RenderComponent<TestComponent>(parameters => parameters
            .AddUnmatched("foo", "bar")
        );

        var div = cut.Find("div");
        div.HasAttribute("tntid").Should().BeTrue();
        div.GetAttribute("tntid").Should().Be(cut.Instance.ComponentIdentifier);
        div.HasAttribute("foo").Should().BeTrue();
        div.GetAttribute("foo").Should().Be("bar");
    }

    [Fact]
    public void OnParametersSet_ShouldSetTnTId_WhenAdditionalAttributesIsNull()
    {
        var cut = RenderComponent<TestComponent>();
        var div = cut.Find("div");
        div.HasAttribute("tntid").Should().BeTrue();
        div.GetAttribute("tntid").Should().Be(cut.Instance.ComponentIdentifier);
    }


    [Fact]
    public void OnParametersSet_ShouldUpdateTnTId_WhenIncorrect()
    {
        var cut = RenderComponent<TestComponent>(parameters => parameters
            .AddUnmatched("tntid", "wrong")
        );
        cut.Render();

        var div = cut.Find("div");
        div.HasAttribute("tntid").Should().BeTrue();
        div.GetAttribute("tntid").Should().Be(cut.Instance.ComponentIdentifier);
    }
}

// Minimal test double for TnTComponentBase
public class TestComponent : TnTComponentBase
{
    public override string ElementClass => string.Empty;
    public override string ElementStyle => string.Empty;

    protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "div");
        if (AdditionalAttributes != null)
        {
            foreach (var kvp in AdditionalAttributes)
            {
                builder.AddAttribute(1, kvp.Key, kvp.Value);
            }
        }
        builder.CloseElement();
    }
}