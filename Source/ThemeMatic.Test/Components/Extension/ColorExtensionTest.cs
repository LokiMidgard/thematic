using System.Windows.Media;
using Xunit;
using ThemeMatic.Components.Extension;

namespace ThemeMatic.Test.Components.Extension
{
    public class ColorExtensionTest
    {
        [Fact]
        public void CanRoundTripSimpleColor()
        {
            var hue = Colors.Red.GetHue();
            var sat = Colors.Red.GetSaturation();
            var lightness = Colors.Red.GetLightness();
            var outColor = ColorExtension.FromHSL(hue, sat, lightness);
            Assert.Equal(Colors.Red, outColor);
        }

        // this one is really close, but not exactly equal...one of the hex triplets is off by 1
        [Fact]
        public void CanRoundTripComplexColor()
        {
            var hue = Colors.Plum.GetHue();
            var sat = Colors.Plum.GetSaturation();
            var lightness = Colors.Plum.GetLightness();
            var outColor = ColorExtension.FromHSL(hue, sat, lightness);
            Assert.Equal(Colors.Plum, outColor);
            
        }
    }
}
