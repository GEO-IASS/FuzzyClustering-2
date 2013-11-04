using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XCluster.Model
{
    public static class Distance
    {
        private  static  readonly Lazy<StringCollection> _lazyInitializer = new Lazy<StringCollection>(() => 
                                                                                        new StringCollection(){
                                                                                            "Лінійна відстань",
                                                                                            "Евклідова відстань",
                                                                                            "Квадратична евклідова відстань",
                                                                                            "Відстань Мінковського"
                                                                                        });

        public static StringCollection DistanceList
        {
            get
            {
                return _lazyInitializer.Value;
            }
        }



        public delegate List<double[]> GetDistanceDelegat(List<double[]> data);

        public static List<double[]> GetEuclideanDistance(List<double[]> data)
        {
            var count = data.Count;
            var dimentionCount = data[0].Length;
            var result = new List<double[]>();
            double sum = 0;

            data = GetNormal(data);
            for (var i = 0; i < count; i++)
            {
                var tmp = new double[count];
                for (var j = 0; j < count; j++)
                {
                    for (var k = 0; k < dimentionCount; k++)
                        sum += (data[i][k] - data[j][k]) * (data[i][k] - data[j][k]);

                    tmp[j] = Math.Sqrt(sum);
                    sum = 0;
                }
                result.Add(tmp);
            }
            return result;
        }

        public static List<double[]> GetSquqreEuclideanDistance(List<double[]> data)
        {
            var count = data.Count;
            var dimentionCount = data[0].Length;
            var result = new List<double[]>();
            double sum = 0;
            data = GetNormal(data);
            for (var i = 0; i < count; i++)
            {
                var tmp = new double[count];
                for (var j = 0; j < count; j++)
                {
                    for (var k = 0; k < dimentionCount; k++)
                        sum += (data[i][k] - data[j][k]) * (data[i][k] - data[j][k]);

                    tmp[j] = Math.Sqrt(sum);
                    sum = 0;
                }
                result.Add(tmp);
            }
            return result;
        }

        public static List<double[]> GetLinearDistance(List<double[]> data)
        {
            var count = data.Count;
            var dimentionCount = data[0].Length;
            var result = new List<double[]>();
            double sum = 0;

            data = GetNormal(data);
            for (var i = 0; i < count; i++)
            {
                var tmp = new double[count];
                for (var j = 0; j < count; j++)
                {
                    for (var k = 0; k < dimentionCount; k++)
                        sum += Math.Abs(data[i][k] - data[j][k]);

                    tmp[j] = sum;
                    sum = 0;
                }
                result.Add(tmp);
            }
            return result;
        }

        public static List<double[]> GetMimkovskyiDistance(List<double[]> data)
        {
            var step = 3;
            var count = data.Count;
            var dimentionCount = data[0].Length;
            var result = new List<double[]>();
            double sum = 0;

            data = GetNormal(data);
            for (var i = 0; i < count; i++)
            {
                var tmp = new double[count];
                for (var j = 0; j < count; j++)
                {
                    for (var k = 0; k < dimentionCount; k++)
                        sum += Math.Pow(Math.Abs(data[i][k] - data[j][k]), step);

                    tmp[j] = Math.Pow(sum, (1.0 / step));
                    sum = 0;
                }
                result.Add(tmp);
            }
            return result;
        }

        private static List<double[]> GetNormal(List<double[]> data)
        {
            var count = data.Count;
            var dimentionCount = data[0].Length;
            var result = new double[count][];

            for (var i = 0; i < count; i++)
                result[i] = new double[dimentionCount];

            for (var i = 0; i < dimentionCount; i++)
            {
                var max = data[0][i];
                var min = data[0][i];

                for (var j = 0; j < count; j++)
                {
                    if (data[j][i] > max)
                        max = data[j][i];
                    if (data[j][i] < min)
                        min = data[j][i];
                }
                
                for (var j = 0; j < count; j++)
                {
                    result[j][i] = (max - data[j][i]) / (max - min);
                }
            }
            return result.ToList();
        }
    }
}
