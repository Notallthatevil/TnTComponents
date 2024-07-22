using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System.Diagnostics;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTNavLink {

    [Parameter]
    public TnTColor? ActiveBackgroundColor { get; set; }

    [Parameter]
    public TnTColor ActiveTextColor { get; set; } = TnTColor.OnSecondaryContainer;

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public bool? AutoFocus { get; set; }

    [Parameter]
    public TnTColor? BackgroundColor { get; set; } = TnTColor.Transparent;

    [Parameter]
    public TnTBorderRadius? BorderRadius { get; set; } = new(10);

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;
    [Parameter]
    /// <inheritdoc />
    public string? ElementLang { get; set; }
    [Parameter]
    /// <inheritdoc />
    public string? ElementTitle { get; set; }
    public string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddDisabled(Disabled)
        .AddRipple(Ripple)
        .AddActionableBackgroundColor(_isActive ? ActiveBackgroundColor : BackgroundColor)
        .AddForegroundColor(_isActive ? ActiveTextColor : TextColor)
        .AddOutlined(Outlined)
        .AddFilled(BackgroundColor != TnTColor.Transparent)
        .AddElevation(Elevation)
        .AddBorderRadius(BorderRadius)
        .AddClass("tnt-active", _isActive)
        .Build();

    public string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public ElementReference Element { get; set; }

    [Parameter]
    public int Elevation { get; set; } = 0;

    [Parameter]
    public TnTIcon? EndIcon { get; set; }

    [Parameter, EditorRequired]
    public string Href { get; set; } = default!;

    [Parameter]
    public string? ElementId { get; set; }

    [Parameter]
    public NavLinkMatch Match { get; set; }

    [Parameter]
    public bool Outlined { get; set; }

    [Parameter]
    public bool Ripple { get; set; }

    [Parameter]
    public TnTIcon? StartIcon { get; set; }

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    private bool _isActive { get; set; }

    [Inject]
    private NavigationManager _navManager { get; set; } = default!;

    private string? _hrefAbsolute;

    public void Dispose() {
        _navManager.LocationChanged -= OnLocationChanged;
    }

    protected override void OnInitialized() {
        _navManager.LocationChanged += OnLocationChanged;
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();
        _hrefAbsolute = Href is null ? null : _navManager.ToAbsoluteUri(Href).AbsoluteUri;
        _isActive = ShouldMatch(_navManager.Uri);
    }

    private static bool IsStrictlyPrefixWithSeparator(string value, string prefix) {
        var prefixLength = prefix.Length;
        if (value.Length > prefixLength) {
            return value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                && (
                    // Only match when there's a separator character either at the end of the prefix
                    // or right after it.
                    // Example: "/abc" is treated as a prefix of "/abc/def" but not "/abcdef"
                    // Example: "/abc/" is treated as a prefix of "/abc/def" but not "/abcdef"
                    prefixLength == 0
                    || !IsUnreservedCharacter(prefix[prefixLength - 1])
                    || !IsUnreservedCharacter(value[prefixLength])
                );
        }
        else {
            return false;
        }
    }

    private static bool IsUnreservedCharacter(char c) {
        // Checks whether it is an unreserved character according to
        // https://datatracker.ietf.org/doc/html/rfc3986#section-2.3 Those are characters that are
        // allowed in a URI but do not have a reserved purpose (e.g. they do not separate the
        // components of the URI)
        return char.IsLetterOrDigit(c) ||
                c == '-' ||
                c == '.' ||
                c == '_' ||
                c == '~';
    }

    private bool EqualsHrefExactlyOrIfTrailingSlashAdded(string currentUriAbsolute) {
        Debug.Assert(_hrefAbsolute != null);

        if (string.Equals(currentUriAbsolute, _hrefAbsolute, StringComparison.OrdinalIgnoreCase)) {
            return true;
        }

        if (currentUriAbsolute.Length == _hrefAbsolute.Length - 1) {
            // Special case: highlight links to http://host/path/ even if you're at http://host/path
            // (with no trailing slash) // This is because the router accepts an absolute URI value
            // of "same as base URI but without trailing slash" as equivalent to "base URI", which
            // in turn is because it's common for servers to return the same page for
            // http://host/vdir as they do for host://host/vdir/ as it's no good to display a blank
            // page in that case.
            if (_hrefAbsolute[_hrefAbsolute.Length - 1] == '/' && _hrefAbsolute.StartsWith(currentUriAbsolute, StringComparison.OrdinalIgnoreCase)) {
                return true;
            }
        }

        return false;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs args) {
        // We could just re-render always, but for this component we know the only relevant state
        // change is to the _isActive property.
        var shouldBeActiveNow = ShouldMatch(args.Location);
        if (shouldBeActiveNow != _isActive) {
            _isActive = shouldBeActiveNow;
            StateHasChanged();
        }
    }

    /// Taken from
    /// <see cref="NavLink" />
    private bool ShouldMatch(string currentUriAbsolute) {
        if (_hrefAbsolute == null) {
            return false;
        }

        if (EqualsHrefExactlyOrIfTrailingSlashAdded(currentUriAbsolute)) {
            return true;
        }

        if (Match == NavLinkMatch.Prefix && IsStrictlyPrefixWithSeparator(currentUriAbsolute, _hrefAbsolute)) {
            return true;
        }

        return false;
    }

    /// Taken from
    /// <see cref="NavLink" />
    /// Taken from
    /// <see cref="NavLink" />
    /// Taken from
    /// <see cref="NavLink" />
}