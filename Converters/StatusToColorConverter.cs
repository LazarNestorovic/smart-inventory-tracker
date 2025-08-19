using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SmartInventoryTracker.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                switch (status)
                {
                    case "In Stock":
                        return new SolidColorBrush(Colors.Green);
                    case "Low Stock":
                        return new SolidColorBrush(Colors.Orange);
                    case "Out of Stock":
                        return new SolidColorBrush(Colors.Red);
                    default:
                        return new SolidColorBrush(Colors.White);
                }
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
