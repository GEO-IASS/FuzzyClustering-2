using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace ZedGraphUserControls
{
    public partial class ZedGraphUserControl: UserControl
    {
        private readonly Color[] _colors =
        { 
            Color.Blue, Color.Red, Color.YellowGreen, Color.SeaGreen, Color.LightBlue,
            Color.Yellow, Color.Purple, Color.Pink, Color.Orange, Color.Firebrick
        };

        public ZedGraphUserControl()
        {
            InitializeComponent();
            CreateGraph(zgcGraph);
        }

        public new int Width
        {
            get { return zgcGraph.Width; }
            set { zgcGraph.Width = value; }
        }

        public new int Height
        {
            get { return zgcGraph.Height; }
            set { zgcGraph.Height = value; }
        }

        public GraphPane GraphPane
        {
            get { return zgcGraph.GraphPane; }
            set { zgcGraph.GraphPane = value; }
        }

        private void CreateGraph(ZedGraphControl zgc)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;

            // Set the Titles
            myPane.Title.Text = "Кластеризація";
            myPane.XAxis.Title.Text = "X";
            myPane.YAxis.Title.Text = "Y";
            myPane.XAxis.Scale.MajorStep = 5;

            // Make up some data arrays based on the Sine function
            double x, y1, y2;
            var list1 = new PointPairList();
            var list2 = new PointPairList();
            for (var i = 0; i < 36; i++)
            {
                x = (double)i + 5;
                y1 = 1.5 + Math.Sin((double)i * 0.2);
                y2 = 3.0 * (1.5 + Math.Sin((double)i * 0.2));
                list1.Add(x, y1);
                list2.Add(x, y2);
            }

            // Generate a red curve with diamond
            // symbols, and "Porsche" in the legend
            var myCurve = myPane.AddCurve("",
                  list1, System.Drawing.Color.Red, SymbolType.Diamond);
            myCurve.Line.IsVisible = false;

            // Generate a blue curve with circle
            // symbols, and "Piper" in the legend
            var myCurve2 = myPane.AddCurve("",
                  list2, System.Drawing.Color.Blue, SymbolType.Circle);
            myCurve2.Line.IsVisible = false;

            // Tell ZedGraph to refigure the
            // axes since the data have changed
            zgc.AxisChange();
        }

        public void BuildGraph( List<List<double[]>> data)
        {
            var clusterCount = data.Count;
            // Получим панель для рисования
            GraphPane pane = zgcGraph.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            // Интервал, в котором будут лежать точки
            double xmax = 10;
            double ymax = 7;

            // Создадим список точек
            PointPairList[] list = new PointPairList[clusterCount];
            for (var i = 0; i < clusterCount; i++)
                list[i] = new PointPairList();
            LineItem[] myCurve = new LineItem[clusterCount];
            // Заполняем список точек
            for (int i = 0; i < clusterCount; i++)
            {
                for (int j = 0; j < data[i].Count; j++)
                {
                    xmax = xmax < data[i][j][0] ? data[i][j][0] : xmax;
                    ymax = ymax < data[i][j][1] ? data[i][j][1] : ymax;
                    list[i].Add(data[i][j][0], data[i][j][1]);
                }

                myCurve[i] = pane.AddCurve("Кластер" + (i + 1), list[i], _colors[i], SymbolType.Diamond);
                myCurve[i].Line.IsVisible = false;
                myCurve[i].Symbol.Border.IsVisible = false;
                myCurve[i].Symbol.Fill = new Fill(_colors[i]);
                myCurve[i].Symbol.Size = 7;
            }

            // Устанавливаем интересующий нас интервал по оси X
            pane.XAxis.Scale.Min = 0;
            pane.XAxis.Scale.Max = xmax + xmax/10;
            pane.XAxis.Title.Text = "Властивість Х";
            pane.Title.Text = "Кластеризація";

            // Устанавливаем интересующий нас интервал по оси Y
            pane.YAxis.Scale.Min = 0;
            pane.YAxis.Scale.Max = ymax + ymax/10;
            pane.YAxis.Title.Text = "Властивіст Y";
            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            // В противном случае на рисунке будет показана только часть графика, 
            // которая умещается в интервалы по осям, установленные по умолчанию
            zgcGraph.AxisChange();

            // Обновляем график
            zgcGraph.Invalidate();
        }

        public void BuildGraph(double[][] data)
        {
            var clusterCount = data[0].Length;
            // Получим панель для рисования
            GraphPane pane = zgcGraph.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            // Создадим список точек
            PointPairList[] list = new PointPairList[clusterCount];
            for (var i = 0; i < clusterCount; i++)
                list[i] = new PointPairList();
            LineItem[] myCurve = new LineItem[clusterCount];
            // Заполняем список точек
            for (int i = 0; i < clusterCount; i++)
            {
                for (int j = 0; j < data.Length; j++)
                {
                    list[i].Add(j+1, data[j][i]);
                }

                myCurve[i] = pane.AddCurve("Кластер" + (i + 1), list[i], _colors[i], SymbolType.Diamond);
                myCurve[i].Line.IsVisible = false;
                myCurve[i].Symbol.Border.IsVisible = false;
                myCurve[i].Symbol.Fill = new Fill(_colors[i]);
                myCurve[i].Symbol.Size = 7;
            }

            // Устанавливаем интересующий нас интервал по оси X
            pane.XAxis.Scale.Min = -1;
            pane.XAxis.Scale.Max = data.Length+1;
            pane.XAxis.Title.Text = "Об'єкт";
            pane.Title.Text = "Кластеризація";

            // Устанавливаем интересующий нас интервал по оси Y
            pane.YAxis.Scale.Min = -0.1;
            pane.YAxis.Scale.Max = 1.1;
            pane.YAxis.Title.Text = "Властивіст Y";

            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            // В противном случае на рисунке будет показана только часть графика, 
            // которая умещается в интервалы по осям, установленные по умолчанию
            zgcGraph.AxisChange();

            // Обновляем график
            zgcGraph.Invalidate();
        }

        public void BuildDendrogram(double[][] data)
        {
            var clusterCount = data[0].Length;
            // Получим панель для рисования
            GraphPane pane = zgcGraph.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            // Создадим список точек
            PointPairList[] list = new PointPairList[clusterCount];
            for (var i = 0; i < clusterCount; i++)
                list[i] = new PointPairList();
            LineItem[] myCurve = new LineItem[clusterCount];
            // Заполняем список точек

            var ax  =new []{3.0, 3.0};
            var ay = new[] { 1.0, 0.90 };
            var bx = new[] { 1.0, 4.0 };
            var by = new[] { 0.9, 0.90 };
            var cx = new[] { 1.0, 1.0 };
            var cy = new[] { 0.9, 0 };
            var dx = new[] { 4.0, 4.0 };
            var dy = new[] { 0.9, 0 };


            pane.AddCurve("", ax, ay, Color.Blue, SymbolType.None);
            pane.AddCurve("", bx, by, Color.Blue, SymbolType.None);
            pane.AddCurve("", cx, cy, Color.Blue, SymbolType.None);
            pane.AddCurve("", dx, dy, Color.Blue, SymbolType.None);


            // Устанавливаем интересующий нас интервал по оси X
            pane.XAxis.Scale.Min = -1;
            pane.XAxis.Scale.Max = data.Length + 1;
            //pane.XAxis.Type = AxisType.Text;
            pane.XAxis.Title.Text = "Об'єкт";
            pane.Title.Text = "Кластеризація";
            //pane.X2Axis.Type = AxisType.Text;
            //pane.X2Axis.Scale.TextLabels = new[] { "obj1", "obj2", "obj3", "obj4"};
            //pane.X2Axis.IsVisible = true;
            //pane.X2Axis.

            // Устанавливаем интересующий нас интервал по оси Y
            pane.YAxis.Scale.Min = -0.1;
            pane.YAxis.Scale.Max = 1.1;
            pane.YAxis.Title.Text = "Властивіст Y";

            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            // В противном случае на рисунке будет показана только часть графика, 
            // которая умещается в интервалы по осям, установленные по умолчанию
            zgcGraph.AxisChange();

            // Обновляем график
            zgcGraph.Invalidate();
        }

        public void ClearGraph()
        {
            var width = Width;
            var height = Height;
            zgcGraph.GraphPane = new GraphPane(new RectangleF(0,0,width,height),"Кластеризація","Х","Y");
            zgcGraph.AxisChange();
        }

        public string Title
        {
            get { return zgcGraph.GraphPane.Title.Text; }
            set { zgcGraph.GraphPane.Title.Text = value; }
        }

        public new void Refresh()
        {
            zgcGraph.Invalidate();
        }

        public void AddClickEvent(MouseEventHandler handler)
        {
            zgcGraph.MouseClick += handler;
        }

        public void BuildPoint(List<double[]> data)
        {
            double xmax = 10;
            double ymax = 7;
            var pane = zgcGraph.GraphPane;
            pane.CurveList.Clear();

            var list = new PointPairList();

            foreach (var point in data)
            {
                xmax = xmax < point[0] ? point[0] : xmax;
                ymax = ymax < point[1] ? point[1] : ymax;
                list.Add(point[0],point[1]);
            }

            var myCurve = pane.AddCurve("Множина", list, Color.Blue, SymbolType.Diamond);
            myCurve.Line.IsVisible = false;
            myCurve.Symbol.Fill.Color = Color.Blue;
            myCurve.Symbol.Fill.Type = FillType.Solid;
            myCurve.Symbol.Size = 7;

            pane.XAxis.Scale.Min = 0;
            pane.XAxis.Scale.Max = xmax + xmax / 10;
            pane.XAxis.Title.Text = "Властивість Х";
            pane.Title.Text = "Кластеризація";

            pane.YAxis.Scale.Min = 0;
            pane.YAxis.Scale.Max = ymax + ymax / 10;
            pane.YAxis.Title.Text = "Властивіст Y";
          
            zgcGraph.AxisChange();

            zgcGraph.Invalidate();
        }
    }
}
