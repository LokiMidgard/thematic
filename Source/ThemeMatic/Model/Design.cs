using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ThemeMatic.Components.Extension;

namespace ThemeMatic.Model
{
    public class Design : INotifyPropertyChanged
    {
        private ColorScheme colorScheme;
        public ColorScheme ColorScheme
        {
            get { return colorScheme; }
            set
            {
                if (colorScheme == value) return;
                colorScheme = value;
                this.Changed(() => ColorScheme, PropertyChanged);
            }
        }

        private Theme theme;
        public Theme Theme
        {
            get { return theme; }
            set
            {
                if (theme == value) return;
                theme = value;
                this.Changed(() => Theme, PropertyChanged);
                if (colorScheme != null)
                {
                    colorScheme.Theme = theme;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
