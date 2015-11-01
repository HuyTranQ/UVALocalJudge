using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                var stopWatch = Stopwatch.StartNew();
                DesiredCapabilities capabilities = new DesiredCapabilities();
                var driverService = ChromeDriverService.CreateDefaultService(@"E:\");
                driverService.HideCommandPromptWindow = true;
                var webDriver = new InternetExplorerDriver();
                webDriver.Navigate().GoToUrl("http://www.udebug.com/UVa/10812");
                IWebElement inputBox = webDriver.FindElement(By.Id("edit-input-data"));
                inputBox.SendKeys("3\n2035415231 1462621774\n1545574401 1640829072\n2057229440 1467906174");
                IWebElement submitButton = webDriver.FindElement(By.Id("edit-output"));
                submitButton.SendKeys("\n");
                submitButton.Click();
                string answer = webDriver.PageSource;
                int begin = answer.IndexOf("<pre>") + 5;
                answer = answer.Substring(begin, answer.IndexOf("</pre>") - begin);
                Console.WriteLine(answer);
                webDriver.Close();
                Console.WriteLine(stopWatch.ElapsedMilliseconds);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
