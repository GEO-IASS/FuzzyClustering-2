using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using XCluster.Model;


namespace XCluster.ViewModel
{
    public static class  SOperation
    {
        public static List<double[]> ReadFile()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.Filter = "Data Files ( *.txt )| *.txt";
            openFileDialog.FilterIndex = 1;
            var input = new StringCollection();
            if (openFileDialog.ShowDialog() != DialogResult.OK) return null;

            try
            {
                var fs = openFileDialog.OpenFile();
                var sr = new StreamReader(fs);


                while (!sr.EndOfStream)
                {
                    input.Add(sr.ReadLine());
                }
            }
            catch
            {
                MessageBox.Show("Файл не знайдено");
            }

            var result = new List<double[]>();

            for (var i = 0; i < input.Count; i++)
            {
                var tmp = input[i].Split(';');
                var count = String.IsNullOrEmpty(tmp[tmp.Count() - 1]) ? tmp.Count() - 1 : tmp.Count();
                result.Add(new double[count]);
                for (var j = 0; j < count; j++)
                {
                    if (!Double.TryParse(tmp[j], out result[i][j]))
                        return null;
                }
            }

            return result;
        }

        public static void WriteFile(List<double[]> data)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files ( *.txt )|*.txt";
            saveFileDialog.Title = "Save a Text Files";
            saveFileDialog.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                using (var fs = (FileStream) saveFileDialog.OpenFile())
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        foreach (var obj in data)
                        {
                            foreach (var value in obj)
                                sw.Write(value + ";");
                            sw.WriteLine();
                        }
                    }
                }
            }
        }

        public static void SaveResult(ClusterResult result)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files ( *.txt )|*.txt";
            saveFileDialog.Title = "Save a Text Files";
            saveFileDialog.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                using (var fs = (FileStream)saveFileDialog.OpenFile())
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        if (result.Clusters != null)
                        {
                            sw.WriteLine("Чітке розбитя");
                            var i = 1;
                            foreach (var cluster in result.Clusters)
                            {
                                sw.WriteLine("Кластер № " + i);
                                foreach (var obj in cluster)
                                {
                                    sw.Write("[");
                                    foreach (var prop in obj)
                                    {
                                        sw.Write(" {0}, ",prop);
                                    }
                                    sw.Write("]");
                                    sw.WriteLine();
                                }
                                sw.WriteLine();
                                i++;
                            }
                        }

                        if (result.FuzzyClusters != null)
                        {
                            sw.Write("Нечітке розбитя");
                            var rank = result.FuzzyClusters[0].Length;
                            for (var j = 0; j < result.FuzzyClusters.Length; j++)
                            {
                                sw.Write("Об'єкт № " + (j+1) + ": ");
                                for (var k = 0; k < rank; k++)
                                    sw.Write("Кластер " + (k + 1) + " = " + result.FuzzyClusters[j][k] + "; ");
                                sw.WriteLine();
                            }
                        }
                    }
                }
            }
        }
    }
}
