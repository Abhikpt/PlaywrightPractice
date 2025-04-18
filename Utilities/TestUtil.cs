
using System.Text.Json;

namespace PlaywrightPractice.Utilities;

public class TestUtil
{
    public static string DateString = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
    public static string ProjectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
    public static string ScreenshotPath = "../../../Resources/Test" + DateString + ".jpg";
    public static string jsonFilePath = "../../../Resources/Test" + DateString + ".json" ;


   
        public static async Task SaveObjectToJsonFileAsync<T>(T data, string filePath)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                await File.WriteAllTextAsync(filePath, jsonData);
                Console.WriteLine($"✅ Data saved to JSON file: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to save data to JSON file: {ex.Message}");
            }
        }

        public static async Task<T> ReadObjectFromJsonFileAsync<T>(string filePath)
        {

            try
            {
                if (File.Exists(filePath))
                {
                    var jsonData = await File.ReadAllTextAsync(filePath);
                    var matchCardsFromJson = JsonSerializer.Deserialize<List<CricketMatchCard>>(jsonData);
                    return JsonSerializer.Deserialize<T>(jsonData);
                }
                else
                {
                    Console.WriteLine("❌ JSON file not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to read or parse JSON file: {ex.Message}");
            }

            // Return default value for T if no valid data is found
            return default;
        }
       
}
