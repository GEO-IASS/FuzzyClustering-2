using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCluster.Model
{
    public class ClusterResult
    {
        public List<List<double[]>> Clusters{ get; set; }
        public double[][] FuzzyClusters{ get; set; }
        public List<double[]> Points{ get; set; }

        public ClusterResult(List<double[]> points, List<List<double[]>> clusters)
        {
            Points = points;
            Clusters = clusters;
        }

        public ClusterResult(List<double[]> points, double[][] fuzzyClusters)
        {
            Points = points;
            FuzzyClusters = fuzzyClusters;
        }

        public ClusterResult(List<double[]> points,List<List<double[]>> clusters, double[][] fuzzyClusters)
        {
            Points = points;
            Clusters = clusters;
            FuzzyClusters = fuzzyClusters;
        }

    }
}
