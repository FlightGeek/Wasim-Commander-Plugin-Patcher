using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Wasim_Plugin_Patcher
{
    public partial class MainWindow : Window
    {
        private string flightSim = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void VerifyBut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string simFolder = string.IsNullOrEmpty(flightSim)
                ? "Microsoft Flight Simulator"
                : $"Microsoft Flight Simulator {flightSim}";

            string path = Path.Combine(userPath, simFolder, "Packages", "Community", "wasimcommander-module");
            Debug.WriteLine(path);

            if (Directory.Exists(path))
            {
                MessageBox.Show("The Plugin was already installed.", "Verification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("The Plugin does not exist.", "Verification", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FS20But_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            flightSim = ""; // FS2020
            FS20But.Visibility = Visibility.Collapsed;
            FS20Sel.Visibility = Visibility.Collapsed;
            FS24But.Visibility = Visibility.Visible;
            FS24Sel.Visibility = Visibility.Visible;
            Debug.WriteLine("FS20But clicked");
        }

        private void FS24But_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            flightSim = "2024"; // FS2024
            FS24But.Visibility = Visibility.Collapsed;
            FS24Sel.Visibility = Visibility.Collapsed;
            FS20But.Visibility = Visibility.Visible;
            FS20Sel.Visibility = Visibility.Visible;
            Debug.WriteLine("FS24But clicked");
        }

        private void InsertBut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string simFolder = string.IsNullOrEmpty(flightSim)
                ? "Microsoft Flight Simulator"
                : $"Microsoft Flight Simulator {flightSim}";

            string destinationPath = Path.Combine(userPath, simFolder, "Packages", "Community", "wasimcommander-module");

            if (Directory.Exists(destinationPath))
            {
                MessageBox.Show("The Plugin is already installed.", "Insert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string sourcePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"wasimcommander-module");


            if (!Directory.Exists(sourcePath))
            {
                MessageBox.Show("Source plugin folder not found.", "Insert", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                CopyDirectory(sourcePath, destinationPath);
                MessageBox.Show("Plugin successfully installed.", "Insert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during installation: {ex.Message}", "Insert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            foreach (var dir in Directory.GetDirectories(sourceDir))
            {
                string destSubDir = Path.Combine(destDir, Path.GetFileName(dir));
                CopyDirectory(dir, destSubDir);
            }
        }

        private void RemoveBut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string simFolder = string.IsNullOrEmpty(flightSim)
                ? "Microsoft Flight Simulator"
                : $"Microsoft Flight Simulator {flightSim}";

            string path = Path.Combine(userPath, simFolder, "Packages", "Community", "wasimcommander-module");
            Debug.WriteLine(path);

            if (!Directory.Exists(path))
            {
                MessageBox.Show("The Plugin is not installed.", "Remove", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Directory.Delete(path, true);
                MessageBox.Show("Plugin successfully removed.", "Remove", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during removal: {ex.Message}", "Remove", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
