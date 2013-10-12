using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Clustering
{
    public partial class FHelp : Form
    {
        public FHelp()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            richTextBox1.Text = "Задати множину обєктів можна трьома способами:\n 1. Ввести значення властивостей обєтів у табличку.\n 2. Завантажити збережені обєкти.\n 3. Задати за допоогою миші.";
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            richTextBox1.Text = "Перед початком кластеризації необхідно задати початкові параметри.\n 1. Обрати спосіб визначення близькості обєктів.\n 2. Задати кількість кластерів розбиття або ж обрати автоматичну кластеризацію, у цьому випадку буде обрана найоптимальніша кількість кластерів.";
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            richTextBox1.Text = "Результат кластеризації представлений графічно, відображається у декартовй системі координат, де різними кольорами забарвлені різні кластери. При текстовому вивдені результатів клатеризації відображається перелік кластерів із об'єктами які до нього входять.";
        }
    }
}
