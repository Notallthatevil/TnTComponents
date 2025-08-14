using System;
using AwesomeAssertions;
using TnTComponents.Core;
using Xunit;

public class TnTComponentIdentifierTests {
    private class TestContext : TnTComponentIdentifierContext {
        public TestContext(Func<uint, string> newId) : base(newId) { }
    }
    [Fact]
    public void NewId_DefaultLength_ReturnsPrefixedHexString() {
        var id = TnTComponentIdentifier.NewId();
        id.Should().StartWith("tnt_");
        id.Length.Should().BeGreaterThan(4);
    }

    [Fact]
    public void NewId_CustomLength_ReturnsCorrectLength() {
        var id = TnTComponentIdentifier.NewId(12);
        id.Should().StartWith("tnt_");
        id.Length.Should().Be(12);
    }

    [Fact]
    public void NewId_ThrowsIfLengthTooLarge() {
        Action act = () => TnTComponentIdentifier.NewId(20);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void NewId_UsesContextIfSet() {
        using var ctx = new TnTComponentIdentifierContext(i => $"custom_{i}");
        var id = TnTComponentIdentifier.NewId();
        id.Should().Be("custom_0");
        var id2 = TnTComponentIdentifier.NewId();
        id2.Should().Be("custom_1");
    }

    [Fact]
    public void Context_Dispose_RemovesCurrent() {
        var ctx = new TnTComponentIdentifierContext(i => $"custom_{i}");
        TnTComponentIdentifierContext.Current.Should().NotBeNull();
        ctx.Dispose();
        TnTComponentIdentifierContext.Current.Should().BeNull();
    }

    [Fact]
    public void Context_GenerateId_ResetsAfterMax() {
    using var ctx = new TestContext(i => $"id_{i}");
    var field = typeof(TnTComponentIdentifierContext).GetField("<_currentIndex>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    field?.SetValue(ctx, 99999998u);
    // Assert field is set correctly before calling GenerateId
    var currentIndex = (uint?)field?.GetValue(ctx);
    currentIndex.Should().Be(99999998u, "_currentIndex should be set to 99999998 before GenerateId()");
    var method = typeof(TnTComponentIdentifierContext).GetMethod("GenerateId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    var id1 = method?.Invoke(ctx, null) as string;
    id1.Should().Be("id_99999998");
    var id2 = method?.Invoke(ctx, null) as string;
    id2.Should().Be("id_0");
    var id3 = method?.Invoke(ctx, null) as string;
    id3.Should().Be("id_1");
    }
}
