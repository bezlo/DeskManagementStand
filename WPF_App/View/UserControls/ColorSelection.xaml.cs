using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DeskManagementStand_App.View.UserControls
{

    public partial class ColorSelection : UserControl
    {
        private ColorSelectorViewModel ViewModel => (ColorSelectorViewModel)DataContext;
        public ColorSelection()
        {
            InitializeComponent();
            DataContext = new ColorSelectorViewModel();
            DrawColorRing();
        }

        private void DrawColorRing()
        {
            double centerX = 80;
            double centerY = 80;
            double radius = 80;
            for (int angle = 0; angle < 360; angle += 2)
            {
                var color = ViewModel.GetType()
                    .GetMethod("HsvToRgb", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .Invoke(ViewModel, new object[] { (double)angle, 1.0, 1.0 }) as Color? ?? Colors.Black;

                var line = new Line
                {
                    X1 = centerX + Math.Cos((angle - 1) * Math.PI / 180) * radius,
                    Y1 = centerY + Math.Sin((angle - 1) * Math.PI / 180) * radius,
                    X2 = centerX + Math.Cos(angle * Math.PI / 180) * radius,
                    Y2 = centerY + Math.Sin(angle * Math.PI / 180) * radius,
                    Stroke = new SolidColorBrush(color),
                    StrokeThickness = 40,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round
                };
                ColorRingCanvas.Children.Add(line);
            }
        }

        private void ColorRingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var pos = e.GetPosition(ColorRingCanvas);
                double centerX = ColorRingCanvas.Width / 2;
                double centerY = ColorRingCanvas.Height / 2;
                double dx = pos.X - centerX;
                double dy = pos.Y - centerY;
                double angle = (Math.Atan2(dy, dx) * 180 / Math.PI + 360) % 360;
                ViewModel.SetColorFromAngle(angle);
            }
        }
    }
}
