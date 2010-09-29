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
using ThemeMatic.Model;
using ThemeMatic.ViewModels;

namespace ThemeMatic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var design = new Design();
            design.ColorScheme = new ColorScheme();
            var themeFactory = new ThemeFactory();
            // TODO design.Theme = themeFactory.GetAllThemes().First();
            var vm = new ApplicationViewModel(design, themeFactory.GetAllThemes());
            this.DataContext = vm;
        }
    }
}
