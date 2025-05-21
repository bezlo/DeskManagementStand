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

namespace DeskManagementStand_App.View.UserControls.ChargingTextUserControl
{
    public partial class ChargingTextUserControl : UserControl
    {
        public static readonly DependencyProperty VoltageProperty =
        DependencyProperty.Register("VoltProperty", typeof(string), typeof(ChargingTextUserControl), new PropertyMetadata(string.Empty));

        public string Voltage
        {
            get { return (string)GetValue(VoltageProperty); }
            set { SetValue(VoltageProperty, value); }
        }
        public static readonly DependencyProperty CurrentProperty =
        DependencyProperty.Register("VoltProperty", typeof(string), typeof(ChargingTextUserControl), new PropertyMetadata(string.Empty));

        public string Current
        {
            get { return (string)GetValue(CurrentProperty); }
            set { SetValue(CurrentProperty, value); }
        }
        public static readonly DependencyProperty PowerProperty =
        DependencyProperty.Register("VoltProperty", typeof(string), typeof(ChargingTextUserControl), new PropertyMetadata(string.Empty));

        public string Power
        {
            get { return (string)GetValue(PowerProperty); }
            set { SetValue(PowerProperty, value); }
        }

        public ChargingTextUserControl()
        {
            InitializeComponent();
            Loaded += ChargingTextUserControl_Loaded;
        }

        private void ChargingTextUserControl_Loaded(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(Voltage))
            {
                ;
            }
            if (!string.IsNullOrEmpty(Current))
            {
                ;
            }
            if (!string.IsNullOrEmpty(Power))
            {
                ;
            }
        }
    }
}
