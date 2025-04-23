using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

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
