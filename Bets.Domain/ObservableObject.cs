using System.ComponentModel;
using System.Runtime.CompilerServices;
using Bets.Domain.Annotations;

namespace Bets.Domain
{
    public class ObservableObject<T> : INotifyPropertyChanged
    {
        private T Val { get; set; }
        public T Value
        {
            get { return Val; }
            set
            {
                OldValue = Val;

                Val = value;
                OnPropertyChanged();
            }
        }

        private T OldVal { get; set; }
        public T OldValue
        {
            get { return OldVal; }
            set
            {
                OldVal = value;
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
