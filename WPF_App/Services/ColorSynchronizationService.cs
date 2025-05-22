using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Threading;
using DeskManagementStand_App.Helpers;

namespace DeskManagementStand_App.Services
{
    internal class ColorSynchronizationService
    {
        
        private readonly Func<Color> _getCurrentColor;
        private DispatcherTimer _timer;
        private Color _oldColor = Colors.Transparent; // Inicjalizacja zmiennej oldColor
        public string ColorString { get; private set; } = string.Empty; // Inicjalizacja zmiennej ColorString
        public Color CurrentColor { get; private set; } = Colors.Transparent; // Inicjalizacja zmiennej CurrentColor

        public event Action<Color>? ColorChanged; // <- zdarzenie
        public ColorSynchronizationService(Func<Color> getColor)
        {
            
            _getCurrentColor = getColor;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
        }

        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();

        private void Timer_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine("Tick"); // Sprawdź, czy tick w ogóle się wykonuje

            var currentColor = _getCurrentColor();

            if (_oldColor != currentColor)
            {
                _oldColor = currentColor;
                CurrentColor = currentColor; // <- aktualizacja CurrentColor
                ColorString = $"{currentColor.R},{currentColor.G},{currentColor.B}"; // <- aktualizacja ColorString

                ColorChanged?.Invoke(currentColor); // <- wywołanie zdarzenia
            }
        }

    }
}
