using System.IO;
using System.Net.Sockets;
using System.Text;



namespace DeskManagementStand_App.Helpers
{
    internal class TCP_ClientHandler
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
