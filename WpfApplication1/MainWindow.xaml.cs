using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var driverService = PhantomJSDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;

            var webDriver = new PhantomJSDriver(driverService);
            webDriver.Navigate().GoToUrl("http://www.udebug.com/UVa/10812");
            IWebElement inputBox = webDriver.FindElement(By.Id("edit-input-data"));
            inputBox.SendKeys("3\n2035415231 1462621774\n1545574401 1640829072\n2057229440 1467906174");
            IWebElement submitButton = webDriver.FindElement(By.Id("edit-output"));
            submitButton.Click();
            string answer = webDriver.PageSource;
            int begin = answer.IndexOf("<pre>") + 5;
            answer = answer.Substring(begin, answer.IndexOf("</pre>") - begin);
            webDriver.Close();
            MessageBox.Show(answer);
        }
    }
}
