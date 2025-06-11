using System.Windows;
using System.Windows.Input;
using DeskManagementStand_App.ViewModel;
using DeskManagementStand_App.Helpers;

namespace DeskManagementStand_App.View.UserControls
{
    public partial class ConnectionInfo : Window, ICloseable
    {
        public ConnectionInfo(ColorSelectorViewModel colorSelectorViewModel)
        {
            InitializeComponent();

            var viewModel = new ConnectionInfoViewModel(colorSelectorViewModel, this);
            //DataContext is source for bindings in XAML
            DataContext = viewModel;
        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        public void Close()
        {
            base.Close(); //from base class Window
            //this.Close(); this call ConnectionInfo::Close() method stack overflow
        }
    }
    
}
