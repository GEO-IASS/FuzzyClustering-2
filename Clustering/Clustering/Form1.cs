using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ZedGraph;

namespace Clustering
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            domainUpDown1.SelectedIndex = 7;
            dr();
        }

        int check = 0;
        int selected_metruk = 1;
        int count_clast;

        private readonly Color[] _colors =
        { 
            Color.Blue, Color.Red, Color.YellowGreen, Color.SeaGreen, Color.LightBlue,
            Color.Yellow, Color.Purple, Color.Pink, Color.Orange, Color.Firebrick
        };

        private void button3_Click(object sender, EventArgs e)
        {
            ReadFile();
        }

        private void ReadFile()
        {
            dataGridView1.Rows.Clear();
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.Filter = "Data Files ( *.dat )| *.dat";
            openFileDialog1.FilterIndex = 1;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // создание объекта StreamReader
                    System.IO.FileStream fs = (System.IO.FileStream)openFileDialog1.OpenFile();
                    System.IO.StreamReader sr = new System.IO.StreamReader(fs);
                    string[] input;
                    double g;
                    SET.count = int.Parse(sr.ReadLine());

                    SET.vumir = int.Parse(sr.ReadLine());
                    // do

                    SET.fset = new double[SET.count][];
                    for (int i = 0; i < SET.count; i++)
                    {
                        //input = sr.ReadLine().Split();
                        // g = sr.Read();
                        SET.fset[i] = new double[SET.vumir];
                        for (int j = 0; j < SET.vumir; j++)
                            SET.fset[i][j] = double.Parse( sr.ReadLine());//float.Parse(input[j]);    MessageBox.Show(sr.ReadLine().ToString());//
                    } //while (sr.Peek() != -1); // -1 возвращается когда достигнут конец потока
                    sr.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Файл не найден");
                }
            }
            for (int i = 0; i < SET.count; i++)
            {
                dataGridView1.Rows.Add();
                for (int j = 0; j < SET.vumir; j++)
                    dataGridView1.Rows[i].Cells[j].Value = SET.fset[i][j].ToString();
            }

            GetDat();
            dr();
            combobox1_ItemsChange();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetDat();
            dr();
            combobox1_ItemsChange();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void SaveFile()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Data Files ( *.dat )|*.dat";
            saveFileDialog1.Title = "Save an Data Files";
            saveFileDialog1.ShowDialog();
            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                StreamWriter sw = new StreamWriter(fs);
                GetDat();
                sw.WriteLine(SET.count);
                sw.WriteLine(SET.vumir);
                for (int i = 0; i < SET.count; i++)
                {
                    for (int j = 0; j < SET.vumir; j++)
                        sw.WriteLine(SET.fset[i][j] + " ");
                   // sw.WriteLine();
                }
                sw.Close();
                fs.Close();
            }
        }

        private void dr()
        {

            // Получим панель для рисования
            GraphPane pane = zedGraph.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            // Создадим список точек
            PointPairList list = new PointPairList();

            // Интервал, в котором будут лежать точки
            int xmin = 0;
            int xmax = 10;

            int ymin = 0;
            int ymax = 7;

            // Заполняем список точек
            for (int i = 0; i < SET.count; i++)
                list.Add(SET.fset[i][0], SET.fset[i][1]);

            // !!!
            // Создадим кривую с названием "Множина".
            // Обводка ромбиков будут рисоваться голубым цветом (Color.Blue),
            // Опорные точки - ромбики (SymbolType.Diamond)
            LineItem myCurve = pane.AddCurve("Множина", list, Color.Blue, SymbolType.Diamond);

            // !!!
            // У кривой линия будет невидимой
            myCurve.Line.IsVisible = false;

            // !!!
            // Цвет заполнения отметок (ромбиков) - gолубой
            myCurve.Symbol.Fill.Color = Color.Blue;

            // !!!
            // Тип заполнения - сплошная заливка
            myCurve.Symbol.Fill.Type = FillType.Solid;

            // !!!
            // Размер ромбиков
            myCurve.Symbol.Size = 7;


            // Устанавливаем интересующий нас интервал по оси X
            pane.XAxis.Scale.Min = xmin;
            pane.XAxis.Scale.Max = xmax;

            // Устанавливаем интересующий нас интервал по оси Y
            pane.YAxis.Scale.Min = ymin;
            pane.YAxis.Scale.Max = ymax;


            // Устанавливаем интересующий нас интервал по оси X
            pane.XAxis.Scale.Min = xmin;
            pane.XAxis.Scale.Max = xmax;
            pane.XAxis.Title.Text = "Властивість Х";
            pane.Title.Text = "Кластеризація";

            // Устанавливаем интересующий нас интервал по оси Y
            pane.YAxis.Scale.Min = ymin;
            pane.YAxis.Scale.Max = ymax;
            pane.YAxis.Title.Text = "Властивість Y";
            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            // В противном случае на рисунке будет показана только часть графика, 
            // которая умещается в интервалы по осям, установленные по умолчанию
            zedGraph.AxisChange();

            // Обновляем график
            zedGraph.Invalidate();

        }

        public void GetDat()
        {

            SET.count = dataGridView1.RowCount - 1;
            SET.fset = new double[dataGridView1.RowCount - 1][];

            //  MessageBox.Show(SET.vumir.ToString() + " <>");
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                SET.fset[i] = new double[SET.vumir];
                for (int j = 0; j < SET.vumir; j++)
                {
                    SET.fset[i][j] = double.Parse((string)dataGridView1.Rows[i].Cells[j].Value);
                    //MessageBox.Show(SET.fset[i][j].ToString());
                }//
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Кластерed(comboBox1.SelectedIndex + 1);
        }

        private void Кластерed(int x)
        {
            GetDat();
            count_clast = x;
            switch (selected_metruk)
            {
                case 0: SET.EVklidova();
                    break;
                case 1: SET.lin();
                    break;
                case 2: SET.Minkovskiy(2);
                    break;
                case 3: SET.Evklid_kvad();
                    break;
                default:
                    SET.EVklidova();
                    break;
            }
            SET.testc(count_clast);
            Drow_resNEW();
            check = 1;
        }

        private void Drow_resNEW()
        {

            // Получим панель для рисования
            GraphPane pane = zedGraph.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            // Создадим список точек
            PointPairList[] list = new PointPairList[SET.count];

            // Интервал, в котором будут лежать точки
            int xmin = 0;
            int xmax = 10;

            int ymin = 0;
            int ymax = 7;
            for (int i = 0; i < SET.count; i++)
                list[i] = new PointPairList();
            LineItem[] myCurve = new LineItem[count_clast];
            // Заполняем список точек
            for (int i = 0; i < count_clast; i++)
            {
                for (int j = 0; j < SET.tresh[i].Count; j++)
                    list[i].Add(SET.fset[SET.tresh[i][j]][0], SET.fset[SET.tresh[i][j]][1]);

                myCurve[i] = pane.AddCurve("Кластер" + (i + 1).ToString(), list[i], _colors[i], SymbolType.Diamond);
                myCurve[i].Line.IsVisible = false;
                myCurve[i].Symbol.Border.IsVisible = false;
                myCurve[i].Symbol.Fill = new Fill(_colors[i]);
                myCurve[i].Symbol.Size = 7;
            }

            // Устанавливаем интересующий нас интервал по оси X
            pane.XAxis.Scale.Min = xmin;
            pane.XAxis.Scale.Max = xmax;
            pane.XAxis.Title.Text = "Властивість Х";
            pane.Title.Text = "Кластеризація";

            // Устанавливаем интересующий нас интервал по оси Y
            pane.YAxis.Scale.Min = ymin;
            pane.YAxis.Scale.Max = ymax;
            pane.YAxis.Title.Text = "Властивіст Y";
            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            // В противном случае на рисунке будет показана только часть графика, 
            // которая умещается в интервалы по осям, установленные по умолчанию
            zedGraph.AxisChange();

            // Обновляем график
            zedGraph.Invalidate();
        }

        private void combobox1_ItemsChange()
        {
            int S_index = comboBox1.SelectedIndex;
            comboBox1.Items.Clear();

            for (int i = 0; i < SET.fset.Count(); i++)
            {
                comboBox1.Items.Add(i + 1);
                if (i > 8)
                    break;
            }
            if (SET.fset.Count() > S_index)
                comboBox1.SelectedIndex = S_index;

        }

        private void zedGraph_Click(object sender, MouseEventArgs e)
        {
            double x, y;
            double[] z = new double[SET.vumir];
            zedGraph.GraphPane.ReverseTransform(e.Location, out x, out y);
            for (int i = 0; i < SET.vumir; i++)
                if (i % 2 == 0)
                    z[i] = x;
                else
                    z[i] = y;

            dataGridView1.Rows.Add();
            //MessageBox.Show((dataGridView1.Rows.Count-1).ToString());
            for (int i = 0; i < SET.vumir; i++)
                dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells[i].Value = z[i].ToString();
            GetDat();
            if (check == 1)
                Кластерed(comboBox1.SelectedIndex + 1);
            else
                dr();

            combobox1_ItemsChange();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            check = 0;
            dataGridView1.Rows.Clear();
            combobox1_ItemsChange();
            SET.count = 0;
            dr();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            selected_metruk = 0;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            selected_metruk = 1;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            selected_metruk = 2;
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            check = 0;
            SET.vumir = 9 - domainUpDown1.SelectedIndex;
            for (int i = 0; i < 9 - domainUpDown1.SelectedIndex; i++)
            {
                DataGridViewColumn Col = new DataGridViewColumn();
                DataGridViewCell cell = new DataGridViewTextBoxCell();
                Col.CellTemplate = cell;

                Col.HeaderText = "X" + (i + 1).ToString();
                Col.Name = "X" + (i + 1).ToString();
                Col.Visible = true;
                Col.Width = 70;
                dataGridView1.Columns.Add(Col);
            }
        }

        public double indexS(int c)
        {
            double A = 0, B = 1, min;
            double[] S;
            double[] GS = new double[c];
            Кластер_IS(c);

            for (int i = 0; i < c; i++)
            {
                S = new double[SET.tresh[i].Count];
                for (int j = 0; j < SET.tresh[i].Count; j++)
                {
                    for (int k = 0; k < SET.tresh[i].Count(); k++)
                        A += SET.obj_obj_copy[SET.tresh[i][j]][SET.tresh[i][k]];
                    for (int k = 0; k < SET.obj_obj_copy[0].Count(); k++)
                        if (SET.tresh[i].Contains(k) == false)
                        if (B > SET.obj_obj_copy[SET.tresh[i][j]][k])
                                B = SET.obj_obj_copy[SET.tresh[i][j]][k];
                    if (A > B)
                        min = B;
                    else min = A;
                    S[j] = (B - A) / min;
                }
                GS[i] = S.Sum() / SET.tresh[i].Count;
            }

            return GS.Sum()/c;

        }

        private void Кластер_IS(int c)
        {
            GetDat();
            switch (selected_metruk)
            {
                case 0: SET.EVklidova();
                    break;
                case 1: SET.lin();
                    break;
                case 2: SET.Minkovskiy(2);
                    break;
                case 3: SET.Evklid_kvad();
                    break;
                default:
                    SET.EVklidova();
                    break;
            }
            SET.testc(c);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int c_best = 1;
            double max = -9900, index;
            for (int i = 1; i < SET.fset.Count(); i++)
            {
                index = indexS(i);

                if (max < index && index < 99999)
                {
                    c_best = i;
                    max = index;
                }

                if (i == 10)
                    break;
            }
            comboBox1.SelectedIndex = c_best - 1;
            Кластерed(c_best);
        }

        private void zedGraph_Load(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            selected_metruk = 2;
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void проАвтораToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Програму розробив\nстудент СШІм-11\nСамковський Тарас");
        }

        private void допомогаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FHelp fhelp = new FHelp();
            fhelp.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FResult frez = new FResult();
            frez.Show();
        }

        private void завантажитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadFile();
        }

        private void зберегтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
    