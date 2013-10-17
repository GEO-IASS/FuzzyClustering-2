using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace XCluster.Model
{
    public class ClusterCentroid //: ClusterPoint
    {
        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="x">Centroid x-coord</param>
        /// <param name="y">Centroid y-coord</param>
        /// <param name="PixelCount">The number of pixels in the cluster, used to find the average</param>
        /// <param name="RSum">The sum of all the cluster pixels Red values, used to find the average</param>
        /// <param name="GSum">The sum of all the cluster pixels Green values, used to find the average</param>
        /// <param name="BSum">The sum of all the cluster pixels Blue values, used to find the average</param>
        /// <param name="MembershipSum">The sum of all the cluster pixels Red values, used to find the average</param>
        /// <param name="PixelColor">The sum of all the cluster pixels Red values, used to find the average</param>
        /// <param name="OriginalPixelColor">The sum of all the cluster pixels Red values, used to find the average</param>
        public double X { get; set; }
        public double Y { get; set; }
        public double PixelCount { get; set; }
        public double MembershipSum { get; set; }
        public double[] PropertiesSum { get; set; }
        public double[] Data { get; set; }
        public double[] OriginalData { get; set; }
        
        public ClusterCentroid(double[] data)
        {
            this.PropertiesSum = new double[data.Length];
            Array.ForEach(this.PropertiesSum, d => d = 0);
            this.PixelCount = 0;
            this.MembershipSum = 0;
            this.Data = data;
            this.OriginalData = data;
        }

        public ClusterCentroid(double[] data, double x, double y):this(data)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
