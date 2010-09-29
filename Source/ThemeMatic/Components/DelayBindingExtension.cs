using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ThemeMatic.Components
{

    [MarkupExtensionReturnType(typeof(object))]
    public class DelayBindingExtension : MarkupExtension
    {
        public DelayBindingExtension()
        {
            Delay = TimeSpan.FromSeconds(0.5);
        }

        public DelayBindingExtension(PropertyPath path)
            : this()
        {
            Path = path;
        }

        public IValueConverter Converter { get; set; }
        public object ConverterParamter { get; set; }
        public string ElementName { get; set; }
        public RelativeSource RelativeSource { get; set; }
        public object Source { get; set; }
        public bool ValidatesOnDataErrors { get; set; }
        public bool ValidatesOnExceptions { get; set; }
        public TimeSpan Delay { get; set; }
        
        [ConstructorArgument("path")]
        public PropertyPath Path { get; set; }
        
        [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
        public CultureInfo ConverterCulture { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var valueProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (valueProvider != null)
            {
                var bindingTarget = valueProvider.TargetObject as DependencyObject;
                var bindingProperty = valueProvider.TargetProperty as DependencyProperty;
                if (bindingProperty == null || bindingTarget == null)
                {
                    throw new NotSupportedException(string.Format(
                        "The property '{0}' on target '{1}' is not valid for a DelayBinding. The DelayBinding target must be a DependencyObject, "
                        + "and the target property must be a DependencyProperty.",
                        valueProvider.TargetProperty,
                        valueProvider.TargetObject));
                }

                var binding = new Binding();
                binding.Path = Path;
                binding.Converter = Converter;
                binding.ConverterCulture = ConverterCulture;
                binding.ConverterParameter = ConverterParamter;
                if (ElementName != null) binding.ElementName = ElementName;
                if (RelativeSource != null) binding.RelativeSource = RelativeSource;
                if (Source != null) binding.Source = Source;
                binding.ValidatesOnDataErrors = ValidatesOnDataErrors;
                binding.ValidatesOnExceptions = ValidatesOnExceptions;

                return DelayBinding.SetBinding(bindingTarget, bindingProperty, Delay, binding);
            }
            return null;
        }
    }

}
