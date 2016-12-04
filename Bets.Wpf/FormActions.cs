using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bets.Wpf
{
    public class FormActions : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _seconds;

        public FormActions()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Start();
        }

        public void RestartTimer()
        {
            _seconds = 0;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            _seconds++;
            BusyText = $"Время выполнения: {_seconds}";
        }

        private string _busyText;
        public string BusyText
        {
            get { return _busyText; }
            set { _busyText = value; OnPropertyChanged("BusyText"); }
        }

    }
}
