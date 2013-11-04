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
using XCluster.View;

namespace XCluster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CluteringResult windowCluteringResult;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            windowCluteringResult = new CluteringResult();
            windowCluteringResult.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            windowCluteringResult = new CluteringResult(2);
            windowCluteringResult.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            windowCluteringResult = new CluteringResult(3);
            windowCluteringResult.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var imageWindow = new ClusterImage();
            imageWindow.Show();
        }
    }
}
