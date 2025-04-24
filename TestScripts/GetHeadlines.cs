using PlaywrightPractice;

namespace PlaywrightPractice.TestScript;

[TestFixture]
public class GetHeadlines : BaseClass
{

    [OneTimeSetUp]
    public async Task Setup()
    {
        await StartAsync(headless: false); // 👀 Set to true for headless mode
    }
     [OneTimeTearDown]
    public async Task Teardown()
    {
        await StopAsync();
    }



    [Test]
    public async Task GetAllHeadlingFromTopStoriesAsync()
    {
         await _page.GotoAsync("https://news.google.com/home?hl=en-IN&gl=IN&ceid=IN:en");

         var topstoriesCard =   _page.Locator("//*[@class='aqvwYd' and text()='Top stories']/../../../following-sibling::div");
         var Storiesheading = await topstoriesCard.Locator("//a[@class='JtKRv iTin5e' or @class='gPFEn']").AllAsync();
        int i = 0;
        foreach(var card in Storiesheading)
        {
            i++;
            string headlines = await card.InnerTextAsync();
            Console.WriteLine($"heading {i}: ");
            Console.WriteLine(headlines);
            Console.WriteLine("-------------------------");
        } 
    }
}