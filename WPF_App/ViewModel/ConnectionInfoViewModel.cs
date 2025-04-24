using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DeskManagementStand_App.Helpers;
using DeskManagementStand_App.MVVM;
using DeskManagementStand_App.Services;

namespace DeskManagementStand_App.ViewModel;

public class ConnectionInfoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private TCP_ClientHandler _tcpHandler = new TCP_ClientHandler();

        private readonly ColorSynchronizationService _colorSyncService;
        public ObservableCollection<string> MessagesFromPi { get; } = new ObservableCollection<string>();
        //public string MessagesFromPiText => string.Join(Environment.NewLine, MessagesFromPi);
        public ICommand ConnectButtonCommand { get; }
        public ICommand DisconnectButtonCommand { get; }

        private string _statusInfo = "Brak połączenia";
        public string StatusInfo
        {
            get { return _statusInfo; }
            set
            {
                _statusInfo = value;
                OnPropertyChanged();
                //OnPropertyChanged(nameof(_statusInfo));
            }
        }

        private string _messagesFromPiText;
        public string MessagesFromPiText
        {
            get => _messagesFromPiText;
            set
            {
                _messagesFromPiText = value;
                OnPropertyChanged();
            }
    }

        public ConnectionInfoViewModel()
        {
            // Initialize the TCP handler
            ConnectButtonCommand = new RelayCommand(_ => Connect());
            DisconnectButtonCommand = new RelayCommand(_ => Disconnect());
            // Initialize the ColorSynchronizationService with the TCP handler and a lambda to get the selected color value
            _colorSyncService = new ColorSynchronizationService(() => ColorSelectorViewModel.SelectedColorValue);
            _colorSyncService.Start();

            _colorSyncService.ColorChanged += color =>
            {
                // Reaguj na nowy kolor 
                _tcpHandler.SendAsync($"Nowy kolor: {color.R},{color.G},{color.B}");
            };

    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
         {
             Debug.WriteLine($"Property changed: {name}");
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
         }
        private async void Connect()
        {
            try
            {
                // Utwórz nowy handler
                _tcpHandler = new TCP_ClientHandler();

                // Zarejestruj event — ZA KAŻDYM razem po nowym utworzeniu obiektu
                _tcpHandler.DataReceived += OnDataReceived;

                await _tcpHandler.ConnectAsync("192.168.0.182", 5000);

                string message = $"Połączono z PC | {DateTime.Now:HH:mm:ss}";
                await _tcpHandler.SendAsync(message);

                StatusInfo = "Status: Połączono";
            }
            catch (Exception ex)
            {
                StatusInfo = $"Status: Błąd połączenia - {ex.Message}";
            }
        }
        public void Disconnect()
        {
            if (_tcpHandler != null)
            {
                _tcpHandler.DataReceived -= OnDataReceived; // odpinanie eventu — ważne
                _tcpHandler.Disconnect();
            }
        }


        private void OnDataReceived(string message)
        {
            // Upewnij się, że operujesz na wątku UI
            //Dispatcher.Invoke(() =>
            //{
            AddMessageFromPi("[Odebrano z Pi] " + message + "\n");
            //MessagesTextBox.AppendText("[Odebrano z Pi] " + message + "\n");
            //MessagesTextBox.ScrollToHome();
            //});
        }
        public void AddMessageFromPi(string message)
        {
            MessagesFromPi.Add("[Odebrano z Pi] " + message);
            MessagesFromPiText += $"[Odebrano z Pi] {message}\n";
        }


    }
    


