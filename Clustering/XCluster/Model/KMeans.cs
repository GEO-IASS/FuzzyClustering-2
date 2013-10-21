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
        public double[][] Data { get; private set; }
        public KMeans(double[][] data)
        {
            this.Data = data;
        }

        public IMLCluster[] GetClusters(int clusterCount)
        {
            var set = new BasicMLDataSet();

            int i;
            for (i = 0; i < Data.Length; i++)
            {
                set.Add(new BasicMLData(Data[i]));
            }

            var kmeans = new KMeansClustering(clusterCount, set);
            kmeans.Iteration();

            return kmeans.Clusters;
        }
         
    }
}
