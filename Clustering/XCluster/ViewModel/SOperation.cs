using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


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
                result.Add(new double[tmp.Count()]);
                for (var j = 0; j < tmp.Count(); j++)
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
    }
}
