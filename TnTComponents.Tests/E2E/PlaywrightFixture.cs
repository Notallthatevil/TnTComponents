using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Playwright;
using System.Diagnostics;

namespace TnTComponents.Tests.E2E;

/// <summary>
///     Base fixture for E2E tests using Playwright. Handles browser, context, and page lifecycle management with a test server using WebApplicationFactory.
/// </summary>
public class PlaywrightFixture : IAsyncLifetime {
    public HttpClient HttpClient {
        get {
            if (_factory == null) throw new InvalidOperationException("Web application factory is not initialized.");
            return new HttpClient { BaseAddress = new Uri(ServerAddress) };
        }
    }
    public string ServerAddress => _factory?.ServerAddress ?? throw new InvalidOperationException("Web application factory is not initialized.");
    public IBrowserContext Context { get; private set; } = null!;
    public IPage Page { get; private set; } = null!;
    private const int HealthCheckTimeoutSeconds = 30;
    private IBrowser? _browser;
    private IBrowserType? _browserType;
    private NTWebAppFactory? _factory;
    private IPlaywright? _playwright;

    /// <summary>
    ///     Clean up browser and application resources after test.
    /// </summary>
    public async ValueTask DisposeAsync() {
        if (Page != null) {
            await Page.CloseAsync();
        }

        if (Context != null) {
            await Context.CloseAsync();
        }

        if (_browser != null) {
            await _browser.CloseAsync();
        }

        _playwright?.Dispose();

        // Dispose of the WebApplicationFactory
        _factory?.Dispose();
    }

    /// <summary>
    ///     Initialize the test application server and Playwright browser.
    /// </summary>
    public async ValueTask InitializeAsync() {
        // Start the application using WebApplicationFactory
        _factory = new();
        // Ensure the server is started so ServerAddress is populated
        _ = _factory.Services;

        // Initialize Playwright browser
        _playwright = await Playwright.CreateAsync();
        _browserType = _playwright.Chromium;
        _browser = await _browserType.LaunchAsync(new() {
            Headless = true,
        });

        Context = await _browser.NewContextAsync();
        Page = await Context.NewPageAsync();
    }
}