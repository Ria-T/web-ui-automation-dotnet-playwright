using Microsoft.Playwright;
using Common.Locators;

namespace Pages;

/// <summary>Checkout “Your Information” form page.</summary>
public class CheckoutInformationPage
{
    private readonly IPage _page;
    public CheckoutInformationPage(IPage page) => _page = page;

    // Information page "am I here?"
    public Task AssertOnPageAsync() => CheckoutLocators.InformationFirstNameInput(_page).WaitForAsync();

    public async Task FillAndContinueAsync(string first, string last, string postal)
    {
        await CheckoutLocators.InformationFirstNameInput(_page).FillAsync(first);
        await CheckoutLocators.InformationLastNameInput(_page).FillAsync(last);
        await CheckoutLocators.InformationPostalCodeInput(_page).FillAsync(postal);
        await CheckoutLocators.InformationContinueButton(_page).ClickAsync();
    }

    public async Task ContinueWithMissingAsync(string fieldToOmit)
    {
        if (!fieldToOmit.Equals("First Name", StringComparison.OrdinalIgnoreCase))
            await CheckoutLocators.InformationFirstNameInput(_page).FillAsync("Ada");
        if (!fieldToOmit.Equals("Last Name", StringComparison.OrdinalIgnoreCase))
            await CheckoutLocators.InformationLastNameInput(_page).FillAsync("Lovelace");
        if (!fieldToOmit.Equals("Postal Code", StringComparison.OrdinalIgnoreCase))
            await CheckoutLocators.InformationPostalCodeInput(_page).FillAsync("12345");

        await CheckoutLocators.InformationContinueButton(_page).ClickAsync();
    }

    public Task<bool> IsValidationAlertVisibleAsync() =>
        CheckoutLocators.InformationValidationAlert(_page).IsVisibleAsync();

    public async Task<string?> ReadValidationTextAsync()
    {
        var err = CheckoutLocators.InformationValidationAlert(_page);
        return await err.IsVisibleAsync() ? (await err.InnerTextAsync()).Trim() : null;
    }
}

