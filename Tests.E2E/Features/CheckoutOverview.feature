@checkout @overview

Feature: Checkout overview carries products and prices forward

  Background:
    Given I navigate to the website
    And I sign in with valid credentials

  Scenario: Products and prices transfer correctly from cart
    When I add 2 random products to the cart
    And I go to the cart
    And I capture the cart products and prices
    And I click Checkout
    And I complete the checkout information form
    And I continue to the checkout overview
    Then I verify that the overview products and the prices match the cart
    And I verify that the overview subtotal equals the sum of line item totals
