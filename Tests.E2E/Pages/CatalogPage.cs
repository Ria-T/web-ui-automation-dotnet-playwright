using Microsoft.Playwright;
using Common.Locators;

namespace Pages;

/// <summary>Inventory page: add products and open the cart </summary>
public class CatalogPage
{
    private readonly IPage _page;
    public CatalogPage(IPage page) => _page = page;

    // Uses collection locators and per-tile scoping to avoid cross-tile matches
    public async Task AddRandomProductsAsync(int count)
    {
        var tiles = InventoryLocators.ItemTiles(_page);  // Collection locator for all product tiles
        int available = await tiles.CountAsync();
        if (available == 0) throw new InvalidOperationException("No inventory items found.");

        // Build a random set of distinct indices in [0..available)
        // Enumerable.Range creates 0..available-1, then we shuffle via OrderBy(random key),
        // and Take(count) selects the first 'count'. This guarantees no duplicates
        var picks = Enumerable.Range(0, available).OrderBy(_ => Guid.NewGuid()).Take(count);
        foreach (int i in picks)// For each selected tile, scope to it (Nth(i)) and click its Add button.
            await InventoryLocators.AddToCartButton(tiles.Nth(i)).ClickAsync();
    }

    public Task OpenCartAsync() => InventoryLocators.CartLink(_page).ClickAsync();
}
