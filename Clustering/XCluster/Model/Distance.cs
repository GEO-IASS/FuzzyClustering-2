using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static Double[][] GetEuclideanDistance(double[][] data)
        {
            var count = data.Length;
            var dimentionCount = data[0].Length;
            var result = new double[count][];
            double sum = 0;

            data = GetNormal(data); ;
            for (var i = 0; i < count; i++)
            {
                result[i] = new double[count];
                for (var j = 0; j < count; j++)
                {
                    for (var k = 0; k < dimentionCount; k++)
                        sum += (data[i][k] - data[j][k]) * (data[i][k] - data[j][k]);

                    result[i][j] = Math.Sqrt(sum);
                    sum = 0;
                }
            }
            return result;
        }

        public static Double[][] GetSquqreEuclideanDistance(double[][] data)
        {
            var count = data.Length;
            var dimentionCount = data[0].Length;
            var result = new double[count][];
            double sum = 0;
            data = GetNormal(data);
            for (var i = 0; i < count; i++)
            {
                result[i] = new double[count];
                for (var j = 0; j < count; j++)
                {
                    for (var k = 0; k < dimentionCount; k++)
                        sum += (data[i][k] - data[j][k]) * (data[i][k] - data[j][k]);

                    result[i][j] = Math.Sqrt(sum);
                    sum = 0;
                }
            }
            return result;
        }

        public static Double[][] GetLinearDistance(Double[][] data)
        {
            var count = data.Length;
            var dimentionCount = data[0].Length;
            var result = new double[count][];
            double sum = 0;

            data = GetNormal(data);
            for (var i = 0; i < count; i++)
            {
                result[i] = new double[count];
                for (var j = 0; j < count; j++)
                {
                    for (var k = 0; k < dimentionCount; k++)
                        sum += Math.Abs(data[i][k] - data[j][k]);

                    result[i][j] = sum;
                    sum = 0;
                }
            }
            return result;
        }

        public static Double[][] GetMimkovskyiDistance(double[][] data, int step)
        {
            var count = data.Length;
            var dimentionCount = data[0].Length;
            var result = new double[count][];
            double sum = 0;

            data = GetNormal(data);
            for (var i = 0; i < count; i++)
            {
                result[i] = new double[count];
                for (var j = 0; j < count; j++)
                {
                    for (var k = 0; k < dimentionCount; k++)
                        sum += Math.Pow(Math.Abs(data[i][k] - data[j][k]), step);

                    result[i][j] = Math.Pow(sum, (1.0 / step));
                    sum = 0;
                }
            }
            return result;
        }

        private static Double[][] GetNormal(double[][] data)
        {
            var count = data.Length;
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
            return result;
        }
    }
}
