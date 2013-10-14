using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCluster.Model
{
    class WPGMA
    {
        private List<int>[] _clusters;
        public double[][] Data { get; private set; }
        public double ClusteringMark { get; private set; }
        public int ClusterCount { get; private set; }
        private double[][] distanceMatrix;

        public WPGMA(double[][] data)
        {
            this.Data = data;
        }

        private double[][] Iteration(double[][] data)
        {
            var min = data[0][1];
            var elemntCount = data.Length;
            int x = 0, y = 1;
            int m = 0, n = 0;

            //Get nearest pair of object in set.
            for (var i = 0; i < elemntCount-1; i++)
                for (var j = i+1; j < elemntCount; j++)
                    if ((data[i][j] < min) && (i != j))
                    {
                        x = i;
                        y = j;
                        min = data[i][j];
                    }

            //Create new massive of elements.
            var result = new double[elemntCount - 1][];
            for (var i = 0; i < elemntCount - 1; i++)
                result[i] = new double[elemntCount - 1];

            //Calculate new weight.
            for (var i = 0; i < elemntCount; i++)
            {
                if ((i != x) && (i != y))
                {
                    for (var j = 0; j < elemntCount; j++)
                        if ((j != x) && (j != y))
                        {
                            result[m][n] = data[i][j];
                            n++;
                        }
                        else
                            if (j == x)
                            {
                                result[m][n] = (data[i][x] + data[i][y]) / 2;
                                n++;
                            }
                    m++;
                }
                if (i == x)
                {
                    for (var j = 0; j < elemntCount; j++)
                        if ((j != x) && (j != y))
                        {
                            result[m][n] = (data[x][j] + data[y][j]) / 2;
                            n++;
                        }
                        else
                            if (j == x)
                            {
                                result[m][n] = 0;
                                n++;
                            }
                    m++;
                }
                n = 0;
            }

            UnionClaster(x, y);
            return result;
        }

        public HashSet<HashSet<double[]>> GetClusters(int n)
        {
            var elemntsCount = Data.Length;
            _clusters = new List<int>[elemntsCount];
            for (var i = 0; i < elemntsCount; i++)
            {
                _clusters[i] = new List<int> { i };
            }
            distanceMatrix = Distance.GetSquqreEvklidDistance(Data);
            var tempData = distanceMatrix;
            while (_clusters.Length > n)
            {
                tempData = Iteration(tempData);
            }

            var result = new HashSet<HashSet<Double[]>>();
            foreach (var cluster in _clusters)
            {
                var clusterSet = new HashSet<Double[]>();
                foreach (var i in cluster)
                {
                    clusterSet.Add(Data[i]);
                }
                result.Add(clusterSet);
            }
            ClusteringMark = GetClusteringMark();
            ClusterCount = n;
            return result;
        }

        public HashSet<HashSet<double[]>> GetClusters()
        {
            var bestClusterCount = 1;
            var bestClusteringMark = 0.0;
            for (var i = 1; i < Data.Length + 1; i++)
            {
                GetClusters(i);
                if (!(bestClusteringMark < ClusteringMark)) continue;
                bestClusteringMark = ClusteringMark;
                bestClusterCount = i;
            }

            return GetClusters(bestClusterCount);
        }

        private void UnionClaster(int x, int y)
        {
            var clasterCount = _clusters.Length;
            var tmp = new List<int>[clasterCount - 1];
            var j = 0;
            for (var i = 0; i < clasterCount; i++)
            {
                if ((i != x) && (i != y))
                {
                    tmp[j] = new List<int>();
                    tmp[j].AddRange(_clusters[i]);
                    j++;
                }
                if (i != x) continue;
                tmp[j] = new List<int>();
                tmp[j].AddRange(_clusters[i]);
                tmp[j].AddRange(_clusters[y]);
                j++;
            }
            _clusters = tmp;
        }

        private double GetClusteringMark()
        {
            var clustersCount = _clusters.Length;
            double GS = 0;

            foreach (var cluster in _clusters)
            {
                var elementCount = cluster.Count;
                double S = 0;

                for (var i = 0; i < elementCount && elementCount > 1; i++)
                {
                    double A = 0, B = 1;
                    for (var j = 0; j < elementCount; j++)
                        A += distanceMatrix[cluster[i]][cluster[j]];
                   
                    for (var k = 0; k < distanceMatrix.Length; k++)
                        if (cluster.Contains(k) == false)
                            if (B > distanceMatrix[cluster[i]][k])
                                B = distanceMatrix[cluster[i]][k];
                    A /= elementCount - 1;
                    S += (B - A) / Math.Max(A,B);
                }
                GS += S / elementCount;
            }
            return GS / clustersCount;
        }
    }
}
