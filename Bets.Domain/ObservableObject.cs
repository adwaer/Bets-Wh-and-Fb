using System.ComponentModel;
using System.Runtime.CompilerServices;
using Bets.Domain.Annotations;

namespace Bets.Domain
{
    public class ObservableObject<T> : INotifyPropertyChanged
    {
        private T _val;
        public T Value
        {
            get { return _val; }
            set
            {

                IsOLd = value.Equals(_val);

                _val = value;
                OnPropertyChanged();
            }
        }
        private bool _isOLd;
        public bool IsOLd
        {
            get
            {
                return _isOLd;
            }
            set
            {
                _isOLd = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
