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
        private List<List<List<double[]>>> clusterTree;
        public List<double[]> Data { get; private set; }
        public double ClusteringMark { get; private set; }
        public int ClusterCount { get; private set; }
        private List<double[]> distanceMatrix;
        private List<double[]> dendrogram;
        private double[] alpha;

        private Distance.GetDistanceDelegat getDistance;

        public WPGMA(List<double[]> data, Distance.GetDistanceDelegat distanceHandler)
        {
            clusterTree = new List<List<List<double[]>>>();
            alpha = new double[data.Count];
            this.Data = data;
            getDistance += distanceHandler ?? Distance.GetEuclideanDistance;
        }

        private List<double[]> Iteration(List<double[]> data)
        {
            var min = data[0][1];
            var elemntCount = data.Count;
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

            alpha[elemntCount-1] = min; 
            UnionClaster(x, y);

            return result.ToList();
        }

        public List<List<double[]>> GetClusters(int n)
        {
            var elemntsCount = Data.Count;
            _clusters = new List<int>[elemntsCount];
            for (var i = 0; i < elemntsCount; i++)
            {
                _clusters[i] = new List<int> { i };
            }
            distanceMatrix = getDistance(Data);
            var tempData = distanceMatrix;
            while (_clusters.Length > n)
            {
                tempData = Iteration(tempData);
            }

            var result = new List<List<Double[]>>();
            foreach (var cluster in _clusters)
            {
                var clusterSet = new List<Double[]>();
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

        public List<List<double[]>> GetClusters()
        {
            var bestClusterCount = 1;
            var bestClusteringMark = 0.0;
            for (var i = 1; i < Data.Count + 1; i++)
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
                   
                    for (var k = 0; k < distanceMatrix.Count; k++)
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

        public List<double[]> GetDendrogram()
        {
            var result = new List<double[]>();
            var pPoint = 1;
            for (var i = 1; i <= Data.Count; i++)
            {
                clusterTree.Add(GetClusters(i));
            }
            result.Add(new []{1.0,clusterTree[1][0].Count});
            for (var i = 2; i < Data.Count; i++)
            {
                result.Add(new[] { alpha[i], result[result.Count - 1][0] });
                for (var j = 0; j < Data[i].Count(); j++)
                {
                    if (!Data[i - 1].Contains(Data[i][j]))
                    {
                        
                    }
                }
            }
            return result;

        }
    }
}
