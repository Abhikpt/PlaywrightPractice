namespace PlaywrightPractice;

using Microsoft.Playwright;


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
        await using var browser = await playwright.Chromium.LaunchAsync();

        // page
        var page = await browser.NewPageAsync();

        //go to URl
        await page.GotoAsync( "http://www.eaapp.somee.com");

        //clicked on LoginElement
        await page.ClickAsync(selector: "text=Login");



       
    }
}