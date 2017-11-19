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
            get => _val;
            set
            {
                _val = value;
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
