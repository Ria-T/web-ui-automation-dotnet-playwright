using Microsoft.Playwright;

namespace Common.Locators;

/// <summary>Stable locators for the Login page (prefers data-test) </summary>
public static class LoginLocators
{
    public static ILocator Username(IPage p) => p.GetByTestId("username");
    public static ILocator Password(IPage p) => p.GetByTestId("password");
    public static ILocator LoginButton(IPage p) => p.GetByTestId("login-button");
    public static ILocator ErrorBanner(IPage page) => page.GetByTestId("error");
}
