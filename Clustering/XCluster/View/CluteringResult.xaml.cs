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
        public List<double[]> data = Data;
        private MainWindow parentWindow;
        private ClusterResult clusteringResult;

        public ZedGraphUserControl zdg { get; set; }
        public CluteringResult(MainWindow parent, int method = 1)
        {
            InitializeComponent();
            this.method = method;
            this.parentWindow = parent;
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            zdg = new ZedGraphUserControl();
            
            zdg.AddClickEvent(zdg_Click);
            zdg.MouseClick += zdg_Click;
            
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

        private void SaveClusterResult(object sender, RoutedEventArgs e)
        {
            SOperation.SaveResult(clusteringResult);
        }

        private void Edit_Data(object sender, RoutedEventArgs e)
        {
            var editWimdow = new DataEdit(this,data);
            editWimdow.Show();
            this.Hide();
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
                    clusteringResult = new ClusterResult(data, wpgma.GetClusters(clusterCount ?? defaultClusterCount));
                    //zdg.BuildGraph(wpgma.GetClusters(clusterCount ?? defaultClusterCount));
                    break;
                case 2:
                    var cmean = new CMeans(data);
                     clusteringResult = new ClusterResult(data, cmean.GetClusters(clusterCount ?? defaultClusterCount));
                    break;
                case 3:
                    var kmean = new KMeans(data);
                    clusteringResult = new ClusterResult(data, kmean.GetClusters(clusterCount ?? defaultClusterCount));
                    break;
            }


            var displayMode = DisplayMode.SelectedIndex;
            switch (displayMode)
            {
                case 0:
                    if (clusteringResult.Clusters != null)
                    zdg.BuildGraph(clusteringResult.Clusters);
                    break;
                case 1:
                    if (clusteringResult.FuzzyClusters != null)
                    zdg.BuildGraph(clusteringResult.FuzzyClusters);
                    break;
                case 2:
                    if (clusteringResult.FuzzyClusters != null)
                        zdg.BuildDendrogram(clusteringResult.FuzzyClusters);
                    break;
            } 
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            parentWindow.Show();
            this.Closed -= Window_Closed;
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            parentWindow.Close();
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
