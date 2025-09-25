Feature: Login validation

  Background:
    Given I navigate to the website

  @login @happy
  Scenario Outline: Successful login with valid credentials
    When I sign in with "<username>" and "<password>"
    Then I should land on the inventory page

    Examples:
      | username        | password       |
      | standard_user   | secret_sauce   |

  @login @negative
  Scenario Outline: Login shows an error for invalid credentials
    When I try to sign in with "<username>" and "<password>"
    Then I should see a login error containing "<message>"

    Examples:
      | username         | password        | message                    |
      | wrong_user       | secret_sauce    | Epic sadface               |
      | standard_user    | wrong_pass      | Epic sadface               |
      |                  | secret_sauce    | Username is required       |
      | standard_user    |                 | Password is required       |
