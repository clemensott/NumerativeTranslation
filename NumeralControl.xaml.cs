using System.Windows;
using System.Windows.Controls;

namespace NumerativeTranslation
{
    /// <summary>
    /// Interaktionslogik für NumeralControl.xaml
    /// </summary>
    public partial class NumeralControl : UserControl
    {
        public static readonly DependencyProperty RoundProperty =
            DependencyProperty.Register("Round", typeof(Round), typeof(NumeralControl),
                new PropertyMetadata(new Round(), new PropertyChangedCallback(OnRoundPropertyChanged)));

        private static void OnRoundPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var s = (NumeralControl)sender;
            var value = (Round)e.NewValue;

            s.SetActualValue();
        }

        public static readonly DependencyProperty BaseProperty =
            DependencyProperty.Register("Base", typeof(int), typeof(NumeralControl),
                new PropertyMetadata(10, new PropertyChangedCallback(OnBasePropertyChanged)));

        private static void OnBasePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var s = (NumeralControl)sender;
            var value = (int)e.NewValue;

            if (value < 2) s.Base = 2;
            else if (value > 36) s.Base = 36;
            else s.SetActualValue();
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(BigExpoFraction), typeof(NumeralControl),
                new PropertyMetadata(new BigExpoFraction(), new PropertyChangedCallback(OnValuePropertyChanged)));

        private static void OnValuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var s = (NumeralControl)sender;
            var value = (BigExpoFraction)e.NewValue;

            s.SetActualValue();
        }

        public Round Round
        {
            get { return (Round)GetValue(RoundProperty); }
            set { SetValue(RoundProperty, value); }
        }

        public int Base
        {
            get { return (int)GetValue(BaseProperty); }
            set { SetValue(BaseProperty, value); }
        }

        public BigExpoFraction Value
        {
            get { return (BigExpoFraction)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private BigExpoFraction translation;

        public NumeralControl()
        {
            InitializeComponent();

            translation = new BigExpoFraction(0, Base, 0);
            tbxNumeral.Text = translation.ToString();
        }

        private void Btn2Base1_Click(object sender, RoutedEventArgs e)
        {
            Base = 2;
        }

        private void Btn8Base1_Click(object sender, RoutedEventArgs e)
        {
            Base = 8;
        }

        private void Btn10Base1_Click(object sender, RoutedEventArgs e)
        {
            Base = 10;
        }

        private void Btn16Base1_Click(object sender, RoutedEventArgs e)
        {
            Base = 16;
        }

        private void TbxNumeral_TextChanged(object sender, TextChangedEventArgs e)
        {
            BigExpoFraction fraction = new BigExpoFraction(tbxNumeral.Text, Base);

            if (fraction != translation && fraction != Value)
            {
                translation = fraction;
                Value = fraction;
            }
        }

        private void SetActualValue()
        {
            if (translation.DenominatorBase != Base || Value != translation)
            {
                translation = NumeralSystemConverter.Translate(Value, Base, Round);

                if (tbxNumeral != null)
                {
                    int selectionStart = tbxNumeral.SelectionStart;
                    int selectionLength = tbxNumeral.SelectionLength;

                    tbxNumeral.Text = translation.ToString();
                    tbxNumeral.Select(selectionStart, selectionLength);
                }
            }
        }
    }
}
