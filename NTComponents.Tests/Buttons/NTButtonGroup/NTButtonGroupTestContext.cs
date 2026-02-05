using System.Collections.Generic;
using AutoFixture;
using Bunit;
using NTComponents;
using NTComponents.Tests.TestingUtility;
using TestingUtilityHelpers = NTComponents.Tests.TestingUtility.TestingUtility;

namespace NTComponents.Tests.Buttons.NTButtonGroup;

/// <summary>
///     Provides shared setup and helpers for testing <see cref="NTButtonGroup{TObjectType}" />.
/// </summary>
public abstract class NTButtonGroupTestContext : BunitContext {
    /// <summary>
    ///     Fixture used to generate random test data.
    /// </summary>
    protected Fixture Fixture { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="NTButtonGroupTestContext" /> class.
    /// </summary>
    protected NTButtonGroupTestContext() {
        Fixture = new Fixture();
        TestingUtilityHelpers.SetupRippleEffectModule(this);
    }

    /// <summary>
    ///     Creates a pair of items that can be shared across tests.
    /// </summary>
    /// <param name="iconOnlyFirstItem">Determines whether the first item renders as an icon-only button.</param>
    /// <param name="defaultSecondItem">Determines whether the second item should be marked as default selected.</param>
    /// <returns>A collection containing the generated items.</returns>
    protected IReadOnlyCollection<NTButtonGroupItem<string>> CreateItems(bool iconOnlyFirstItem = false, bool defaultSecondItem = false) {
        var first = new NTButtonGroupItem<string> {
            Key = Fixture.Create<string>(),
            Label = iconOnlyFirstItem ? null : Fixture.Create<string>(),
            IsDefaultSelected = false
        };

        if (iconOnlyFirstItem) {
            first = first with {
                StartIcon = MaterialIcon.Home
            };
        }

        var second = new NTButtonGroupItem<string> {
            Key = Fixture.Create<string>(),
            Label = Fixture.Create<string>(),
            IsDefaultSelected = defaultSecondItem
        };

        return new[] { first, second };
    }
}
