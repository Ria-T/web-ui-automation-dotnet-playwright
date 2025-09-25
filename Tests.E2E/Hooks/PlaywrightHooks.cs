using Microsoft.Playwright;
using TechTalk.SpecFlow;
using Common;
using Serilog;

namespace Hooks;

/// <summary>Playwright lifecycle: one playwright instance and one browser per run, new context/page per scenario. 
/// Includes video recording and takes screenshots on failure 
/// Configures GetByTestId to target data-test="…"
/// </summary>
[Binding]
public sealed class PlaywrightHooks
{
    private static IPlaywright? _pw;
    private static IBrowser? _browser;

    [BeforeTestRun]
    public static async Task InitAsync()
    {
        Directory.CreateDirectory("reports");
        _pw = await Playwright.CreateAsync();

        // Make page.GetByTestId(...) use data-test="*"
        _pw.Selectors.SetTestIdAttribute("data-test");

        // Launch a single shared browser (headless from Env.Headless)
        _browser = await _pw.Chromium.LaunchAsync(new() { Headless = Env.Headless, SlowMo = Env.SlowMoMs });
        Log.Information("Browser launched. Headless={Headless}", Env.Headless);
    }

    [AfterTestRun]
    public static async Task DisposeAsync() => await _browser!.CloseAsync();

    /// <summary>Per-scenario setup: new isolated context + page.</summary>
    [BeforeScenario]
    public async Task CreateContextAsync(ScenarioContext sc)
    {
        // New context = isolated cookies/localStorage, also attach video recording
        var context = await _browser!.NewContextAsync(new()
        {
            RecordVideoDir = "reports/videos",
            RecordVideoSize = new() { Width = 1280, Height = 720 }
        });
        var page = await context.NewPageAsync(); // Create a tab for this scenario

        // Stash for step definitions to reuse
        sc["page"] = page;
        sc["context"] = context;
    }

    /// <summary>Per-scenario : screenshot on error, close to flush video</summary>
    [AfterScenario]
    public async Task CloseContextAsync(ScenarioContext sc)
    {
        var page = (IPage)sc["page"];
        var context = (IBrowserContext)sc["context"];

        if (sc.TestError is not null)
        {
            var name = $"{Sanitize(sc.ScenarioInfo.Title)}.png";
            await page.ScreenshotAsync(new() { Path = Path.Combine("reports", name), FullPage = true });
            Log.Error(sc.TestError, "Scenario failed: {Title}", sc.ScenarioInfo.Title);
        }
        await context.CloseAsync();  // Closing the context finalizes and writes the video to disk (flush)
    }

    // Replace invalid filename chars with underscores so we can save files per scenario title
    private static string Sanitize(string s) => string.Join("_", s.Split(Path.GetInvalidFileNameChars()));
}

