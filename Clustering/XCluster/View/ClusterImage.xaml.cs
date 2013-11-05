using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using XCluster.Model;
using XCluster.ViewModel;

namespace XCluster.View
{
    /// <summary>
    /// Interaction logic for ClusterImage.xaml
    /// </summary>
    public partial class ClusterImage : Window
    {
        public Bitmap sourceImage;
        public Bitmap filteredImage;
        public Bitmap originalImage;
        private MainWindow parentWindow;

        private BackgroundWorker backgroundWorker;
        public Stopwatch stopWatch;

        public ClusterImage(MainWindow parent)
        {
            InitializeComponent();
            this.parentWindow = parent;
            backgroundWorker = new BackgroundWorker();
            stopWatch = new Stopwatch();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += backgroundWorker1_DoWork;
            backgroundWorker.RunWorkerCompleted += (sender, args) =>
                                                        {
                                                            StartClustering.IsEnabled = true;
                                                            UploadImage.IsEnabled = true;
                                                            StopClustering.IsEnabled = false;
                                                        };
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenImage();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StartClustering.IsEnabled = false;
            UploadImage.IsEnabled = false;
            StopClustering.IsEnabled = true;
            stopWatch.Reset();
            stopWatch.Start();
            backgroundWorker.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            filteredImage = (Bitmap) sourceImage.Clone();
            var clusterCount = 2;
            var maxIterations = 10;
            var accuracy = 0.001;

            this.Dispatcher.Invoke(() =>
            {
                clusterCount = ClusterCount.Value ?? 2;
                maxIterations = IterationCount.Value ?? 10;
                accuracy = Prescision.Value ?? 0.0001;
            });
            var points = new List<ClusterPoint>();

            for (var row = 0; row < originalImage.Width; ++row)
            {
                for (var col = 0; col < originalImage.Height; ++col)
                {
                    var color = originalImage.GetPixel(row, col);
                    points.Add(new ClusterPoint(ColorToDouble(color), row, col));
                }
            }

            var fcm = new CMeans(points);
            var newImage = fcm.GetClusters(clusterCount);


            var tempImage = new Bitmap(originalImage.Width, originalImage.Height, PixelFormat.Format32bppRgb);

            for (var j = 0; j < fcm.Points.Count; j++)
            {
                for (var i = 0; i < fcm.Clusters.Count; i++)
                {
                    var p = fcm.Points[j];
                    if (Math.Abs(newImage[j][i] - p.ClusterIndex) < accuracy)
                    {
                        tempImage.SetPixel((int)p.X, (int)p.Y, DoubleToColor(fcm.Clusters[i].Data));
                    }
                }
            }

            RImage.Dispatcher.Invoke(() =>
            {
                var bitmapImage = new BitmapImage();
                var memory = new MemoryStream();
                tempImage.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.EndInit();
                RImage.Source = bitmapImage;
            });
           
        }
        
        private double[] ColorToDouble(Color color)
        {
            var result = new double[3];
            result[0] = color.R;
            result[1] = color.G;
            result[2] = color.B;

            return result;
        }

        private Color DoubleToColor(double[] data)
        {
            if (data == null || data.Length != 3)
                return Color.Red;

            var result = Color.FromArgb((int)data[0],(int)data[1], (int)data[2]);
            return result;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            parentWindow.Show();
            this.Closed -= Window_Closed;
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            parentWindow.Close();
        }

        private void Menu_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Menu_Save(object sender, RoutedEventArgs e)
        {
            OpenImage();
        }

        private void Menu_Open(object sender, RoutedEventArgs e)
        {
            OpenImage();
        }

        private void OpenImage()
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    using (var memory = new MemoryStream())
                    {
                        var bitmap = new Bitmap(openFileDialog.FileName);
                        sourceImage = (Bitmap)bitmap.Clone();
                        originalImage = (Bitmap)bitmap.Clone();
                        bitmap.Save(memory, ImageFormat.Bmp);
                        memory.Position = 0;
                        var bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = memory;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();
                        FImage.Source = bitmapImage;
                        StartClustering.IsEnabled = true;
                    }

                }
                catch (NotSupportedException ex)
                {
                    System.Windows.Forms.MessageBox.Show("Image format is not supported: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (ArgumentException ex)
                {
                    System.Windows.Forms.MessageBox.Show("Invalid image: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Failed loading the image", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void StopClustering_Click(object sender, RoutedEventArgs e)
        {
            if (backgroundWorker != null)
            {
                backgroundWorker.CancelAsync();
            }
        }

        private void Show_Help(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            var helpWindow = new HelpWindow(this);
            helpWindow.Show();
        }

        private void Show_About(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            var helpWindow = new AboutWindow(this);
            helpWindow.Show();
        }
    }
}
