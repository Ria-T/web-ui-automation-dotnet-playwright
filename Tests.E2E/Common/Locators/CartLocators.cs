using Microsoft.Playwright;

namespace Common.Locators;

/// <summary>Locators for the Cart page </summary>
public static class CartLocators
{
    /// <summary>
    /// All cart line-item rows.
    /// Returns a *collection locator* (can use .CountAsync(), .Nth(i), .First, etc.).
    /// </summary>
    public static ILocator Rows(IPage p) => p.Locator(".cart_item"); // row structure

    /// <summary>
    /// Product name cell within a given row
    /// We scope the search to the row
    /// </summary>
    public static ILocator RowName(ILocator row) => row.GetByTestId("inventory-item-name");

    /// <summary>
    /// Product price cell within a given row
    /// </summary>
    public static ILocator RowPrice(ILocator row) => row.GetByTestId("inventory-item-price");
    public static ILocator CheckoutButton(IPage p) => p.GetByTestId("checkout");
}
