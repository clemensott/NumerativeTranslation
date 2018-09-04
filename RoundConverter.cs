using System;
using System.Globalization;
using System.Windows.Data;

namespace NumerativeTranslation
{
    class RoundConverter : IValueConverter
    {
        private Round currentValue;
        private IntConverter relCon, absCon;

        public RoundConverter()
        {
            relCon = new IntConverter();
            absCon = new IntConverter();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            currentValue = (Round)value;

            switch (parameter.ToString())
            {
                case "IsRelative":
                    return currentValue.IsRelative;

                case "IsAbsolute":
                    return !currentValue.IsRelative;

                case "Relative":
                    return relCon.Convert(currentValue.Relative);

                case "Absolute":
                    return absCon.Convert(currentValue.Absolute);

                default:
                    throw new NotImplementedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (parameter.ToString())
            {
                case "IsRelative":
                    currentValue.IsRelative = (bool)value;
                    break;

                case "IsAbsolute":
                    currentValue.IsRelative = !(bool)value;
                    break;

                case "Relative":
                    currentValue.Relative = relCon.ConvertBack(value);
                    break;

                case "Absolute":
                    currentValue.Absolute = absCon.ConvertBack(value);
                    break;

                default:
                    throw new NotImplementedException();
            }

            return currentValue;
        }
    }
}
