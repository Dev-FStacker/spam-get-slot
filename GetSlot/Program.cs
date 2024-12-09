using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace GetSlot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChromeOptions options = new ChromeOptions();
            options.DebuggerAddress = "127.0.0.1:9222";

            IWebDriver driver = new ChromeDriver(options);

            try
            {
                Console.WriteLine("Attaching to the existing browser session...");

                string targetUrl = "https://fap.fpt.edu.vn/FrontOffice/MoveSubject.aspx?id=48494";
                if (driver.Url != targetUrl)
                {
                    driver.Navigate().GoToUrl(targetUrl);
                }

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

                while (true)
                {

                    try
                    {
                        IWebElement saveButton = wait.Until(driver =>
                        {
                            IWebElement element = driver.FindElement(By.Id("ctl00_mainContent_btSave"));
                            return (element.Displayed && element.Enabled) ? element : null;
                        });

                        saveButton.Click();
                        Console.WriteLine("Save button clicked.");

                        // Wait for the alert to appear
                        IAlert alert = wait.Until(driver =>
                        {
                            try
                            {
                                return driver.SwitchTo().Alert();
                            }
                            catch (NoAlertPresentException)
                            {
                                return null;
                            }
                        });

                        if (alert != null)
                        {
                            Console.WriteLine($"Alert text: {alert.Text}");
                            alert.Accept();
                            Console.WriteLine("Alert accepted.");
                        }
                    }
                    catch (WebDriverTimeoutException ex)
                    {
                        Console.WriteLine($"Timeout waiting for element or alert: {ex.Message}");
                    }
                    catch (NoSuchElementException ex)
                    {
                        Console.WriteLine($"Element not found: {ex.Message}");
                    }

                    // Optional: Wait briefly before the next iteration
                    System.Threading.Thread.Sleep(2000);
                }

                Console.WriteLine("Process completed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                // Leave the browser session open
                Console.WriteLine("Automation completed.");
            }
        }
    }
}
