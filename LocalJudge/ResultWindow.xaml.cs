using LocalJudge.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using WatiN.Core;

namespace LocalJudge
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        const string prefix = "http://www.udebug.com/uva/";
        static string textInput, textOutput, textCorrect;
        public static string programOutput = null;
        IE browser;
        int timeLimit;
        string idProblem;
        public ResultWindow(int timeLimit , string idProblem) : this()
        {
            this.timeLimit = timeLimit;
            this.idProblem = idProblem;
            Process process = new Process();
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = "cmd";
            process.StartInfo.Arguments =
                string.Format("/C \"\"{0}\" < \"{1}\" > \"{2}\"\"",
                Properties.Settings.Default.FileExec,
                Properties.Settings.Default.FileInput,
                Properties.Settings.Default.FileOutput);
            process.Start();
            if (!process.WaitForExit(timeLimit * 1000))
                process.Kill();
            if (process.ExitCode != 0)
            {
                runtime.Visibility = Visibility.Visible;
                return;
            }
            timeExec.Content = process.TotalProcessorTime.Milliseconds / 1000f + "s";
            try
            {
                textInput = File.ReadAllText(Properties.Settings.Default.FileInput);
                textOutput = File.ReadAllText(Properties.Settings.Default.FileOutput);
                textOutput = textOutput.Replace("\r\n", "\n");
                //Settings.MakeNewIeInstanceVisible = false;
                WatiN.Core.Settings.Instance.AutoMoveMousePointerToTopLeft = false;
                browser = new IE(prefix + idProblem);
                TextField inputField = browser.TextField(Find.ById("edit-input-data"));
                WatiN.Core.Button buttonSubmit = browser.Button(Find.ById("edit-output"));
                inputField.Value = textInput;
                buttonSubmit.Click();
                browser.WaitForComplete();
                string result = browser.Html;
                int begin = result.IndexOf("<pre>") + 5;
                textCorrect = result.Substring(begin, result.IndexOf("</pre>") - begin);
                File.WriteAllText(Properties.Settings.Default.FileCorrect, textCorrect);
                if (textOutput != textCorrect)
                    incorrect.Visibility = Visibility.Visible;
                else
                    accepted.Visibility = Visibility.Visible;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            browser.ForceClose();
        }
        public ResultWindow()
        {
            InitializeComponent();
        }
    }
}
