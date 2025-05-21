using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeskManagementStand_App.View.UserControls
{
    
    public partial class Rspi : UserControl
    {
        
        public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("XDXDXXD", typeof(string), typeof(Rspi), new PropertyMetadata(string.Empty));
        
        public string TempName
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

      
        
        public Rspi()
        {
            InitializeComponent();
            Loaded += Rspi_Loaded;
            IconGrid.SizeChanged += Rspi_Loaded;
        }
        private void Rspi_Loaded(object sender, RoutedEventArgs e)
        {
            

            // Set the image source to the icon path
            if (!string.IsNullOrEmpty(TempName))
            {
                var drawingimage = TryFindResource(TempName) as DrawingImage;
                if(drawingimage != null)
                {
                    
                    var image = new Image
                    {
                        Source = drawingimage,
                        //Margin = new Thickness(5, 5, 5, 5),
                        Width = Math.Min(IconGrid.ActualWidth, IconGrid.ActualHeight) * 0.6,
                        Height = Math.Min(IconGrid.ActualWidth, IconGrid.ActualHeight) * 0.6,
                        Stretch = Stretch.Uniform,
                    };
                    IconGrid.Children.Clear(); // Clear previous images
                    IconGrid.Children.Add(image);
                }
            }
            
            IconGrid.Children.Add(RspiButton);
            RspiButton.Height = Math.Min(IconGrid.ActualWidth, IconGrid.ActualHeight) * 0.6;
            RspiButton.Width  = Math.Min(IconGrid.ActualWidth, IconGrid.ActualHeight) * 0.6;
            
        }

        private void AdjustIconsSize()
        {
            ;
        }
    }
}
