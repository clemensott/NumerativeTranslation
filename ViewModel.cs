using System.ComponentModel;

namespace NumerativeTranslation
{
    class ViewModel : INotifyPropertyChanged
    {
        private BigExpoFraction actualValue;
        private Round round;

        public BigExpoFraction ActualValue
        {
            get { return actualValue; }
            set
            {
                if (value == actualValue) return;

                actualValue = value;
                OnPropertyChanged("ActualValue");
            }
        }

        public Round Round
        {
            get { return round; }
            set
            {
                if (value.Equals(round)) return;

                round = value;
                OnPropertyChanged("Round");
            }
        }

        public ViewModel()
        {
            Round = new Round(true, 10, 5);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
