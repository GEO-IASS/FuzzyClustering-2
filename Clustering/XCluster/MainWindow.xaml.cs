using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Encog.App.Quant.Loader.OpenQuant.Data;
using XCluster.View;
using XCluster.ViewModel;

namespace XCluster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<double[]> Data; 
        private CluteringResult windowCluteringResult;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            windowCluteringResult = new CluteringResult(this);
            windowCluteringResult.Show();
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            windowCluteringResult = new CluteringResult(this,2);
            windowCluteringResult.Show();
            this.Hide();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            windowCluteringResult = new CluteringResult(this,3);
            windowCluteringResult.Show();
            this.Hide();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var imageWindow = new ClusterImage(this);
            imageWindow.Show();
            this.Hide();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var dataEditWindow = new DataEdit(this);
            dataEditWindow.Show();
            this.Hide();
        }

        private void Menu_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Menu_Save(object sender, RoutedEventArgs e)
        {
            SOperation.WriteFile(Data);
        }

        private void Menu_Open(object sender, RoutedEventArgs e)
        {
            SOperation.ReadFile();
        }

        private void Show_Help(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            var helpWindow = new HelpWindow(this);
            helpWindow.Show();
        }

        private void Show_About(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            var helpWindow = new AboutWindow(this);
            helpWindow.Show();
        }
    }
}
