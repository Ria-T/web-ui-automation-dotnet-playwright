using Common;
using Microsoft.Playwright;
using Pages;
using TechTalk.SpecFlow;

[Binding]
public class LoginSteps
{
    private readonly ScenarioContext _sc;
    public LoginSteps(ScenarioContext sc) => _sc = sc;
    private IPage Page => _sc.Get<IPage>("page");

    [Given("I navigate to the website")]
    public Task NavigateAsync() => new LoginPage(Page).OpenAsync();

    [Given("I sign in with valid credentials")]
    public Task SignInAsync() => new LoginPage(Page).SignInAsync(Env.Username, Env.Password);

    [When(@"I sign in with ""(.*)"" and ""(.*)""")]
    public async Task SignInWithAsync(string username, string password)
    {
        username = ResolveEnvToken(username);
        password = ResolveEnvToken(password);
        await new Pages.LoginPage(Page).SignInAndWaitForInventoryAsync(username, password);
    }

    [When(@"I try to sign in with ""(.*)"" and ""(.*)""")]
    public async Task TrySignInAsync(string username, string password)
    {
        username = ResolveEnvToken(username);
        password = ResolveEnvToken(password);
        await new Pages.LoginPage(Page).SignInAsync(username, password);
    }

    [Then("I should land on the inventory page")]
    public async Task AssertInventoryAsync()
    {
        // Wait for inventory marker (first tile) or URL pattern
        await Common.Locators.InventoryLocators.ItemTiles(Page).First.WaitForAsync();
        // or: await Page.WaitForURLAsync("**/inventory.html");
    }

    [Then(@"I should see a login error containing ""(.*)""")]
    public async Task AssertLoginErrorAsync(string expected)
    {
        var page = new Pages.LoginPage(Page);
        Assert.That(await page.IsErrorVisibleAsync(), Is.True, "Expected login error not visible.");
        var text = await page.ReadErrorTextAsync();
        StringAssert.Contains(expected, text, "Error text mismatch.");
    }

    private static string ResolveEnvToken(string value)
    {
        // pattern: {env:VAR}
        if (value.StartsWith("{env:") && value.EndsWith("}"))
        {
            var varName = value[5..^1]; // between {env: and }
            var env = Environment.GetEnvironmentVariable(varName);
            if (string.IsNullOrEmpty(env))
                throw new InvalidOperationException($"Environment variable '{varName}' not set.");
            return env;
        }
        return value;
    }
}
