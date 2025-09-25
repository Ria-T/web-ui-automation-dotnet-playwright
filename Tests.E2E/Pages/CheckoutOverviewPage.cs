using Microsoft.Playwright;
using Common.Locators;
using Common.Extensions;

namespace Pages;

/// <summary>Checkout Overview page (review items and subtotal).</summary>
public class CheckoutOverviewPage
{
    private readonly IPage _page;
    public CheckoutOverviewPage(IPage page) => _page = page;

    // Overview page "am I here?"
    public Task AssertOnPageAsync() =>
        CheckoutLocators.OverviewLineItems(_page).First.WaitForAsync();

    public async Task<IReadOnlyList<LineItem>> ReadOverviewLineItemsAsync()
    {
        var rows = CheckoutLocators.OverviewLineItems(_page);
        int count = await rows.CountAsync();
        var items = new List<LineItem>(count);

        for (int i = 0; i < count; i++)
        {
            var row = rows.Nth(i);
            string name = (await CheckoutLocators.OverviewLineItemName(row).InnerTextAsync()).Trim();
            string price = (await CheckoutLocators.OverviewLineItemPrice(row).InnerTextAsync()).Trim();
            items.Add(new LineItem(name, price.ToMoney()));
        }
        return items;
    }

    public async Task<decimal> ReadItemsSubtotalAsync()
    {
        string label = (await CheckoutLocators.OverviewItemsSubtotalLabel(_page).InnerTextAsync()).Trim();
        return label.ToMoney();
    }
}

public record LineItem(string Name, decimal Price);
