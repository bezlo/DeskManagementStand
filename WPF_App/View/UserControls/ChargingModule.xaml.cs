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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeskManagementStand_App.View.UserControls
{
    /// <summary>
    /// Interaction logic for ChargingModule.xaml
    /// </summary>
    public partial class ChargingModule : UserControl
    {
        public ChargingModule()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Storyboard blinkStoryboard = (Storyboard)this.Resources["BlinkAnimation"];
            blinkStoryboard.Begin();
        }
    }
}
