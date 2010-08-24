using System;
using System.ComponentModel;
using System.Windows.Input;
using ThemeMatic.Components;
using ThemeMatic.Components.Extension;
using ThemeMatic.Model;

namespace ThemeMatic.ViewModels
{
    public class ColorSelectionViewModel : INotifyPropertyChanged
    {
        private readonly ColorScheme colorScheme;

        public ColorSelectionViewModel(ColorScheme colorScheme)
        {
            this.colorScheme = colorScheme;
            selectedColor = colorScheme.Primary;
            SelectColorCommand = new DelegateCommand(SelectColorExecute);
        }

        private void SelectColorExecute(object obj)
        {
            var color = obj as DesignColor;
            if (color != null)
            {
                selectedColor = color;
                this.Changed(() => this.SelectedColor, PropertyChanged);
            }
        }

        public ColorScheme ColorScheme
        {
            get { return colorScheme; }
        }

        private DesignColor selectedColor;
        public DesignColor SelectedColor
        {
            get { return selectedColor; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand ChangeSelectedColor { get; private set; }
        public ICommand SelectColorCommand { get; private set; }
    }
}
