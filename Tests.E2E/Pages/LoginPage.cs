using Microsoft.Playwright;
using Common;
using Common.Locators;

namespace Pages;

/// <summary>Login page flow </summary>
public class LoginPage
{
    private readonly IPage _page;
    public LoginPage(IPage page) => _page = page;

    public Task OpenAsync() => _page.GotoAsync(Env.BaseUrl);

    public async Task SignInAsync(string username, string password)
    {
        await LoginLocators.Username(_page).FillAsync(username);
        await LoginLocators.Password(_page).FillAsync(password);
        await LoginLocators.LoginButton(_page).ClickAsync();
    }

    // For happy path only:
    public async Task SignInAndWaitForInventoryAsync(string username, string password)
    {
        await SignInAsync(username, password);
        await _page.WaitForURLAsync("**/inventory.html"); 
    }

    public Task<bool> IsErrorVisibleAsync() =>
    LoginLocators.ErrorBanner(_page).IsVisibleAsync(); 

    public async Task<string?> ReadErrorTextAsync()
    {
        var loc = LoginLocators.ErrorBanner(_page);
        return await loc.IsVisibleAsync() ? (await loc.InnerTextAsync()).Trim() : null;
    }
}

