using Microsoft.Playwright;

namespace Common.Locators;

/// <summary>Locators for the inventory grid and cart link </summary>
public static class InventoryLocators
{
    public static ILocator ItemTiles(IPage p) => p.GetByTestId("inventory-item");
    public static ILocator ItemName(ILocator tile) => tile.GetByTestId("inventory-item-name"); // Product name element within a given tile 
    public static ILocator ItemPrice(ILocator tile) => tile.GetByTestId("inventory-item-price"); // Product price element within a given tile
    public static ILocator AddToCartButton(ILocator tile) => tile.GetByRole(AriaRole.Button, new() { Name = "Add to cart" }); // "Add to cart" button inside a tile
    public static ILocator CartLink(IPage p) => p.GetByTestId("shopping-cart-link");
}

