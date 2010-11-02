using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ThemeMatic.Components
{
    public class WindowsMessagePresenter : IMessagePresenter
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
