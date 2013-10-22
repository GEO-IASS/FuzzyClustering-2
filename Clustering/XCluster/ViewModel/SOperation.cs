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
        public static StringCollection ReadFile()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.Filter = "Data Files ( *.txt )| *.txt";
            openFileDialog.FilterIndex = 1;
            var input = new StringCollection();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
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
            }
            return input;
        }
    }
}
