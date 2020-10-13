using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using Ookii.Dialogs.Wpf;

namespace BFBB_and_TSSM_Bik_Converter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static class Globals
        {
            public static string path;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var browseDialog = new VistaOpenFileDialog();
            if (browseDialog.ShowDialog() == true)
            {
                directory.Text = browseDialog.FileName.ToString();
            }
        }

        /*private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            volume.Text = slider.Value.ToString();
        }*/

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Hide();
            AllocConsole();
            string scale;
            if ((bool)bfbb.IsChecked)
            {
                scale = "640:480";
            }
            else
            {
                // must be a movie target
                scale = "512:480";
            }
            string aviName = DateTime.Now.Ticks + ".avi";
            Console.WriteLine("Converting file to avi...");
            string filename = directory.Text.Substring(directory.Text.LastIndexOf('\\') + 1);
            //int volumeInt = Int32.Parse(volume.Text);
            // Process.Start("cmd.exe", $"/C cd {Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))}&ffmpeg -i \"{directory.Text}\" -filter:a \"volume = {volumeInt/100}\" -vf scale={scale} {aviName}").WaitForExit();
            Console.WriteLine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            Process.Start("cmd.exe", $"/C cd {Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}&ffmpeg -i \"{directory.Text}\" -vf scale={scale} {aviName}").WaitForExit();
            Console.WriteLine("Conversion successful!");
            
            string path = directory.Text.Remove(directory.Text.LastIndexOf('\\'));
            string rlst = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), DateTime.Now.Ticks + ".rlst");
            File.WriteAllText(rlst, $"cd {Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)} \nBinkc \"{aviName}\" \"{aviName.Replace(".avi", ".bik")}\"  /v100 /d0 /m3.0 /l4 /p8 /O");
            Process.Start(new ProcessStartInfo(rlst) { UseShellExecute = true });
            Process[] processlist = Process.GetProcessesByName("binkc");
            do
            {
                processlist = Process.GetProcessesByName("binkc");
            }
            while (processlist.Length == 0);
            Process[] processlistC = Process.GetProcessesByName("binkc");
            do
            {
                processlistC = Process.GetProcessesByName("binkc");
            }
            while (processlistC.Length >= 1);
            Console.WriteLine("Conversion to .bik complete!");
            File.Delete(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), aviName));
            File.Delete(rlst);
            Process.Start("explorer.exe", "/select, \"" + Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), aviName.Replace(".avi", ".bik")) + "\"");
            Environment.Exit(1);
        }

    }
}
