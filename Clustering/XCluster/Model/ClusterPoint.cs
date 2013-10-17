using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace XCluster.Model
{
    public class ClusterPoint
    {
        public string Name { get; set; }

        public double[] Cluster { get; set; }

        /// <summary>
        /// Gets or sets X-coord of the point
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets Y-coord of the point
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the RGB color of the point
        /// </summary>
        public double[] Data { get; set; }

        /// <summary>
        /// Gets or sets the original RGB color of the point
        /// </summary>
        public double[] OriginalData { get; set; }

        /// <summary>
        /// Gets or sets cluster index, the strongest membership value to a cluster, used for defuzzification
        /// </summary>
        public double ClusterIndex { get; set; }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="x">X-coord</param>
        /// <param name="y">Y-coord</param>
        /// <param name="z">Data</param>
        /// <param name="z">OriginalData</param>
        /// <param name="z">ClusterIndex</param>
        public ClusterPoint(double[] data)
        {
            this.Data = data;
            this.OriginalData = data;
            this.ClusterIndex = -1;
        }

        public ClusterPoint(double[] data, double x, double y):this(data)
        {
            this.X = x;
            this.Y = y;
        }

        public ClusterPoint(double[] data, string name): this(data)
        {
            this.Name = name;
        }

        public ClusterPoint(double[] data, double x, double y, string name)
            : this(data)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
        }
    }
}
