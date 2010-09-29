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
using System.Windows.Shapes;

namespace ThemeMatic
{
    /// <summary>
    /// Interaction logic for PreviewWindow.xaml
    /// </summary>
    public partial class PreviewWindow : Window
    {
        public PreviewWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PreviewWindow_Loaded);
        }

        void PreviewWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Style = FindResource(typeof(Window)) as Style;
            this.ApplyTemplate();
        }
    }
}
