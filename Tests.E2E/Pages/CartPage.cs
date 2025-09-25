using Common.Extensions;
using Common.Locators;
using Microsoft.Playwright;
using Serilog;

namespace Pages;

/// <summary>Cart page: read line items and proceed to checkout </summary>
public class CartPage
{
    private readonly IPage _page;
    public CartPage(IPage page) => _page = page;

    public record CartLineItem(string ProductName, decimal UnitPrice);

    /// <summary>
    /// Scrape the cart table into strongly-typed line items
    /// Uses collection locators, trims text, and parses price strings via ToMoney()
    /// </summary>
    public async Task<IReadOnlyList<CartLineItem>> ReadCartLineItemsAsync()
    {
        var rows = CartLocators.Rows(_page);
        int n = await rows.CountAsync();

        Log.Warning("Cart item rows found: {Count}", n);
        if (n == 0)
            Log.Warning("Cart is empty when attempting to read line items.");

        var list = new List<CartLineItem>(n); // Pre-size the list to avoid internal resizing while adding

        for (int i = 0; i < n; i++)
        {
            var row = rows.Nth(i); // scope to a single row
            string name = (await CartLocators.RowName(row).InnerTextAsync()).Trim();
            string priceText = (await CartLocators.RowPrice(row).InnerTextAsync()).Trim();
            list.Add(new CartLineItem(name, priceText.ToMoney()));
        }
        return list;
    }

    /// <summary>
    /// Clicks "Checkout" and waits for navigation to the step-one URL
    /// Task.WhenAll binds the wait to the click to prevent race conditions
    /// </summary>
    public async Task ClickCheckoutAsync()
    {
        await Task.WhenAll(
            _page.WaitForURLAsync("**/checkout-step-one.html"), // Explicit navigation wait (URL pattern). Could also wait for a page marker
            CartLocators.CheckoutButton(_page).ClickAsync()
        );
    }

}

