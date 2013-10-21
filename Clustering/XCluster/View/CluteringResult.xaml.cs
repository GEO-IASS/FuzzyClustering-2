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
using System.Windows.Shapes;
using System.Windows.Forms.Integration;
using ZedGraphUserControls;
using Xceed.Wpf.Toolkit;
using System.Windows.Forms; 

namespace XCluster.View
{
    /// <summary>
    /// Interaction logic for CluteringResult.xaml
    /// </summary>
    public partial class CluteringResult : Window
    {
        public CluteringResult()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            var host = new WindowsFormsHost();
            var graph = new ZedGraphUserControl();
            host.Child = graph;
            grdMain.Children.Add(host); 
        }
    }
}
