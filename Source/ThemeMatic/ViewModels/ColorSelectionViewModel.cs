using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ThemeMatic.Model;

namespace ThemeMatic.ViewModels
{
    public class ColorSelectionViewModel : INotifyPropertyChanged
    {
        private readonly ColorScheme colorScheme;

        public ColorSelectionViewModel(ColorScheme colorScheme)
        {
            this.colorScheme = colorScheme;
        }

        public ColorScheme ColorScheme
        {
            get { return colorScheme; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
