using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Clustering
{
    public partial class FResult : Form
    {
        public FResult()
        {
            InitializeComponent();
            Srez();

        }

        private void Srez()
        {
            for (int i = 0; i < SET.tresh.Count(); i++)
            {
                richTextBox1.Text += "\nКластер №" + (i + 1)+":\n";
                for (int j = 0; j < SET.tresh[i].Count(); j++)
                {
                    richTextBox1.Text += " " + (SET.tresh[i][j] + 1) + "{";
                    for (int l = 0; l < SET.vumir; l++)
                        richTextBox1.Text += string.Format(" {0:0.00};",SET.fset[SET.tresh[i][j]][l]);         
                    richTextBox1.Text += "};";
                }
                richTextBox1.Text += "\n";
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Text Files ( *.txt )|*.txt";
            saveFileDialog1.Title = "Save an Data Files";
            saveFileDialog1.ShowDialog();
            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(richTextBox1.Text);
                sw.Close();
                fs.Close();
            }
        }
    }
}
