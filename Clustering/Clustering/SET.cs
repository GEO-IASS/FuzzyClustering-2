using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Clustering
{
    class SET
    {
        
        public static int vumir = 2;
        public static int count = 0;
        public static double[][] obj_ozn;
        public static double[][] obj_obj;
        public static double[][] obj_obj_copy;
        public static double[][] fset;
        public static List<int>[] tresh;

        public static void EVklidova()
        {
            obj_obj = new double[count][];
            double d = 0;
            metruka();
            for (int i = 0; i < count; i++)
            {
                obj_obj[i] = new double[count];
                for (int j = 0; j < count; j++)
                {
                    for (int k = 0; k < vumir; k++)
                        d += (obj_ozn[i][k] - obj_ozn[j][k]) * (obj_ozn[i][k] - obj_ozn[j][k]);

                    obj_obj[i][j] = Math.Sqrt(d);
                    d = 0;
                }
            }
           obj_obj_copy = obj_obj;
        }

        public static void Evklid_kvad()
        {
            obj_obj = new double[count][];
            double d = 0;
            metruka();
            for (int i = 0; i < count; i++)
            {
                obj_obj[i] = new double[count];
                for (int j = 0; j < count; j++)
                {
                    for (int k = 0; k < vumir; k++)
                        d += (obj_ozn[i][k] - obj_ozn[j][k]) * (obj_ozn[i][k] - obj_ozn[j][k]);

                    obj_obj[i][j] = Math.Sqrt(d);
                    d = 0;
                }
            }
            obj_obj_copy = obj_obj;
        }

        public static void lin()
        {
            obj_obj = new double[count][];
            double d = 0;
            metruka();
            for (int i = 0; i < count; i++)
            {
                obj_obj[i] = new double[count];
                for (int j = 0; j < count; j++)
                {
                    for (int k = 0; k < vumir; k++)
                        d += Math.Abs(obj_ozn[i][k] - obj_ozn[j][k]);

                    obj_obj[i][j] = d;
                    d = 0;
                }
            }
            obj_obj_copy = obj_obj;
        }

        public static void Minkovskiy(int step)
        {
            obj_obj = new double[count][];
            double d = 0;
            metruka();
            for (int i = 0; i < count; i++)
            {
                obj_obj[i] = new double[count];
                for (int j = 0; j < count; j++)
                {
                    for (int k = 0; k < vumir; k++)
                        d += Math.Pow(Math.Abs(obj_ozn[i][k] - obj_ozn[j][k]),step);

                    obj_obj[i][j] = Math.Pow(d,(1.0/step));
                    d = 0;
                }
            }
            obj_obj_copy = obj_obj;
      }

        private static void metruka()
        {
            obj_ozn = new double[count][];
            for (int i = 0; i < count; i++)
                obj_ozn[i] = new double[vumir];
            for (int i = 0; i < vumir; i++)
            {
                double max = fset[0][i];
                double min = fset[0][i];

                for (int j = 0; j < count; j++)
                {
                    if (fset[j][i] > max)
                        max = fset[j][i];
                    if (fset[j][i] < min)
                        min = fset[j][i];
                }
                for (int j = 0; j < count; j++)
                {
                    obj_ozn[j][i] = (max - fset[j][i]) / (max - min);
                }

            }

        }


      public static  double[][] rek()
        {
            double min = obj_obj[0][1];
            int x = 0, y = 1;
            int m = 0, n = 0;
            for(int i = 0;i < count; i++)
                for(int j = 0; j < count; j++)
                    if ((obj_obj[i][j] < min) && (i != j))
                    {
                        x = i;
                        y = j;
                        min = obj_obj[i][j];
                    }
            double[][] tmp = new double[count - 1][];
            for (int i = 0; i < count - 1; i++)
                tmp[i] = new double[count - 1];
            for (int i = 0; i < count; i++)
            {
                if ((i != x) && (i != y))
                {
                    for (int j = 0; j < count; j++)
                        if ((j != x) && (j != y))
                        {
                            tmp[m][n] = obj_obj[i][j];
                            n++;
                        }
                        else
                            if (j == x)
                            {
                                tmp[m][n] = (obj_obj[i][x] + obj_obj[i][y]) / 2;
                                n++;
                            }
                    m++;
                }
                if (i == x)
                {
                    for (int j = 0; j < count; j++)
                    if ((j != x) && (j != y))
                    {
                        tmp[m][n] = (obj_obj[x][j] + obj_obj[y][j]) / 2;
                        n++;
                    }
                    else
                        if (j == x)
                        {
                            tmp[m][n] = 0;
                            n++;
                        }
                    m++;
                }
                n = 0;
            }
            claster_rez(x, y);
            count--;

                            return tmp;
        }

      public static void testc(int k)
      {
          tresh = new List<int>[count];
          for (int i = 0; i < count; i++)
          {
              tresh[i] = new List<int>();
              tresh[i].Add(i);
          }
          //vrite();
          //outp();
          while (count > k)
          {
              obj_obj = rek();
              //vrite();
             // outp();
          }
          //outp();
      }

      private static void claster_rez(int x, int y)
      {
          List<int>[] tmp = new List<int>[count - 1];
          int j = 0;
          for (int i = 0; i < count; i++)
          {
              if ((i != x) && (i != y))
              {
                  tmp[j] = new List<int>();
                  tmp[j].AddRange(tresh[i]);
                  j++;
              }
              if (i == x)
              {
                  tmp[j] = new List<int>();
                  tmp[j].AddRange(tresh[i]);
                  tmp[j].AddRange(tresh[y]);
                  j++;
              }
          }

          tresh = tmp;
      }

      private static void vrite()
      {
          for (int i = 0; i < count; i++)
              for (int j = 0; j < count; j++)
                  MessageBox.Show(obj_obj[i][j].ToString());
      }

      private static void outp()
      {
          for (int j = 0; j < count; j++)
          {
              MessageBox.Show(j.ToString() + " Claster ");
              for (int i = 0; i < tresh[j].Count; i++)
                  MessageBox.Show(tresh[j][i].ToString());
          }
      }
    }
}
