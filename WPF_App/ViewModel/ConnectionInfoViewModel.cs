using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DeskManagementStand_App.MVVM;

namespace DeskManagementStand_App.ViewModel
{
    class ConnectionInfoViewModel
    {
        private TcpClientHandler _tcpHandler = new TcpClientHandler();
        public ICommand ConnectButtonCommand { get; }
        public ICommand DisconnectButtonCommand { get; }

        private string _statusInfo = "Brak połączenia";

        public string StatusInfo
        {
            get { return _statusInfo; }
            set { _statusInfo = value; }
        }

        public ObservableCollection<string> MessagesFromPi { get; } = new ObservableCollection<string>();

        public void AddMessageFromPi(string message)
        {
            MessagesFromPi.Add("[Odebrano z Pi] " + message);
        }

        public ConnectionInfoViewModel()
        {
            ConnectButtonCommand = new RelayCommand(_ => ConnectButton_Click());
            DisconnectButtonCommand = new RelayCommand(_ => DisconnectButton_Click(null, null));
        }
        private async void ConnectButton_Click()
        {
            try
            {
                await _tcpHandler.ConnectAsync("192.168.0.182", 5000);
                _tcpHandler.DataReceived += OnDataReceived;

                // wyślij wiadomość od razu po połączeniu
                string message = $"Połączono z PC | {DateTime.Now:HH:mm:ss}";
                await _tcpHandler.SendAsync(message);

                StatusInfo = "Status: Połączono";
            }
            catch (Exception ex)
            {
                StatusInfo = $"Status: Błąd połączenia - {ex.Message}";
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

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            _tcpHandler.Disconnect();
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
}

