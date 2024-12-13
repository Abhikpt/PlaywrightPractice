namespace PlaywrightPractice;

using Microsoft.Playwright;
using NUnit.Compatibility;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async  Task  Test1()
    {
    
        //Playwright
        using var playwright = await Playwright.CreateAsync();

        //browser
        await using var browser = await playwright.Chromium.LaunchAsync( new BrowserTypeLaunchOptions
        {
            Headless = false
        });

        // page
        var page = await browser.NewPageAsync();

        //go to URl
        await page.GotoAsync( "http://www.eaapp.somee.com");

        //clicked on LoginElement
        await page.ClickAsync(selector: "text=Login");

        //take screenshot
        await page.ScreenshotAsync(new PageScreenshotOptions
        {

            // store at bin/debug/net8.0
            Path = "../../../Screenshots/ss01.jpg"

        });
            
       
    }
}