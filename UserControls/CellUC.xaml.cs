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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConwaysGameOfLife.UserControls
{
    /// <summary>
    /// Interaction logic for CellUC.xaml
    /// </summary>
    public partial class CellUC : UserControl
    {
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(CellUC), new UIPropertyMetadata(string.Empty));

        

        public CellUC()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set
            {
                SetValue(StatusProperty, value);
            }
        }

        private static void OnGridChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }
    }
}
