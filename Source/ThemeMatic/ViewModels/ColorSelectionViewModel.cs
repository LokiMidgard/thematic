using System.ComponentModel;
using System.Windows.Input;
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
            // TODO - ICommand wire-up to allow them to change selected color
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
    }
}
