﻿// ===== File: ColorSelectorViewModel.cs =====
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Media;

//namespace DeskManagementStand_App.ViewModel;
// Klasa ViewModel dla ColorSelector
// Zawiera logikę do konwersji kolorów i powiadamiania o zmianach
// Implementuje INotifyPropertyChanged, aby powiadamiać widok o zmianach w danych

public class ColorSelectorViewModel : INotifyPropertyChanged
    {
    //##### public
    public  Color SelectedColorValue;

    public event PropertyChangedEventHandler? PropertyChanged;
    public Color SelectedColor
    {
        get => _selectedColor;
        set
        {
            if (_selectedColor == value) return; // Zmiana tylko, gdy kolor się zmienia
            else if (_selectedColor != value)
            {
                _selectedColor = value;
                SelectedColorValue = value;
                OnPropertyChanged();
                //OnPropertyChanged(nameof(SelectedColorValue));
                //ColorChanged?.Invoke(value); // powiadom inne klas
            }
        }
    }
    public Brush SelectedBrush
    {
        get => _selectedBrush;
        set
        {
            if (_selectedBrush == value) return; // Zmiana tylko, gdy kolor się zmienia
            else if (_selectedBrush != value)
            {
                _selectedBrush = value;
                OnPropertyChanged();
            }
        }
    }
    //public event Action<Color> ColorChanged;





    //##### private    
    private Color _selectedColor;

    private Brush _selectedBrush;
    
    
    public void SetColorFromAngle(double angleDegrees)
        {
            SelectedColor = HsvToRgb(angleDegrees, 1, 1);
            SelectedBrush = new SolidColorBrush(SelectedColor);
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
    
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        Debug.WriteLine($"Property changed: {name}");
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }

    //private void ChangeCurrentColorRecursive(Drawing drawing, Brush newBrush)
    //{
    //    if (drawing is DrawingGroup group)
    //    {
    //        foreach (var child in group.Children)
    //        {
    //            ChangeCurrentColorRecursive(child, newBrush);
    //        }
    //    }
    //    else if (drawing is GeometryDrawing geometryDrawing)
    //    {
    //        // Zmiana fill (Brush)
    //        if (geometryDrawing.Brush is SolidColorBrush fillBrush &&
    //            fillBrush.Color == ((SolidColorBrush)SystemColors.ControlTextBrush).Color)
    //        {
    //            geometryDrawing.Brush = newBrush;
    //        }
    //
    //        // Zmiana stroke (Pen.Brush)
    //        if (geometryDrawing.Pen?.Brush is SolidColorBrush strokeBrush &&
    //            strokeBrush.Color == ((SolidColorBrush)SystemColors.ControlTextBrush).Color)
    //        {
    //            geometryDrawing.Pen.Brush = newBrush;
    //        }
    //    }
    //}
}
