using Bunit;
using TnTComponents;
using Xunit;

namespace TnTComponents.Tests.Theming;

public class TnTMeasurements_Tests : Bunit.TestContext
{
    [Fact]
    public void Renders_Default_Values()
    {
        // Act
        var cut = RenderComponent<TnTMeasurements>();

        // Assert
        cut.MarkupMatches(@"<style class='tnt-measurements'>
    :root {
        --tnt-header-height: 64px;
        --tnt-footer-height: 64px;
        --tnt-side-nav-width: 256px;
    }
</style>");
    }

    [Fact]
    public void Renders_Custom_Values()
    {
        // Act
        var cut = RenderComponent<TnTMeasurements>(parameters => parameters
            .Add(p => p.HeaderHeight, 80)
            .Add(p => p.FooterHeight, 40)
            .Add(p => p.SideNavWidth, 300)
        );

        // Assert
        cut.MarkupMatches(@"<style class='tnt-measurements'>
    :root {
        --tnt-header-height: 80px;
        --tnt-footer-height: 40px;
        --tnt-side-nav-width: 300px;
    }
</style>");
    }
}
