using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

internal class GetLink
{
    private static readonly HttpClient _client = new HttpClient();

    public static bool log = true;
    public static async Task<string[]> YoutubeVideoLink(string request, string filter = "all")
    {
        request = HttpUtility.UrlEncode(request);
        string url = filter switch
        {
            "week" => $"https://www.youtube.com/results?search_query={request}&sp=EgIIAw%253D%253D",
            "month" => $"https://www.youtube.com/results?search_query={request}&sp=EgQIBBAB",
            "year" => $"https://www.youtube.com/results?search_query={request}&sp=EgQIBRAB",
            _ => $"https://www.youtube.com/results?search_query={request}"
        };

        string htmlCode = await _client.GetStringAsync(url);

        // Используем более простое регулярное выражение и ограничиваем поиск
        string pattern = @"watch\?v=[\w-]{11}";
        MatchCollection matches = Regex.Matches(htmlCode, pattern);

        return matches.Cast<Match>()
                      .Select(m => "https://www.youtube.com/" + m.Value)
                      .Distinct()
                      .ToArray();
    }
    public static async Task<string[]> LinkParsing(string videoTag, int linksCount)
    {

        string[] youtubeLinks = await GetLink.YoutubeVideoLink(videoTag);
        List<string> vkVideoLinks = new List<string>();
        if(log == true)
        {
            for (int i = 0; i < youtubeLinks.Length && i < linksCount; i++)
            {
                Console.WriteLine("Video: " + youtubeLinks[i]);
                string[] vkLinks = await GetLink.VKlink(youtubeLinks[i]);

                if (vkLinks.Length > 0)
                {
                    foreach (string vkLink in vkLinks)
                    {
                        Console.WriteLine("\tVK: " + vkLink);
                        vkVideoLinks.Add(vkLink);
                    }
                }
                else
                {
                    Console.WriteLine("\tVK: Not found");
                }
            }
        }
        if(log == false)
        {
            for (int i = 0; i < youtubeLinks.Length && i < linksCount; i++)
            {
                string[] vkLinks = await GetLink.VKlink(youtubeLinks[i]);

                if (vkLinks.Length > 0)
                {
                    foreach (string vkLink in vkLinks)
                    {
                        Console.WriteLine("\tVK: " + vkLink);
                        vkVideoLinks.Add(vkLink);
                    }
                }
            }
        }
        return vkVideoLinks.ToArray();
    }

    public static async Task<string[]> VKlink(string startLink)
    {
        try
        {
            string htmlCode = await _client.GetStringAsync(startLink);

            // Ограничиваем поиск по регулярному выражению
            string pattern = @"https:\/\/vk\.com\/[\w\d_-]+";
            MatchCollection matches = Regex.Matches(htmlCode, pattern, RegexOptions.Singleline);

            return matches.Cast<Match>()
                          .Select(m => m.Value.Trim())
                          .Distinct()
                          .ToArray();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return Array.Empty<string>();
        }
    }
}

class Program
{
    async Task Main(string[] args)
    {
        GetLink.log = true;
        string videoTag = Console.ReadLine();
        int linksCount = int.Parse(Console.ReadLine());
        string[] URL = GetLink.LinkParsing(videoTag, linksCount).GetAwaiter().GetResult();
        string message = "Здравствуйте, хочу предложить свою помощь в создании превью для видео, для примера прикрепил портфолио. Если вас заинтересовало мое предложение, можем начать сотрудничать!";
        string photoURL = "clck.ru/3CLpuv";
        Console.ReadLine();
        SpamBot.BotLaunch(URL, message, photoURL);
    }
}

class SpamBot
{
    public static void BotLaunch(string[] URL, string message, string photoURL)
    {
        using (WebDriver driver = new EdgeDriver())
        {
            Console.WriteLine("Browser connection is open");
            driver.Navigate().GoToUrl("https://id.vk.com");
            Console.WriteLine("Press any key to start spam");
            Console.ReadKey();

            foreach (string url in URL)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Page ({url}) launch successful");
                Console.ForegroundColor = ConsoleColor.Gray;
                driver.Navigate().GoToUrl(url);

                try
                {
                    driver.FindElement(By.CssSelector("span.vkuiButton__in > span.vkuiButton__before")).Click();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Element 'User's private messages' successfully found");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Element 'User's private messages' not found, trying another...");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    try
                    {
                        driver.FindElement(By.CssSelector("a.FlatButton.FlatButton--secondary.FlatButton--size-m.redesigned-group-action.redesigned-group-action--short")).Click();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Element 'Private messages of the community' successfully found");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Element 'Private messages of the community' not found. Skipping.");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        continue;
                    }
                }

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                try
                {
                    var messageBox = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("mail_box_editable")));
                    messageBox.SendKeys(message + "\n" + photoURL);
                    driver.FindElement(By.Id("mail_box_send")).Click();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Message Successfully Sent");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The private message element is overlapped. Skipping.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }

            Console.WriteLine("Browser connection closed");
        }
    }
}
