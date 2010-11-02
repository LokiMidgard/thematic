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
            CheckColor(Colors.Red);
        }

        [Fact]
        public void CanRoundTripComplexColor()
        {
            CheckColor(Colors.Plum);            
        }

        [Fact]
        public void AllNamedColorsCanBeRoundTripped()
        {
            CheckColor(Colors.AliceBlue);
            CheckColor(Colors.AntiqueWhite);
            CheckColor(Colors.Aquamarine);
            CheckColor(Colors.Azure);
            CheckColor(Colors.Beige);
            CheckColor(Colors.Bisque);
            CheckColor(Colors.Black);
            CheckColor(Colors.BlanchedAlmond);
            CheckColor(Colors.Blue);
            CheckColor(Colors.BlueViolet);
            CheckColor(Colors.Brown);
            CheckColor(Colors.BurlyWood);
            CheckColor(Colors.CadetBlue);
            CheckColor(Colors.Chartreuse);
            CheckColor(Colors.Chocolate);
            CheckColor(Colors.Coral);
            CheckColor(Colors.CornflowerBlue);

            // these ones fail
            CheckColor(Colors.Aqua);
        }

        public void CheckColor(Color colorToCheck)
        {
            var hue = colorToCheck.GetHue();
            var sat = colorToCheck.GetSaturation();
            var lightness = colorToCheck.GetLightness();
            var outColor = ColorExtension.FromHSL(hue, sat, lightness);
            Assert.Equal(colorToCheck, outColor);
        }
    }
}
