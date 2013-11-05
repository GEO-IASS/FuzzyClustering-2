using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace XCluster.View
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        private Window parentWindow;

        public HelpWindow(Window parent)
        {
            InitializeComponent();
            parentWindow = parent;
            this.Closed += (sender, args) =>
                                    {
                                        parentWindow.IsEnabled = true;
                                    };
        }

        private void Close_Help(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
