using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using XCluster.Model;

namespace XCluster
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
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
            new double[] {143, 123, 153},
            new double[] {133, 124, 123},
            new double[] {153, 113, 153},
            new double[] {6, 1, 3},
            new double[] {3, 2, 1}
        };
        public App()
        {
            var c = new WPGMA(Data);
            var result = c.GetClusters(2);
            MessageBox.Show(c.ClusteringMark.ToString());
        }
        
    }
}
