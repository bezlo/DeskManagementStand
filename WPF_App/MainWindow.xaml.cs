using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using DeskManagementStand_App.ViewModel;


namespace DeskManagementStand_App
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        Color oldColor = Colors.AliceBlue; // Zainicjalizuj zmienną oldColor
        public MainWindow()
        {
            InitializeComponent();
            // Tworzysz instancję MainWindowViewModel
            var mainViewModel = new MainWindowViewModel();
            DataContext = mainViewModel; // Ustawiasz DataContext
        }
    }

}