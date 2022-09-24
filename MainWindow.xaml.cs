using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Animation;
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

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            Options options = new Options();
            options.Owner = this;
            options.ShowDialog();
        }

        /*private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            volume.Text = slider.Value.ToString();
        }*/

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            AllocConsole();
            string scale;
            if ((bool)bfbb.IsChecked || (bool)scoob.IsChecked)
            {
                scale = "640:480";
            }
            else if ((bool)tssm.IsChecked || (bool)rotu.IsChecked)
            {
                scale = "512:480";
            }
            else
            {
                //must be an incredibles target
                scale = "512:448";
            }
            Console.WriteLine("Converting file to avi...");
            string filename = Path.GetFileNameWithoutExtension(directory.Text);
            string aviName = filename + ".avi";
            string path = Path.GetDirectoryName(directory.Text);
            //int volumeInt = Int32.Parse(volume.Text);
            // Process.Start("cmd.exe", $"/C cd {Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))}&ffmpeg -i \"{directory.Text}\" -filter:a \"volume = {volumeInt/100}\" -vf scale={scale} {aviName}").WaitForExit();
            if (letterbox.Text == "1")
            {
                if (trim.Text == "1")
                {
                    int fs = Int32.Parse(frameStart.Text);
                    int fe = Int32.Parse(frameEnd.Text);
                    TimeSpan ts = TimeSpan.FromSeconds(fs);
                    TimeSpan te = TimeSpan.FromSeconds(fe);
                    string trimStart = ts.ToString(@"mm\:ss");
                    string trimEnd = te.ToString(@"mm\:ss");
                    Process.Start("cmd.exe", $"/C cd {Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}&ffmpeg -i \"{directory.Text}\" -vf \"scale ={scale}:force_original_aspect_ratio = decrease,pad ={scale}:-1:-1:color = black\" -ss {trimStart} -to {trimEnd} {Path.Combine(path, aviName)}").WaitForExit();
                }
                else
                {
                    Process.Start("cmd.exe", $"/C cd {Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}&ffmpeg -i \"{directory.Text}\" -vf \"scale ={scale}:force_original_aspect_ratio = decrease,pad ={scale}:-1:-1:color = black\" {Path.Combine(path, aviName)}").WaitForExit();
                }
            }
            else
            {
                if (trim.Text == "1")
                {
                    int fs = Int32.Parse(frameStart.Text);
                    int fe = Int32.Parse(frameEnd.Text);
                    TimeSpan ts = TimeSpan.FromSeconds(fs);
                    TimeSpan te = TimeSpan.FromSeconds(fe);
                    string trimStart = ts.ToString(@"mm\:ss");
                    string trimEnd = te.ToString(@"mm\:ss");
                    Process.Start("cmd.exe", $"/C cd {Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}&ffmpeg -i \"{directory.Text}\" -vf scale={scale} -ss {trimStart} -to {trimEnd} {Path.Combine(path, aviName)}").WaitForExit();
                }
                else
                {
                    Process.Start("cmd.exe", $"/C cd {Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}&ffmpeg -i \"{directory.Text}\" -vf scale={scale} {Path.Combine(path, aviName)}").WaitForExit();
                }          
            }
            Console.WriteLine("Conversion successful!");
            
            string rlst = Path.Combine(path, DateTime.Now.Ticks + ".rlst");
            File.WriteAllText(rlst, $"cd {path} \nBinkc \"{aviName}\" \"{aviName.Replace(".avi", ".bik")}\"  /v100 /d0 /m3.0 /l4 /p8 /O");
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
            File.Delete(Path.Combine(path, aviName));
            File.Delete(rlst);
            try
            {
                Process[] proc = Process.GetProcessesByName("radbatch");
                proc[0].Kill();
            }
            catch { }
            Process.Start("explorer.exe", "/select, \"" + Path.Combine(path, aviName.Replace(".avi", ".bik")) + "\"");
            Environment.Exit(1);
        }
    }
}
