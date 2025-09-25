# web-ui-automation-dotnet-playwright
Simple tests for the classic Swag Labs website

-	Tech stack: .NET 8 + Playwright (C#), SpecFlow (Gherkin), NUnit, Serilog
(OS: Windows 10, IDE: Visual Studio)


- How to run:
  
Prerequisites:

.NET 8 SDK installed

Playwright browsers installed once:

dotnet tool update --global Microsoft.Playwright.CLI

playwright install

Environment (optional, defaults shown):
$env:BASE_URL  = "https://www.saucedemo.com/"
$env:TEST_USER = "standard_user"
$env:TEST_PASS = "secret_sauce"
$env:HEADLESS  = "true"   # set "false" to watch in a real browser

Run tests:
dotnet test -c Release --logger "trx;LogFileName=TestResults.trx"

Headed debug run:
$env:HEADLESS = "false"
dotnet test -c Release

Filter by tags (if tagged):
dotnet test -c Release --filter TestCategory=checkout

Clean & rebuild:
dotnet clean
dotnet restore
dotnet build -c Release

For slower execution & slower video:
$env:SLOWMO_MS = "500"
