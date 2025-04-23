
using System.Threading.Tasks;

namespace PlaywrightPractice.TestScripts;

public class PracticeLogic
{


    [Test, Category("Execute the logic in this method")]
    public void LogicExecution()
    {
        Method01();
        Method02();
        Console.WriteLine("Logic Execution Completed");
    }

    public void Method01()
    {
        Console.WriteLine("Started Method 01");
        Thread.Sleep(2000);
        Console.WriteLine("Completed Method 01");

    }

    public async void Method02()
    {
        Console.WriteLine("Started Method 02");
        await DelayM();
        Console.WriteLine("Completed Method 02");  

    }


    public async Task DelayM()
    {
       // Console.WriteLine("Delay async method got Started");
        await Task.Delay(TimeSpan.FromSeconds(14));
        Console.WriteLine("Delay async method got completed");
    }

    
}