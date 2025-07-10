namespace TnTComponents.Tests.TestingUtility;
public partial class TnTIconStub {
    public override string? ElementClass => throw new NotImplementedException();
    public override string? ElementStyle => throw new NotImplementedException();
    public TnTIconStub() {
        // Default constructor
    }

    public TnTIconStub(string icon) => Icon = icon;
}
