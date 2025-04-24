using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using DeskManagementStand_App.MVVM;


namespace DeskManagementStand_App.ViewModel;

public class ConnectionInfoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private TcpClientHandler _tcpHandler = new TcpClientHandler();
    
        private DispatcherTimer _timer;
        Color oldColor = Colors.AliceBlue; // Zainicjalizuj zmienną oldColor
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
             ConnectButtonCommand = new RelayCommand(_ => Connect());
             DisconnectButtonCommand = new RelayCommand(_ => Disconnect());
               
             _timer = new DispatcherTimer
             {
                 Interval = TimeSpan.FromSeconds(1)
             };
             _timer.Tick += Timer_Tick;
             _timer.Start();
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine("Tick"); // Sprawdź, czy tick w ogóle się wykonuje

            var colorVal = ColorSelectorViewModel.SelectedColorValue;

            if (oldColor != colorVal)
            {
                oldColor = colorVal;
                string colorString = $"{colorVal.R},{colorVal.G},{colorVal.B}";
                _tcpHandler.SendAsync(colorString);
            }
            else
            {
                return;
            }
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
                _tcpHandler = new TcpClientHandler();

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
    public class TcpClientHandler
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        private bool _isReceiving = false;

        public event Action<string> DataReceived; // Event do powiadamiania o odebranych danych

        public async Task ConnectAsync(string ip, int port)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(ip, port);
            _stream = _client.GetStream();
            _reader = new StreamReader(_stream, Encoding.UTF8);
            // UTF8 bez BOM
            _writer = new StreamWriter(_stream, new UTF8Encoding(false)) { AutoFlush = true };

            // start pętli odbioru, ale nie czekaj na jej zakończenie
            _ = StartReceivingLoop();
        }

        public async Task SendAsync(string data)
        {
            if (_writer != null)
                await _writer.WriteLineAsync(data);
        }

        public async Task<string> ReceiveAsync()
        {
            try
            {
                if (_reader != null)
                    return await _reader.ReadLineAsync().WaitAsync(_cts.Token);
            }
            catch (OperationCanceledException)
            {
                // przerwane oczekiwanie
            }
            catch (IOException ex)
            {
                Console.WriteLine($"[IOException] {ex.Message}");
            }

            return null;
        }

        private async Task StartReceivingLoop()
        {
            _isReceiving = true;

            while (_isReceiving)
            {
                var received = await ReceiveAsync();
                if (!string.IsNullOrEmpty(received))
                {
                    DataReceived?.Invoke(received); // powiadom UI lub inną klasę
                }
            }
        }

        public void Disconnect()
        {
            StopReceiving();
            _cts.Cancel();

            _reader?.Dispose();
            _writer?.Dispose();
            _stream?.Dispose();
            _client?.Close();
        }

        private void StopReceiving()
        {
            _isReceiving = false;
        }
    }


