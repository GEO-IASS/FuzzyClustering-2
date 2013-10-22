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
using XCluster.Model;
using ZedGraphUserControls;
using Xceed.Wpf.Toolkit;
using System.Windows.Forms; 
using XCluster.ViewModel;
namespace XCluster.View
{
    /// <summary>
    /// Interaction logic for CluteringResult.xaml
    /// </summary>
    public partial class CluteringResult : Window
    {
        public ZedGraphUserControl zdg { get; set; }
        public CluteringResult()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            zdg = new ZedGraphUserControl();

            //Resize ZedGraph COntrol.
            grdHost.SizeChanged += (o, args) =>
            {
                var gr = o as WindowsFormsHost;
                zdg.Width = (int)gr.ActualWidth - 20;
                zdg.Height = (int)gr.ActualHeight - 20;
            };
            grdHost.Child = zdg;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var data = SOperation.ReadFile();
            System.Windows.MessageBox.Show(data.Count != 0 ? data[0] : "No Points!");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            zdg.ClearGraph();
            zdg.Title = "My MRGA TITLE";
            zdg.Refresh();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var alg = new CMeans(Data);
            var result = alg.GetClusters(2);
            zdg.BuildGraph(result);

        }

        public static double[][] Data =
        {
            new double[] {58, 55, 52},
            new double[] {56, 55, 52},
            new double[] {52, 50, 54},
            new double[] {1, 2, 3},
            new double[] {3, 2, 3},
            new double[] {2, 5, 3},
            new double[] {4, 3, 3},
            new double[] {6, 1, 3},
            new double[] {6, 1, 3},
            new double[] {3, 2, 1},
            new double[] {143, 123, 153},
            new double[] {133, 124, 123},
            new double[] {153, 113, 153}
        };
       
           
        
    }
}
