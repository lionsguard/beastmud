using System;
using System.Windows;
using System.Windows.Data;

namespace Beast.MapMaker.Converter
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanVisiblityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        #endregion
    }
}
