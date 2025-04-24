using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using DeskManagementStand_App.MVVM;

namespace DeskManagementStand_App.ViewModel;

    public class MainWindowViewModel
    {
        public ColorSelectorViewModel ColorSelectorViewModel { get; } = new ColorSelectorViewModel();

        public ICommand CloseAppCommand { get; }
        public ICommand OpenConnectionSettingsCommand { get; }

        public MainWindowViewModel()
        {
            CloseAppCommand = new RelayCommand(_ => CloseApp());
            OpenConnectionSettingsCommand = new RelayCommand(_ => OpenConnectionSettings());
        }

        private void CloseApp()
        {
            Application.Current.Shutdown();
        }
        private void OpenConnectionSettings()
        {
            var connectionsettingsWindow = new DeskManagementStand_App.View.UserControls.ConnectionInfo();
            connectionsettingsWindow.Show(); // lub Show(), zależnie od potrzeby
        }

    }


