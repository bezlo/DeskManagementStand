using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using DeskManagementStand_App.MVVM;

namespace DeskManagementStand_App.ViewModel
{
    public class MainWindowViewModel 
    {
        public ColorSelectorViewModel ColorSelectorViewModel { get; set; }

        public ICommand CloseAppCommand { get; }

            public MainWindowViewModel()
            {
                 ColorSelectorViewModel = new ColorSelectorViewModel();
                 CloseAppCommand = new RelayCommand(_ => CloseApp());
            }

            private void CloseApp()
            { 
                Application.Current.Shutdown();
            }
        
    }
}
