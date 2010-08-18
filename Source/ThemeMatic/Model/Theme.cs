using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using ThemeMatic.Components.Extension;

namespace ThemeMatic.Model
{
    public class Theme: INotifyPropertyChanged
    {
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;
                name = value;
                this.Changed(() => Name, PropertyChanged);
            }
        }

        private string baseResourceDictionaryUrl;

        public string BaseResourceDictionaryUrl
        {
            get { return baseResourceDictionaryUrl; }
            set
            {
                if (baseResourceDictionaryUrl == value) return;
                baseResourceDictionaryUrl = value;
                this.Changed(() => BaseResourceDictionaryUrl, PropertyChanged);
            }
        }

        public ResourceDictionary BaseResourceDictionary
        {
            get { return null; /* TODO */ }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set
            {
                if (description == value) return;
                description = value;
                this.Changed(() => Description, PropertyChanged);
            }
        }

        private ImageSource previewImage;

        public ImageSource PreviewImage
        {
            get { return previewImage; }
            set
            {
                if (previewImage == value) return;
                previewImage = value;
                this.Changed(() => PreviewImage, PropertyChanged);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
