// ===== File: ColorSelectorViewModel.cs =====
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Media;

public class ColorSelectorViewModel : INotifyPropertyChanged
{
    private SolidColorBrush _selectedColor = new SolidColorBrush(Colors.Magenta);
    public Color SelectedColorValue => _selectedColor.Color;

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
