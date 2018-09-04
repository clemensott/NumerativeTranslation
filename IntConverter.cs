using System;
using System.Globalization;
using System.Windows.Data;

namespace NumerativeTranslation
{
    class IntConverter : IValueConverter
    {
        private string text;

        public int CurrentValue { get; set; }

        public IntConverter()
        {
            CurrentValue = 0;
            text = CurrentValue.ToString();
        }

        public string Convert(object value)
        {
            return Convert((int)value);
        }

        public string Convert(int value)
        {
            if (CurrentValue == value) return text;

            CurrentValue = value;

            return text = value.ToString();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value);
        }

        public int ConvertBack(object value)
        {
            return ConvertBack(value.ToString());
        }

        public int ConvertBack(string value)
        {
            int newValue;
            text = value;

            if (int.TryParse(text, out newValue)) return CurrentValue = newValue;

            return CurrentValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBack(value);
        }
    }
}
