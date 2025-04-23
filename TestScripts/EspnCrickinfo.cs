using System.Text.Json;
using Microsoft.Playwright;
using PlaywrightPractice.Utilities;

namespace PlaywrightPractice.TestScripts;

public class EspnCrickinfo
{
    public static string DateString = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async  Task  Test1()
    {

        List<CricketMatchCard> matchCardsdetails = new List<CricketMatchCard>();
        List<CricketMatchCard> matchCardsFromJson = new List<CricketMatchCard>();
   
        //Playwright
        using var playwright = await Playwright.CreateAsync();

        //browser
        await using var browser = await playwright.Chromium.LaunchAsync( new  BrowserTypeLaunchOptions
        {
            Headless = false
        });
       
        // create new page in browser context
        var page = await browser.NewPageAsync();
        
        //go to URl

        await page.GotoAsync("https://www.espncricinfo.com/", new PageGotoOptions
        {
            Timeout = 60000 // Set timeout to 60 seconds
        });

        Console.WriteLine($"PageTitle: { await page.TitleAsync()}"); 



        //--------------------------extracting Data --------------------------------------       
        await page.WaitForTimeoutAsync(3000); // Let dynamic content load

        var matchCardList = await page.Locator("//*[@id='main-container']/div[2]/div/div[3]/div/div/div/div/div[contains(@class,'slick-slide')]").AllAsync();
        
        Console.WriteLine($"Total Match Cards: {matchCardList.Count}");

        Console.WriteLine("🏏 Recent Matches on ESPN Cricinfo:\n");

        foreach (var card in matchCardList)
        {

          
            try
            {
            var matchTitle = string.Empty;
            try
            {

             matchTitle = await card.Locator("div.ds-truncate span.ds-text-tight-xs.ds-text-typo-mid2").InnerTextAsync();
           
            }

            catch (Exception)
            {
                Console.WriteLine("Match title not found, skipping...");
            }

            var team1 = string.Empty;
            try
            {
                team1 = await card.Locator("div.ci-team-score:nth-child(1) p").InnerTextAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("Team 1 not found, skipping...");
            }

            var team2 = string.Empty;
            try
            {
                team2 = await card.Locator("div.ci-team-score:nth-child(2) p").InnerTextAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("Team 2 not found, skipping...");
            }

            var MatchStatus = string.Empty;
            try
            {
                MatchStatus = await card.Locator("div.ds-h-3 p span").InnerTextAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("Match status not found, skipping...");
            }

            var MatchTime = string.Empty;
            try
            {
                MatchTime = await card.Locator("div.ds-text-tight-m.ds-font-bold").InnerTextAsync() ?? "N/A";
            }
            catch (Exception)
            {
                Console.WriteLine("Match time not found, skipping...");
            }
         
          
            matchCardsdetails.Add(new CricketMatchCard
            {
                MatchTitle = matchTitle,
                Team1 = team1,
                Team2 = team2,
                MatchStatus = MatchStatus,
                MatchTime = MatchTime,
                
            });
        
           
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting match data: {ex.Message}");
                continue; // Skip to the next card if there's an error
            }
        }
        
        // Save matchCardsdetails to a JSON file
        var jsonFilePath = "../../../MatchCardsDetails_" + DateString + ".json";
        try
        {
            var jsonData = JsonSerializer.Serialize(matchCardsdetails, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            await File.WriteAllTextAsync(jsonFilePath, jsonData);
            Console.WriteLine($"✅ Match card details saved to JSON file: {jsonFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Failed to save match card details to JSON file: {ex.Message}");
        }

        // Read matchCardsdetails from the JSON file and display the data
        // try
        // {
        //     if (File.Exists(jsonFilePath))
        //     {
        //         var jsonData = await File.ReadAllTextAsync(jsonFilePath);
        //         var matchCardsFromJson = JsonSerializer.Deserialize<List<CricketMatchCard>>(jsonData);

        //         if (matchCardsFromJson != null && matchCardsFromJson.Any())
        //         {
        //             Console.WriteLine("\n📂 Data extracted from JSON file:\n");
        //             DisplayMatchCards(matchCardsFromJson);
        //         }
        //         else
        //         {
        //             Console.WriteLine("❌ No data found in the JSON file.");
        //         }
        //     }
        //     else
        //     {
        //         Console.WriteLine("❌ JSON file not found.");
        //     }
        // }
        // catch (Exception ex)
        // {
        //     Console.WriteLine($"❌ Failed to read or parse JSON file: {ex.Message}");
        // }

         matchCardsFromJson = await TestUtil.ReadObjectFromJsonFileAsync<List<CricketMatchCard>>(jsonFilePath);
        DisplayMatchCards(matchCardsFromJson?? default(List<CricketMatchCard>));
        // Console.WriteLine("✅ Extraction complete.");
        


        

    }
    

    
    internal void DisplayMatchCards(List<CricketMatchCard> matchCards)
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
            Console.WriteLine($"Match Time: {card.MatchTime}");
             Console.WriteLine("-----------------------------");
        }
    }  

     
    


internal class CricketMatchCard
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

}