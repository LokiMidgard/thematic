using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
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
using ThemeMatic.Components;
using ThemeMatic.Model;
using ThemeMatic.ViewModels;

namespace ThemeMatic
{

 


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [PreserveSig]
        public static extern uint GetModuleFileName
        (
            [In]
    IntPtr hModule,

            [Out] 
    StringBuilder lpFilename,

            [In]
    [MarshalAs(UnmanagedType.U4)]
    int nSize
);

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var themeFactory = new ThemeFactory();
            var vm = new ApplicationViewModel(new Design(){ProjectName = "<Your Project Name Here>"}, themeFactory.GetAllThemes(), new WindowsMessagePresenter(), new SimpleProjectGenerator(this.StartupPath));
            this.DataContext = vm;
            vm.PreviewCommand.Execute(null);
        }

        private string StartupPath
        {
            get
            {
                var buffer = new StringBuilder(1024);
                GetModuleFileName(IntPtr.Zero, buffer, buffer.Capacity);
                return System.IO.Path.GetDirectoryName(buffer.ToString());
            }
        }
 

    }
}
