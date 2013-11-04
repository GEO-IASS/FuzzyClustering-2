using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encog.ML;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Kmeans;

namespace XCluster.Model
{
    class KMeans
    {
        public List<double[]> Data { get; private set; }
        public KMeans(List<double[]> data)
        {
            this.Data = data;
        }

        public List<List<double[]>> GetClusters(int n)
        {
            var set = new BasicMLDataSet();

            foreach (var obj in Data)
            {
                set.Add(new BasicMLData(obj));
            }

            var kmeans = new KMeansClustering(n, set);
            kmeans.Iteration();
           

            return Convert(kmeans.Clusters);
        }

        private static List<List<double[]>> Convert(IEnumerable<IMLCluster> data)
        {
            var result = data
                            .Select(cluster => cluster
                                .Data.Select(point => point.Data)
                                .ToList())
                            .ToList();

            return result;
        }
         
    }
}
