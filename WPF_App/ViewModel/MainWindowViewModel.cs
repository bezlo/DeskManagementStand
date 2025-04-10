using System.Windows;
using System.Windows.Input;
using DeskManagementStand_App.MVVM;

namespace DeskManagementStand_App.ViewModel
{
    class MainWindowViewModel
    {
        
            public ICommand CloseAppCommand { get; }

            public MainWindowViewModel()
            {
                CloseAppCommand = new RelayCommand(_ => CloseApp());
            }

            private void CloseApp()
            { 
                Application.Current.Shutdown();
            }
        
    }
}
