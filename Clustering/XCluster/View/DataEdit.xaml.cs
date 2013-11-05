using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XCluster.ViewModel;

namespace XCluster.View
{
    /// <summary>
    /// Interaction logic for DataEdit.xaml
    /// </summary>
    public partial class DataEdit : Window
    {
        public List<double[]> Data;
        private MainWindow parentWindow;
        private DataTable dataBinding;

        public DataEdit(MainWindow parent)
        {
            InitializeComponent();
            Data = new List<double[]>();
            this.parentWindow = parent;
            myDG.DataSource = dataBinding;
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Data = SOperation.ReadFile() ?? new List<double[]>();
            myDG.DataSource = SetTable(Data);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SOperation.WriteFile(GetTable());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            parentWindow.Close();
        }

        private static DataTable SetTable(List<double[]> data)
        {
            var result = new DataTable();
            if (data.FirstOrDefault() == null)
            {
                result.Columns.Add("Властивість 1", typeof(double));
                result.Columns.Add("Властивість 2", typeof(double));
                return result;
            }

            for (var i = 0; i < data[0].Length; i++)
            {
                result.Columns.Add("Властивість " + (i + 1), typeof(double));
            }

            for (var i = 0; i < data.Count; i++)
            {
                result.Rows.Add();
                for (var j = 0; j < data[i].Length; j++)
                    result.Rows[i][j] = data[i][j];
            }

            return result;
        }


        private List<double[]> GetTable()
        {
            var result = new List<double[]>();

            foreach (DataGridViewRow row in myDG.Rows)
            {
                try
                {
                    var tmp = new double[row.Cells.Count];
                    for (var i = 0; i < row.Cells.Count; i++)
                    {
                        tmp[i] = Double.Parse(row.Cells[i].Value.ToString());
                    }
                    result.Add(tmp);
                }
                catch 
                {
                }
            }

            return result;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            parentWindow.Show();
            this.Closed -= Window_Closed;
            this.Close();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            myDG.Columns.Add("Властивість " + (myDG.Columns.Count + 1), "Властивість " + (myDG.Columns.Count + 1));
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (myDG.Columns.Count < 1) return;
            myDG.Columns.Remove("Властивість " + (myDG.Columns.Count));
        }

        private void Menu_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Menu_Save(object sender, RoutedEventArgs e)
        {
            SOperation.WriteFile(Data);
        }

        private void Menu_Open(object sender, RoutedEventArgs e)
        {
            SOperation.ReadFile();
        }

    }
}
