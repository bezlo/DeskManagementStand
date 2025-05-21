using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// Logika interakcji dla klasy Profile.xaml
    /// </summary>
    public partial class Profile : UserControl
    {
        public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(string), typeof(Profile), new PropertyMetadata(string.Empty));

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public Profile()
        {
            InitializeComponent();
            Loaded += Profile_Loaded;
        }
        private void Profile_Loaded(object sender, RoutedEventArgs e)
        {

            // Set the image source to the icon path
            if (!string.IsNullOrEmpty(Icon))
            {
                var drawingimage = TryFindResource(Icon) as DrawingImage;
                if (drawingimage != null)
                {
                    double width = IconGrid.ActualWidth * 0.6; //* 0.95;
                    double height = IconGrid.ActualHeight * 0.6;// * 0.95;
                    double size = Math.Min(width, height);
                    var image = new Image
                    {
                        Source = drawingimage,
                        //Margin = new Thickness(5, 5, 5, 5),
                        Width = size,
                        Height = size,
                        Stretch = Stretch.Uniform,
                    };
                    IconGrid.Children.Clear(); // Clear previous images
                    IconGrid.Children.Add(image);
                }
            }
        }
    }
}
