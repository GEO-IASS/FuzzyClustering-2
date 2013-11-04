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
        private int defaultClusterCount = 2;
        private int method;
        private List<double[]> data = Data;
        public ZedGraphUserControl zdg { get; set; }
        public CluteringResult(int method = 1)
        {
            this.method = method;
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            zdg = new ZedGraphUserControl();
            
            zdg.AddClickEvent(zdg_Click);
            zdg.MouseClick += zdg_Click;
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
            this.data = SOperation.ReadFile() ?? Data;
            zdg.BuildPoint(data);
            //System.Windows.MessageBox.Show(data != null ? data[0][0].ToString() : "No Points!");
        }

        private void SaveData(object sender, RoutedEventArgs e)
        {
            SOperation.WriteFile(data);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            zdg.ClearGraph();
            zdg.Title = "My MRGA TITLE";
            zdg.Refresh();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Cluster();
        }

        private void zdg_Click(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            double x, y;
            var rank = data[0].Length;
            var point = new double[rank];
            zdg.GraphPane.ReverseTransform(e.Location, out x, out y);

            for (var i = 0; i < rank; i++)
                if (i % 2 == 0)
                    point[i] = x;
                else
                    point[i] = y;
            data.Add(point);
            Cluster();

        }

        public static List<double[]> Data = new List<double[]>
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

        private void Cluster()
        {
            Distance.GetDistanceDelegat distanceHandler;
            switch (cbDistance.SelectedIndex)
            {
                case 0:
                    distanceHandler = Distance.GetLinearDistance;
                    break;
                case 1:
                    distanceHandler = Distance.GetEuclideanDistance;
                    break;
                case 2:
                    distanceHandler = Distance.GetSquqreEuclideanDistance;
                    break;
                case 3:
                    distanceHandler = Distance.GetMimkovskyiDistance;
                    break;
                default:
                    distanceHandler = Distance.GetEuclideanDistance;
                    break;
            }

            var clusterCount = ClusterCount.Value;
            switch (method)
            {
                case 1:
                    var wpgma = new WPGMA(data, distanceHandler);
                    zdg.BuildGraph(wpgma.GetClusters(clusterCount ?? defaultClusterCount));
                    break;
                case 2:
                    var cmean = new CMeans(data);
                    zdg.BuildGraph(cmean.GetClusters(clusterCount ?? defaultClusterCount));
                    break;
                case 3:
                    var kmean = new KMeans(data);
                    zdg.BuildGraph(kmean.GetClusters(clusterCount ?? defaultClusterCount));
                    break;
            }    
        }
           
        
    }
}
