using System.Collections.Generic;
using TnTComponents.Core;
using Microsoft.AspNetCore.Components;
using AwesomeAssertions;
using Xunit;

public class TnTComponentBaseTests {
    private class TestComponent : TnTComponentBase {
        public override string? ElementClass => "test-class";
        public override string? ElementStyle => "color: red;";
        public void CallOnParametersSet() => OnParametersSet();
    }

    [Fact]
    public void ComponentIdentifier_IsNotNullOrEmpty() {
        var comp = new TestComponent();
        comp.ComponentIdentifier.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void AdditionalAttributes_SetsCustomIdentifier() {
        var comp = new TestComponent();
        comp.AdditionalAttributes = new Dictionary<string, object> { { "foo", "bar" } };
        comp.CallOnParametersSet();
        comp.AdditionalAttributes.Should().NotBeNull();
        comp.AdditionalAttributes.Should().ContainKey("tntid");
        comp.AdditionalAttributes?["tntid"].Should().Be(comp.ComponentIdentifier);
    }

    [Fact]
    public void OnParametersSet_DoesNotOverwriteExistingIdentifier() {
        var comp = new TestComponent();
        var id = comp.ComponentIdentifier;
        comp.AdditionalAttributes = new Dictionary<string, object> { { "tntid", id } };
        comp.CallOnParametersSet();
        comp.AdditionalAttributes?["tntid"].Should().Be(id);
    }

    [Fact]
    public void OnParametersSet_CreatesNewDictionaryIfNull() {
        var comp = new TestComponent();
        comp.AdditionalAttributes = null;
        comp.CallOnParametersSet();
        comp.AdditionalAttributes.Should().NotBeNull();
        comp.AdditionalAttributes.Should().ContainKey("tntid");
    }

    [Fact]
    public void ElementClass_ReturnsExpectedValue() {
        var comp = new TestComponent();
        comp.ElementClass.Should().Be("test-class");
    }

    [Fact]
    public void ElementStyle_ReturnsExpectedValue() {
        var comp = new TestComponent();
        comp.ElementStyle.Should().Be("color: red;");
    }
}
