using System;
using System.Globalization;
using Cirrious.MvvmCross.Converters;

namespace MWC.Core.Mvvm.Converters
{
    public class LowerDayNameConverter : MvxBaseValueConverter
    {
        public override object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format (culture, "{0:dddd}", value).ToLowerInvariant ();
        }
    }

    public class LowerShortMonthNameConverter : MvxBaseValueConverter
    {
        public override object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format (culture, "{0:MMM}", value).ToLowerInvariant ();
        }
    }
}
