using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Threading;
using DeskManagementStand_App.Helpers;

namespace DeskManagementStand_App.Services
{
    internal class ColorSynchronizationService
    {
        private readonly TCP_ClientHandler _tcpHandler;
        private readonly Func<Color> _getCurrentColor;
        private DispatcherTimer _timer;
        private Color _oldColor = Colors.Transparent; // Inicjalizacja zmiennej oldColor

        public ColorSynchronizationService(TCP_ClientHandler tcpHandler, Func<Color> getColor)
        {
            _tcpHandler = tcpHandler;
            _getCurrentColor = getColor;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(6)
            };
            _timer.Tick += Timer_Tick;
        }
        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();

        private async void Timer_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine("Tick"); // Sprawdź, czy tick w ogóle się wykonuje

            var currentColor = _getCurrentColor();

            if (_oldColor != currentColor)
            {
                _oldColor = currentColor;
                string colorString = $"{currentColor.R},{currentColor.G},{currentColor.B}";
                //teraz nie dziala, zgaduje ze problemem jest to ze tworzy nowy event kiedy w mainewindow juz jest polaczoyn
                // FIXME: teraz nie dziala, zgaduje ze problemem jest to ze tworzy nowy event kiedy w mainewindow juz jest polaczoyn
                // WARNING: teraz nie dziala, zgaduje ze problemem jest to ze tworzy nowy event kiedy w mainewindow juz jest polaczoyn
                //TODO: teraz nie dziala, zgaduje ze problemem jest to ze tworzy nowy event kiedy w mainewindow juz jest polaczoyn

                await _tcpHandler.SendAsync(colorString);
            }
            else
            {
                return;
            }
        }

    }
}
