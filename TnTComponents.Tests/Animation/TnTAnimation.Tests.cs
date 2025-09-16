using System;
using System.Collections.Generic;
using System.Linq;
using Bunit;
using Xunit;
using TnTComponents;

namespace TnTComponents.Tests.AnimationTests; // distinct namespace

public class TnTAnimation_Tests : BunitContext {
    private const string JsModulePath = "./_content/TnTComponents/Animation/TnTAnimation.razor.js";

    public TnTAnimation_Tests() {
        // Arrange (module expectations for all tests)
        var module = JSInterop.SetupModule(JsModulePath);
        module.SetupVoid("onLoad", _ => true).SetVoidResult();
        module.SetupVoid("onUpdate", _ => true).SetVoidResult();
    }

    [Fact]
    public void Renders_Base_Class() {
        // Arrange / Act
        var cut = Render<TnTAnimation>(p => p.AddChildContent("Content"));
        // Assert
        cut.Markup.Should().Contain("tnt-animation");
    }

    [Fact]
    public void Renders_Default_Animation_Class_FadeIn() {
        // Arrange / Act
        var cut = Render<TnTAnimation>(p => p.AddChildContent("Content"));
        // Assert
        cut.Markup.Should().Contain("tnt-animation-fadein");
    }

    public static IEnumerable<object[]> AllAnimationTypes => System.Enum
        .GetValues(typeof(global::TnTComponents.Animation))
        .Cast<global::TnTComponents.Animation>()
        .Select(v => new object[] { v });

    [Theory]
    [MemberData(nameof(AllAnimationTypes))]
    public void Adds_Correct_Animation_Class(global::TnTComponents.Animation animation) {
        // Arrange / Act
        var cut = Render<TnTAnimation>((ComponentParameterCollectionBuilder<TnTAnimation> p) => p
            .Add(a => a.AnimationType, animation)
            .AddChildContent("Content"));
        // Assert
        cut.Markup.Should().Contain($"tnt-animation-{animation.ToString().ToLower()}");
    }

    [Fact]
    public void ChildContent_Renders() {
        // Arrange / Act
        var cut = Render<TnTAnimation>(p => p.AddChildContent("<span>Animated</span>"));
        // Assert
        cut.Markup.Should().Contain("Animated");
    }

    [Fact]
    public void Duration_Default_In_Style() {
        // Arrange / Act
        var cut = Render<TnTAnimation>(p => p.AddChildContent("Content"));
        // Assert
        var animationStyle = cut.Find("tnt-animation").GetAttribute("style");
        animationStyle.Should().NotBeNull();
        animationStyle!.Replace(" ", string.Empty).Should().Contain("animation-duration:500ms");
    }

    [Fact]
    public void Duration_Custom_In_Style() {
        // Arrange / Act
        var cut = Render<TnTAnimation>((ComponentParameterCollectionBuilder<TnTAnimation> p) => p
            .Add(a => a.Duration, 1500)
            .AddChildContent("Content"));
        // Assert
        var animationStyle = cut.Find("tnt-animation").GetAttribute("style");
        animationStyle.Should().NotBeNull();
        animationStyle!.Replace(" ", string.Empty).Should().Contain("animation-duration:1500ms");
    }

    [Fact]
    public void Threshold_Default_Attribute() {
        // Arrange / Act
        var cut = Render<TnTAnimation>(p => p.AddChildContent("Content"));
        // Assert
        cut.Find("tnt-animation").GetAttribute("tnt-threshold").Should().Be("0.5");
    }

    [Fact]
    public void Threshold_Custom_Attribute() {
        // Arrange / Act
        var cut = Render<TnTAnimation>((ComponentParameterCollectionBuilder<TnTAnimation> p) => p
            .Add(a => a.Threshold, 0.75f)
            .AddChildContent("Content"));
        // Assert
        cut.Find("tnt-animation").GetAttribute("tnt-threshold").Should().Be("0.75");
    }

    [Fact]
    public void Updates_Class_When_AnimationType_Changes_By_New_Instance() {
        // Arrange
        Render<TnTAnimation>(p => p.AddChildContent("Content")); // initial render (default)
        // Act
        var updated = Render<TnTAnimation>((ComponentParameterCollectionBuilder<TnTAnimation> p) => p
            .Add(a => a.AnimationType, global::TnTComponents.Animation.RotateClockwise)
            .AddChildContent("Content"));
        // Assert (only verifying updated instance)
        updated.Markup.Should().Contain("tnt-animation-rotateclockwise");
    }

    [Fact]
    public void Js_Module_Is_Loaded() {
        // Arrange / Act
        var cut = Render<TnTAnimation>(p => p.AddChildContent("Content"));
        // Assert
        cut.Instance.IsolatedJsModule.Should().NotBeNull();
    }
}
