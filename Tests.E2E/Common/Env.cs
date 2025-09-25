namespace Common;

/// <summary>Environment, configurable settings with safe defaults so the suite runs out-of-the-box</summary>
public static class Env
{
    public static string BaseUrl => Get("BASE_URL", "https://www.saucedemo.com/");
    public static string Username => Get("TEST_USER", "standard_user");
    public static string Password => Get("TEST_PASS", "secret_sauce");
    public static bool Headless => Get("HEADLESS", "true") == "true";

    //for slower video recording
    public static int SlowMoMs
    => int.TryParse(Get("SLOWMO_MS", "0"), out var v) ? v : 0; 


    // Private helper: reads an environment variable or falls back to a default
    private static string Get(string key, string fallback) =>
        Environment.GetEnvironmentVariable(key) ?? fallback;
}

