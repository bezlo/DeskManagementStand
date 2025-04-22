using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using DeskManagementStand_App.ViewModel;
using Windows.Web.Http;


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
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private TcpClientHandler _tcpHandler = new TcpClientHandler();

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _tcpHandler.ConnectAsync("192.168.0.182", 5000);
                _tcpHandler.DataReceived += OnDataReceived;

                // wyślij wiadomość od razu po połączeniu
                string message = $"Połączono z PC | {DateTime.Now:HH:mm:ss}";
                await _tcpHandler.SendAsync(message);

                StatusTextBlock.Text = "Status: Połączono";
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Status: Błąd połączenia - {ex.Message}";
            }
        }


        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            _tcpHandler.Disconnect();
        }

        private void OnDataReceived(string message)
        {
            // Upewnij się, że operujesz na wątku UI
            Dispatcher.Invoke(() =>
            {
                MessagesTextBox.AppendText("[Odebrano z Pi] " + message + "\n");
                MessagesTextBox.ScrollToHome();
            });
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine("Tick"); // Sprawdź, czy tick w ogóle się wykonuje
            var viewModel = DataContext as MainWindowViewModel; // Odpowiedni typ tutaj
            if (viewModel?.ColorSelectorViewModel == null)
                return;

            var color = viewModel.ColorSelectorViewModel.SelectedColor.Color;
                    

            if (oldColor != color)
            {
                oldColor = color;
                string colorString = $"{color.R},{color.G},{color.B}";
                _tcpHandler.SendAsync(colorString);
            }
            else
            {
                return;
            }


        }
            //var viewModel = DataContext as ColorSelectorViewModel;
            //if (viewModel != null)
            //{
            //    var color = viewModel.SelectedColor.Color;
            //    string colorString = $"{color.R},{color.G},{color.B}";
            //    _tcpHandler.SendAsync(colorString);
            //}
       //}

        //private void SendRGB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    var viewModel = DataContext as ColorSelectorViewModel;
        //
        //    SolidColorBrush brush = viewModel.SelectedColor;
        //    Color color = brush.Color;
        //
        //    // Przygotuj dane do wysłania
        //    string colorString = $"{color.R},{color.G},{color.B}";
        //    _tcpHandler.SendAsync(colorString);
        //}
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

    /*
        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            // Szuka urządzeń Bluetooth zgodnych z danym UUID
            var devices = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(
                Guid.Parse("12345678-1234-5678-1234-56789abcdef0")));

            if (devices.Count == 0)
            {
                MessageBox.Show("Nie znaleziono Raspberry Pi");
                return;
            }

            // Łączy się z urządzeniem
            var service = await GattDeviceService.FromIdAsync(devices[0].Id);
            var characteristics = await service.GetCharacteristicsAsync();

            // Szuka charakterystyki RGB
            _rgbChar = characteristics.Characteristics.FirstOrDefault(c =>
                c.Uuid == Guid.Parse("abcdef01-1234-5678-1234-56789abcdef0"));

            if (_rgbChar == null)
            {
                MessageBox.Show("Nie znaleziono charakterystyki RGB");
            }
            else
            {
                MessageBox.Show("Połączono!");
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // Pobiera wartości RGB z suwaków
            byte r = (byte)RedSlider.Value;
            byte g = (byte)GreenSlider.Value;
            byte b = (byte)BlueSlider.Value;

            // Przygotowuje dane do wysłania
            var buffer = new byte[] { r, g, b }.AsBuffer();
            var result = await _rgbChar.WriteValueAsync(buffer);

            // Sprawdza wynik wysyłania
            if (result == GattCommunicationStatus.Success)
            {
                StatusLabel.Content = $"Wysłano RGB: {r},{g},{b}";
            }
            else
            {
                StatusLabel.Content = "Błąd wysyłania";
            }
        }
        */
}