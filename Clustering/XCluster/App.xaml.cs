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
        public double[][] sourceImage;
        public double[][] filteredImage;
        public double[][] originalImage;
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
        public App()
        {
            int numClusters = 3;
            int maxIterations = 10;
            double accuracy = 0.04;
            List<ClusterPoint> points = new List<ClusterPoint>();
            Random random = new Random();
            foreach (var d in Data)
            {
                points.Add(new ClusterPoint(0, 0, d));
            }
            

            List<ClusterCentroid> centroids = new List<ClusterCentroid>();
            //Create random points to use a the cluster centroids
            var d3 = new double[][] { new double[] { 33.0, 55.3, 212.3 }, new double[] { 3.0, 5.3, 12.3 }, new double[] { 33.0, 25.3, 62.3 } };
            
            for (var i = 0; i < numClusters; i++)
            {
                centroids.Add(new ClusterCentroid(0, 0, d3[i]));
            }

            //Run Algoritm 
            CMeans alg = new CMeans(points, centroids, 2, Data);


            int k = 0;
            do
            {
                k++;
                alg.J = alg.CalculateObjectiveFunction();
                alg.CalculateClusterCentroids();
                alg.Step();
                double Jnew = alg.CalculateObjectiveFunction();
                if (Math.Abs(alg.J - Jnew) < accuracy) break;
                
            }
            while (maxIterations > k);

            MessageBox.Show(centroids.Count.ToString());
        }
        
    }
}
