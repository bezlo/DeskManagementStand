// ===== File: ColorSelectorViewModel.cs =====
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;

//namespace DeskManagementStand_App.ViewModel;
// Klasa ViewModel dla ColorSelector
// Zawiera logikę do konwersji kolorów i powiadamiania o zmianach
// Implementuje INotifyPropertyChanged, aby powiadamiać widok o zmianach w danych

public class ColorSelectorViewModel : INotifyPropertyChanged
    {
    //##### public
    public static Color SelectedColorValue => _selectedColor.Color;


    //##### private    
    private static SolidColorBrush _selectedColor = new SolidColorBrush(Colors.Magenta);

    private static FileSvgReader _reader = new FileSvgReader(new WpfDrawingSettings());
    //
    private static string DeskPath = "Resources/svg/Desk.svg";
    private static string DeskBackPath ="Resources/svg/Back.svg";
    //
    private static DrawingGroup _deskDrawing = _reader.Read(DeskPath);
   
    private ImageSource _deskImageSource;
    public ImageSource DeskImageSource
    {
        get => _deskImageSource;
        set
        {
            _deskImageSource = value;
            OnPropertyChanged();
        }
    }

    public ColorSelectorViewModel()
    {
        // Ustawienia początkowe
        // Zmiana koloru początkowego
        SetColorFromAngle(160);
        ChangeCurrentColorRecursive(_deskDrawing, _selectedColor);
        DeskImageSource = new DrawingImage(_deskDrawing);
        
    }

    private void ChangeCurrentColorRecursive(Drawing drawing, Brush newBrush)
    {
        if (drawing is DrawingGroup group)
        {
            foreach (var child in group.Children)
            {
                ChangeCurrentColorRecursive(child, newBrush);
            }
        }
        else if (drawing is GeometryDrawing geometryDrawing)
        {
            // Zmiana fill (Brush)
            if (geometryDrawing.Brush is SolidColorBrush fillBrush &&
                fillBrush.Color == ((SolidColorBrush)SystemColors.ControlTextBrush).Color)
            {
                geometryDrawing.Brush = newBrush;
            }

            // Zmiana stroke (Pen.Brush)
            if (geometryDrawing.Pen?.Brush is SolidColorBrush strokeBrush &&
                strokeBrush.Color == ((SolidColorBrush)SystemColors.ControlTextBrush).Color)
            {
                geometryDrawing.Pen.Brush = newBrush;
            }
        }
    }
    public SolidColorBrush SelectedColor
        {
            get => _selectedColor;
            set
            {
                _selectedColor = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedColorValue));
            }
        }
        
        public void SetColorFromAngle(double angleDegrees)
        {
            var color = HsvToRgb(angleDegrees, 1, 1);
            SelectedColor = new SolidColorBrush(color);
        }
    
        private Color HsvToRgb(double h, double s, double v)
        {
            double c = v * s;
            double x = c * (1 - Math.Abs(h / 60 % 2 - 1));
            double m = v - c;
            double r = 0, g = 0, b = 0;
    
            if (h < 60) { r = c; g = x; b = 0; }
            else if (h < 120) { r = x; g = c; b = 0; }
            else if (h < 180) { r = 0; g = c; b = x; }
            else if (h < 240) { r = 0; g = x; b = c; }
            else if (h < 300) { r = x; g = 0; b = c; }
            else { r = c; g = 0; b = x; }
    
            return Color.FromRgb((byte)((r + m) * 255), (byte)((g + m) * 255), (byte)((b + m) * 255));
        }
    
        public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        Debug.WriteLine($"Property changed: {name}");
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
