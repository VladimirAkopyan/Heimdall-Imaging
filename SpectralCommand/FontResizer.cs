using System;
using System.Globalization;
using System.Windows.Data;

namespace SpectralCommand
{
    /*resises text according to the height of the control. Does not take into account the width*/

    public class FontResizer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double actualHeight = (double)value;
            
            //double actualWidth = System.Convert.ToDouble(value); Does not Take width into account ATM
            var fontSize = (int) (actualHeight*.5);
            return fontSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}