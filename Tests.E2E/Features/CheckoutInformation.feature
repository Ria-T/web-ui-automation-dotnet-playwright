@checkout @information

Feature: Checkout information validation

  Background:
    Given I navigate to the website
    And I sign in with valid credentials

  Scenario Outline: Cannot continue if a required field is missing
    When I add 1 product to the cart
    And I go to the cart
    And I click Checkout
    And I leave the "<missing>" field empty on checkout information page
    And I attempt to continue to the overview
    Then I should remain on the checkout information page
    And I should see a validation error
    Examples:
      | missing     |
      | First Name  |
      | Last Name   |
      | Postal Code |