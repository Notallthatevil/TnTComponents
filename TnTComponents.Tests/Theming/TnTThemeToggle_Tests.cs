using Bunit;
using TnTComponents;
using Xunit;

namespace TnTComponents.Tests.Theming;
public class TnTThemeToggle_Tests : Bunit.TestContext
{
    private const string ThemeToggleJsModule = "./_content/TnTComponents/Theming/TnTThemeToggle.razor.js?v=2";

    public TnTThemeToggle_Tests()
    {
        var module = JSInterop.SetupModule(ThemeToggleJsModule);
        module.SetupVoid("onLoad", _ => true);
    }
    [Fact]
    public void Renders_With_Default_Parameters()
    {
        // Act
        var cut = RenderComponent<TnTThemeToggle>();

        // Assert
        var themeToggle = cut.Find("tnt-theme-toggle");
        Assert.Equal("/Themes", themeToggle.GetAttribute("tnt-themes-root"));
        Assert.Equal("light.css", themeToggle.GetAttribute("tnt-light-default"));
        Assert.Equal("dark.css", themeToggle.GetAttribute("tnt-dark-default"));
        var options = cut.FindAll("select.tnt-theme-select option");
        Assert.Contains(options, o => o.TextContent == "Light - Default");
        Assert.Contains(options, o => o.TextContent == "Dark - Default");
        Assert.Contains(options, o => o.TextContent == "System - Default");
    }

    [Fact]
    public void Renders_Theme_Only_Options_When_Contrast_Disabled()
    {
        // Act
        var cut = RenderComponent<TnTThemeToggle>(parameters => parameters
            .Add(p => p.AllowThemeSelection, true)
            .Add(p => p.AllowContrastSelection, false)
        );

        // Assert
        var options = cut.FindAll("select.tnt-theme-select option");
        Assert.Contains(options, o => o.TextContent == "Light");
        Assert.Contains(options, o => o.TextContent == "Dark");
        Assert.Contains(options, o => o.TextContent == "System");
    }

    [Fact]
    public void Renders_Contrast_Only_Options_When_Theme_Disabled()
    {
        // Act
        var cut = RenderComponent<TnTThemeToggle>(parameters => parameters
            .Add(p => p.AllowThemeSelection, false)
            .Add(p => p.AllowContrastSelection, true)
        );

        // Assert
        var options = cut.FindAll("select.tnt-theme-select option");
        Assert.Contains(options, o => o.TextContent == "Default");
        Assert.Contains(options, o => o.TextContent == "Medium");
        Assert.Contains(options, o => o.TextContent == "High");
    }

    [Fact]
    public void Does_Not_Render_When_Hide_Is_True()
    {
        // Act
        var cut = RenderComponent<TnTThemeToggle>(parameters => parameters.Add(p => p.Hide, true));

        // Assert
        var themeToggle = cut.Find("tnt-theme-toggle");
        Assert.Contains("display:none", themeToggle.GetAttribute("style"));
    }
}
