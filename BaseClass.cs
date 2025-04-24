using Microsoft.Playwright;


public class BaseClass
{
    protected IPlaywright _playwright;
    protected IBrowser _browser;
    protected IBrowserContext _context;
    protected IPage _page;

    public async Task StartAsync(bool headless = true)
    {
        _playwright = await Playwright.CreateAsync();

        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = headless,
            Channel = "msedge" // 🔁 Launches Microsoft Edge
        });

        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();
    }

    public async Task StopAsync()
    {
        if (_context != null)
            await _context.CloseAsync();

        if (_browser != null)
            await _browser.CloseAsync();

        _playwright?.Dispose();
    }
}
