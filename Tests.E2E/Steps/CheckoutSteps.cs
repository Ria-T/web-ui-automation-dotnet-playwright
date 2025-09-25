using TechTalk.SpecFlow;
using Microsoft.Playwright;
using NUnit.Framework;
using Serilog;
using Common;
using Pages;

namespace Steps;

/// <summary>Step definitions implementing both required scenarios.</summary>
[Binding]
public class CheckoutSteps
{
    private readonly ScenarioContext _sc;
    public CheckoutSteps(ScenarioContext sc) => _sc = sc;

    private IPage Page => _sc.Get<IPage>("page");
    private IReadOnlyList<CartPage.CartLineItem>? _cartSnapshot;

    [When(@"I add (\d+) (?:random )?products? to the cart")]
    public async Task AddNProductsAsync(int count)
    {
        NUnit.Framework.Assert.That(count, NUnit.Framework.Is.GreaterThan(0), "Count must be > 0");
        await new Pages.CatalogPage(Page).AddRandomProductsAsync(count);
    }

    [When("I go to the cart")]
    public Task OpenCartAsync() => new CatalogPage(Page).OpenCartAsync();

    [When("I capture the cart products and prices")]
    public async Task CaptureCartSnapshotAsync()
    {
        _cartSnapshot = await new CartPage(Page).ReadCartLineItemsAsync();
        Assert.That(_cartSnapshot.Count, Is.GreaterThanOrEqualTo(1), "Cart is empty.");
        Log.Information("Cart snapshot: {@Cart}", _cartSnapshot);
    }

    [When("I click Checkout")]
    public Task ClickCheckoutAsync() => new CartPage(Page).ClickCheckoutAsync();

    [When(@"I leave the ""(.*)"" field empty on checkout information page")]
    public async Task OmitFieldAsync(string which)
    {
        var info = new CheckoutInformationPage(Page);
        await info.AssertOnPageAsync();
        await info.ContinueWithMissingAsync(which);
    }

    [When("I attempt to continue to the overview")]
    public Task AttemptContinueAsync() => Task.CompletedTask;

    [Then("I should remain on the checkout information page")]
    public Task AssertStillOnInformationAsync() =>
        new CheckoutInformationPage(Page).AssertOnPageAsync();

    [Then("I should see a validation error")]
    public async Task AssertValidationErrorAsync()
    {
        var visible = await new CheckoutInformationPage(Page).IsValidationAlertVisibleAsync();
        Assert.That(visible, Is.True, "Expected a required-field validation error.");
    }

    [When("I complete the checkout information form")]
    public Task FillInformationFormAsync() =>
        new CheckoutInformationPage(Page).FillAndContinueAsync("Ada", "Lovelace", "12345");

    [When("I continue to the checkout overview")]
    public Task ContinueToOverviewAsync() => Task.CompletedTask;

    [Then("I verify that the overview products and the prices match the cart")]
    public async Task AssertOverviewMatchesCartAsync()
    {
        var ov = new CheckoutOverviewPage(Page);
        await ov.AssertOnPageAsync();
        var overviewItems = await ov.ReadOverviewLineItemsAsync();

        // Fail fast if the snapshot was not captured earlier
        var snapshot = _cartSnapshot ?? throw new AssertionException("Cart snapshot missing.");

        // Compare counts first
        Assert.That(overviewItems.Count, Is.EqualTo(snapshot.Count), "Item count mismatch.");

        // Order-insensitive comparison of (Name, Price) pairs
        var cartPairs = snapshot
            .Select(i => (i.ProductName.Trim(), i.UnitPrice))
            .OrderBy(x => x.Item1)
            .ToList();

        var overviewPairs = overviewItems
            .Select(i => (i.Name.Trim(), i.Price))
            .OrderBy(x => x.Item1)
            .ToList();

        CollectionAssert.AreEqual(cartPairs, overviewPairs, "Products/prices differ.");
    }

    [Then("I verify that the overview subtotal equals the sum of line item totals")]
    public async Task AssertSubtotalAsync()
    {
        var ov = new CheckoutOverviewPage(Page);
        var items = await ov.ReadOverviewLineItemsAsync();
        var expected = items.Sum(i => i.Price);
        var actual = await ov.ReadItemsSubtotalAsync();
        Assert.That(actual, Is.EqualTo(expected).Within(0.01m), "Subtotal mismatch.");
    }
}
