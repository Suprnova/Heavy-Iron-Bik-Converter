using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BFBB_and_TSSM_Bik_Converter
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public Options()
        {
            InitializeComponent();
            if (((MainWindow)Application.Current.MainWindow).letterbox.Text == "1")
            {
                letterboxing.IsChecked = true;
            }
            if (((MainWindow)Application.Current.MainWindow).trim.Text == "1")
            {
                trim.IsChecked = true;
                frameStart.Text = ((MainWindow)Application.Current.MainWindow).frameStart.Text;
                frameEnd.Text = ((MainWindow)Application.Current.MainWindow).frameEnd.Text;
            }
        }

        private void trim_Checked(object sender, RoutedEventArgs e)
        {
            trimStart.IsEnabled = true;
            frameStart.IsEnabled = true;
            trimEnd.IsEnabled = true;
            frameEnd.IsEnabled = true;
        }

        private void trim_Unchecked(object sender, RoutedEventArgs e)
        {
            trimStart.IsEnabled = false;
            frameStart.IsEnabled = false;
            trimEnd.IsEnabled = false;
            frameEnd.IsEnabled = false;
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (trim.IsChecked ?? true)
            {
                ((MainWindow)Application.Current.MainWindow).trim.Text = "1";
                int fs;
                int fe;
                try
                {
                    fs = Int32.Parse(frameStart.Text);
                    fe = Int32.Parse(frameEnd.Text);
                }
                catch
                {
                    MessageBox.Show("There was an error parsing the trim parameters, are you sure they are in the right format?", "Error");
                    return;
                }
                if (fs == fe || fs > fe)
                {
                    MessageBox.Show("Your trim start can't be greater than or equal to your trim end!", "Error");
                    return;
                }
                ((MainWindow)Application.Current.MainWindow).frameStart.Text = fs.ToString();
                ((MainWindow)Application.Current.MainWindow).frameEnd.Text = fe.ToString();
            }
            else
            {
                ((MainWindow)Application.Current.MainWindow).trim.Text = "0";
                ((MainWindow)Application.Current.MainWindow).frameStart.Text = "0";
                ((MainWindow)Application.Current.MainWindow).frameEnd.Text = "0";
            }
            if (letterboxing.IsChecked ?? true)
            {
                ((MainWindow)Application.Current.MainWindow).letterbox.Text = "1";
            }
            else
            {
                ((MainWindow)Application.Current.MainWindow).letterbox.Text = "0";
            }
            Close();
        }
    }
}
