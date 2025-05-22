using System.Windows;
using DeskManagementStand_App.ViewModel;

namespace DeskManagementStand_App.View.UserControls
{

    public partial class ConnectionInfo : Window
    {
        public ConnectionInfo(ColorSelectorViewModel colorSelectorViewModel)
        {
            InitializeComponent();
            // POPRAWKA: Utwórz instancję ViewModel i ustaw jako DataContext
            // Zakładamy, że masz instancję ColorSelectorViewModel lub możesz ją utworzyć tutaj
            //var colorSelectorViewModel = new ColorSelectorViewModel(); // lub przekaż z zewnątrz
            var viewModel = new ConnectionInfoViewModel(colorSelectorViewModel);
            DataContext = viewModel;
        }
    }
    
}
