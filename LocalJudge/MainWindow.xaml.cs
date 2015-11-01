using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.IO;
using WatiN.Core;
using System.Threading;
using System.Windows.Threading;

namespace LocalJudge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dirExecute.Text = Properties.Settings.Default.FileExec;
            dirInput.Text = Properties.Settings.Default.FileInput;
            dirOutput.Text = Properties.Settings.Default.FileOutput;
            dirCorrect.Text = Properties.Settings.Default.FileCorrect;
        }

        private void Click_FileExecute(object sender, RoutedEventArgs e)
        {
            BrowseFile(dirExecute, ".exe");
            Properties.Settings.Default.FileExec = dirExecute.Text;
            Properties.Settings.Default.Save();
        }

        private void Click_FileInput(object sender, RoutedEventArgs e)
        {
            BrowseFile(dirInput, ".txt");
            Properties.Settings.Default.FileInput = dirInput.Text;
            Properties.Settings.Default.Save();
        }
        private void Click_FileOutputProgram(object sender, RoutedEventArgs e)
        {
            BrowseFile(dirOutput, ".txt");
            Properties.Settings.Default.FileOutput = dirOutput.Text;
            Properties.Settings.Default.Save();
        }
        private void Click_FileOutputUdebug(object sender, RoutedEventArgs e)
        {
            BrowseFile(dirCorrect, ".txt");
            Properties.Settings.Default.FileCorrect = dirCorrect.Text;
            Properties.Settings.Default.Save();
        }
        private void BrowseFile(TextBox textbox, string defaultExtension)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.DefaultExt = defaultExtension;
            bool? result = fileDialog.ShowDialog();
            if (result == true)
                textbox.Text = fileDialog.FileName;
        }
        private void Click_Execute(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(dirExecute.Text))
                MessageBox.Show("Executable must be chosen", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (string.IsNullOrEmpty(dirInput.Text))
                MessageBox.Show("Input file must be chosen", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (string.IsNullOrEmpty(probemID.Text))
                MessageBox.Show("Problem ID must be chosen", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (!isNumeric(probemID.Text))
                MessageBox.Show("Problem ID must be numeric", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (!isNumeric(timeLimit.Text))
                MessageBox.Show("Time limit must be numeric", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (!dirExecute.Text.EndsWith(".exe"))
                MessageBox.Show("Executable must end with .exe", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (timeLimit.Text.CompareTo("10") <= 0)
                MessageBox.Show("Time limit must be < 10", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (!File.Exists(Properties.Settings.Default.FileExec))
                MessageBox.Show("Executable not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (!File.Exists(Properties.Settings.Default.FileInput))
                MessageBox.Show("Input not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (!File.Exists(Properties.Settings.Default.FileOutput))
                MessageBox.Show("Output (Program) not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (!File.Exists(Properties.Settings.Default.FileCorrect))
                MessageBox.Show("Output (uDebug) not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                (new ResultWindow(timeLimit.SelectedIndex + 1 , probemID.Text)).Show();
        }
        private bool isNumeric(string text)
        {
            foreach (char character in text)
                if (character < '0' || character > '9')
                    return false;
            return true;
        }
    }
}
