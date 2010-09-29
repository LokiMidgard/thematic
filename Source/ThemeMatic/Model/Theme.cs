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
                this.dictionary = null;
                this.Changed(() => BaseResourceDictionary, PropertyChanged);
            }
        }

        private ResourceDictionary dictionary;
        public ResourceDictionary BaseResourceDictionary
        {
            get 
            {
                if (dictionary == null && !string.IsNullOrEmpty(baseResourceDictionaryUrl))
                {
                    // just in case it's a pack URI (which it probably will be)
                    if (!UriParser.IsKnownScheme("pack"))
                        UriParser.Register(new GenericUriParser(GenericUriParserOptions.GenericAuthority), "pack", -1);
      
                    Uri uri = new Uri(baseResourceDictionaryUrl, UriKind.RelativeOrAbsolute);
                    dictionary = new ResourceDictionary() { Source = uri};
                }
                return dictionary;
            }
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
