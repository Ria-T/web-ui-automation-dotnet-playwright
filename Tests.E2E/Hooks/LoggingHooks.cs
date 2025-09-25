using Serilog;
using TechTalk.SpecFlow;

namespace Hooks;

/// <summary>Configure Serilog once per run and write to logs/run.log , closes the logger at the end</summary>
[Binding]
public sealed class LoggingHooks
{
    [BeforeTestRun]
    public static void SetupLogger()
    {
        Directory.CreateDirectory("logs");

        // Build the Serilog logger:
        // - Minimum level: Information (skip Debug/Verbose noise)
        // - Sink: rolling file logs/run.log (one file per day), keep only 3 days
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File("logs/run.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 3)
            .CreateLogger();
        Log.Information("=== Test run started {Time} ===", DateTimeOffset.Now);
    }

    [AfterTestRun]
    public static void TearDown()
    {
        Log.Information("=== Test run finished {Time} ===", DateTimeOffset.Now);
        Log.CloseAndFlush();
    }
}
