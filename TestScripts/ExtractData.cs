namespace PlaywrightPractice;

using System.Security.Cryptography.X509Certificates;
using Microsoft.Playwright;
using PlaywrightPractice.Utilities;

public class ExtractData
{
    public static string DateString = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
    public static string ProjectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
    //   public static string ScreenshotPath = "../../../Resources/TestSS" + DateString + ".jpg";
    public static string jsonFilePath = "../../../Resources/CricketCard" + DateString + ".json" ;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async  Task  Test1()
    {

        List<CricketMatchCard> matchCardsdetails = new List<CricketMatchCard>();
        List<CricketMatchCard> MatchCompleted = new List<CricketMatchCard>();
        List<CricketMatchCard> MatchUpcoming = new List<CricketMatchCard>();
        List<CricketMatchCard> MatchLive = new List<CricketMatchCard>();
        List<CricketMatchCard> matchCardsFromJson = new List<CricketMatchCard>();

        //Playwright
        using var playwright = await Playwright.CreateAsync();

        //create browser object by setting browser in non-headless mode
        await using var browser = await playwright.Chromium.LaunchAsync( new  BrowserTypeLaunchOptions
        {
            Headless = false
        });
       

        // create new page in browser context
        var page = await browser.NewPageAsync();
        
        //go to URl
        await page.GotoAsync( "https://www.espncricinfo.com/", new PageGotoOptions
        {
            Timeout = 60000 // Set timeout to 60 seconds
        });

        Console.WriteLine($"PageTitle: { await page.TitleAsync()}"); 

        //take screenshot
        await page.ScreenshotAsync(new PageScreenshotOptions
        {   // store at bin/debug/net8.0
            Path = ProjectDirectory + "/Resources/TestSS" + DateString + ".jpg"
        });


        //--------------------------extracting Data --------------------------------------       
        await page.WaitForTimeoutAsync(3000); // Let dynamic content load

        var matchCardList = await page.Locator(@"div.slick-list>div.slick-track > div.slick-slide div.ds-w-\[288px\]").AllAsync();
        
        Console.WriteLine($"Total Match Cards: {matchCardList.Count}");

        foreach (var card in matchCardList)
        {
            
              // if matche is completed/live
            var elements = await card.Locator("div.ds-truncate span.ds-text-tight-xs.ds-leading-5").AllAsync();
            if (elements.Count > 0 && await elements[0].IsVisibleAsync())
            {
                var matchTitle = await card.Locator("div.ds-truncate span.ds-text-tight-xs.ds-text-typo-mid2").InnerTextAsync();
                var team1 = await card.Locator("div.ci-team-score:nth-child(1) p").InnerTextAsync();
                var team2 = await card.Locator("div.ci-team-score:nth-child(2) p").InnerTextAsync();
                var MatchStatus = await card.Locator("div.ds-h-3 p span").InnerTextAsync();
                var score1 = await card.Locator("div.ci-team-score:nth-child(1) > div:nth-child(2)").InnerTextAsync();    
                var score2 = await card.Locator("div.ci-team-score:nth-child(2) > div:nth-child(2)").InnerTextAsync();
                MatchCompleted.Add(new CricketMatchCard
                {
                    MatchTitle = matchTitle,
                    Team1 = team1,
                    Team2 = team2,
                    MatchStatus = MatchStatus,
                    Score1 = score1,
                    Score2 = score2,    
                });

            }

            else

            {
                var matchTitle = await card.Locator("css=div.ds-truncate span.ds-text-tight-xs.ds-text-typo-mid2").InnerTextAsync();
                var team1 = await card.Locator("css=div.ci-team-score:nth-child(1) p").InnerTextAsync();
                var team2 = await card.Locator("css=div.ci-team-score:nth-child(2) p").InnerTextAsync();
                var MatchStatus = await card.Locator("css=div.ds-h-3 p span").InnerTextAsync();
                var MatchTime = await card.Locator("css=div.ds-text-tight-m.ds-font-bold").InnerTextAsync() ?? "N/A";
                MatchUpcoming.Add(new CricketMatchCard
                {
                    MatchTitle = matchTitle,
                    Team1 = team1,
                    Team2 = team2,
                    MatchStatus = MatchStatus,
                    MatchTime = MatchTime,
                });
            }
           
        }      
        
        // Save the extracted data to a JSON file
        string jsonFilePath = "../../../Resources/Test" + DateString + ".json" ;

        // Combine MatchCompleted and MatchUpcoming into a single list
        var allMatches = MatchCompleted.Concat(MatchUpcoming).ToList();

        // Save the combined data to a JSON file
        await TestUtil.SaveObjectToJsonFileAsync(allMatches, jsonFilePath);
        
        // Display the extracted data from JSON file
        matchCardsFromJson = TestUtil.ReadObjectFromJsonFileAsync<List<CricketMatchCard>>(jsonFilePath).GetAwaiter().GetResult();
        DisplayMatchCards(matchCardsFromJson);

    }
    
    public void DisplayMatchCards(List<CricketMatchCard> matchCards)
    {
        Console.WriteLine("🏏 Recent Matches on ESPN Cricinfo:\n");

        int i =0;
        foreach (var card in matchCards)
        {                  
           i++;
            Console.WriteLine("-----------------------------");
            Console.WriteLine($"Match {i}: {card.MatchTitle}");
            Console.WriteLine($"Teams: {card.Team1} vs {card.Team2}");            
            Console.WriteLine($"Match Status: {card.MatchStatus}"); 
            if(card.MatchTime == null) 
            Console.WriteLine($"Score of team 1, 2: {card.Score1} vs {card.Score2}");
            else
            Console.WriteLine($"Match Time: {card.MatchTime}");
            Console.WriteLine("-----------------------------");
           
        }
    }  

     
    
}


public class CricketMatchCard
{
    public string MatchTitle { get; set; }           // e.g., "33rd Match • IPL • T20 • Wankhede"
    
    public string Team1 { get; set; }                // e.g., "Mumbai Indians"
    public string Team2 { get; set; }                // e.g., "Sunrisers Hyderabad"
    
    public string Score1 { get; set; }               // e.g., "188/5"
    public string Score2 { get; set; }               // e.g., "188/4"
    
    public string Extras { get; set; }               // e.g., "(20 ov, T:189)"
    public string ResultText { get; set; }           // e.g., "Match tied (DC won the Super Over)"
    
    public string MatchStatus { get; set; }          // e.g., "RESULT", "INNINGS BREAK", "Match starts in 5 hrs"
    public string MatchTime { get; set; }            // e.g., "7:30 PM" (if live/upcoming)
    
}


//dump this conten in json ,