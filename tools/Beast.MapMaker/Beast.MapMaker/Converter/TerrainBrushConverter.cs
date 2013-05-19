using Beast.MapMaker.Services;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace Beast.MapMaker.Converter
{
    public class TerrainBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var defaultBrush = new SolidColorBrush((Color)App.Current.Resources["BackgroundColor"]);

            if (value == null || !(value is int))
                return defaultBrush;

            var world = DependencyResolver.Resolve<IWorld>();
            if (world == null)
                return defaultBrush;

            var terrain = world.Terrain.FirstOrDefault(t => t.Id == (int)value);
            if (terrain == null)
                return defaultBrush;

            var converter = new BrushConverter();
            return converter.ConvertFrom(string.Concat("#", terrain.Color));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
