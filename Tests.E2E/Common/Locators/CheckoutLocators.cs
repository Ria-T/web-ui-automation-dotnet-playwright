using Microsoft.Playwright;

namespace Common.Locators;

/// <summary>
/// Checkout selectors split by purpose: Information (form) and Overview (review)
/// Prefers TestId, then roles/labels, then scoped CSS
/// </summary>
public static class CheckoutLocators
{
    // ----- Information (Checkout Step One: information form) -----
    public static ILocator InformationHeader(IPage p) => p.GetByRole(AriaRole.Heading, new() { Name = "Checkout: Your Information" });
    public static ILocator InformationFirstNameInput(IPage p) => p.GetByTestId("firstName");
    public static ILocator InformationLastNameInput(IPage p) => p.GetByTestId("lastName");
    public static ILocator InformationPostalCodeInput(IPage p) => p.GetByTestId("postalCode");
    public static ILocator InformationContinueButton(IPage p) => p.GetByTestId("continue");
    public static ILocator InformationValidationAlert(IPage p) => p.GetByTestId("error");

    // ----- Overview (Checkout Step Two: review cart page) -----
    public static ILocator OverviewHeader(IPage p) => p.GetByRole(AriaRole.Heading, new() { Name = "Checkout: Overview" });
    public static ILocator OverviewLineItems(IPage p) => p.Locator(".cart_item");
    public static ILocator OverviewLineItemName(ILocator row) => row.GetByTestId("inventory-item-name");
    public static ILocator OverviewLineItemPrice(ILocator row) => row.GetByTestId("inventory-item-price");
    public static ILocator OverviewItemsSubtotalLabel(IPage p) => p.Locator(".summary_subtotal_label");
}
