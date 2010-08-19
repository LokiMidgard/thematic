using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThemeMatic.ViewModels;

namespace ThemeMatic.Views
{
    /// <summary>
    /// Interaction logic for ColorSelectionView.xaml
    /// </summary>
    public partial class ColorSelectionView : UserControl
    {
        public bool isDragging;
        private const double SelectionEllipseRadius = 6;
        private ColorSelectionViewModel model;

        public ColorSelectionView()
        {
            InitializeComponent();
            selectionElipse.MouseDown += SelectionMouseDown;
            selectionElipse.MouseUp += SelectionMouseUp;
            selectionContainer.MouseLeave += SelectionMouseLeave;
            selectionElipse.MouseMove += SelectionMouseMove;
            selectionContainer.MouseMove += SelectionMouseMove;
            this.DataContextChanged += new DependencyPropertyChangedEventHandler(ColorSelectionView_DataContextChanged);
        }

        void ColorSelectionView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            model = this.DataContext as ColorSelectionViewModel;
        }



        void SelectionMouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                var position = Mouse.GetPosition(selectionContainer);
                Canvas.SetLeft(selectionElipse, position.X - SelectionEllipseRadius);
                Canvas.SetTop(selectionElipse, position.Y - SelectionEllipseRadius);
                if (model != null && position.X > 0 && position.Y > 0)
                {
                    model.ColorScheme.Primary.Base = GetColorFromPosition(position, colorWheel.Source as BitmapSource, selectionContainer);
                }
            }
        }

        // color selection code adapted from here http://www.codeproject.com/KB/WPF/ImageColorPicker.aspx
        private Color GetColorFromPosition(Point position, BitmapSource bitmapSource, FrameworkElement positionRelativeToElement)
        {
            double x = position.X;
            double y = position.Y;

            x *= bitmapSource.PixelWidth / positionRelativeToElement.ActualWidth;
            if ((int)x > bitmapSource.PixelWidth - 1)
                x = bitmapSource.PixelWidth - 1;
            else if (x < 0)
                x = 0;
            y *= bitmapSource.PixelHeight / positionRelativeToElement.ActualHeight;
            if ((int)y > bitmapSource.PixelHeight - 1)
                y = bitmapSource.PixelHeight - 1;
            else if (y < 0)
                y = 0;

            byte[] pixels = new byte[4];
            int stride = (bitmapSource.PixelWidth *
                          bitmapSource.Format.BitsPerPixel + 7) / 8;
            bitmapSource.CopyPixels(new Int32Rect((int)x, (int)y, 1, 1),
                                                   pixels, stride, 0);

            return Color.FromArgb(pixels[3], pixels[2], pixels[1], pixels[0]);
        }

        void SelectionMouseLeave(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        void SelectionMouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }

        void SelectionMouseDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
        }
    }
}
