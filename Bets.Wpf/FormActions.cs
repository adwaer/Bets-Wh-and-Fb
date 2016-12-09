using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Bets.Domain;
using Bets.Selenium;

namespace Bets.Wpf
{
    public class FormActions : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<ResultViewModel> ResultViewModels { get; set; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsBusy { get; set; }
        public bool IsUpdating { get; set; }

        public FormActions()
        {
            ResultViewModels = new ObservableCollection<ResultViewModel>();
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Start();
        }

        private int _seconds;
        public int Seconds
        {
            get { return _seconds; }
            set
            {
                _seconds = value;
                OnPropertyChanged();
            }
        }

        public void RestartTimer()
        {
            Seconds = 0;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            Seconds++;
            if (!IsBusy && !IsUpdating)
            {
                IsUpdating = true;
                foreach (var resultViewModel in ResultViewModels)
                {
                    resultViewModel.Update();
                }
                IsUpdating = false;
            }
        }

        public void UpdateHandicapWl(ResultViewModel resultViewModel)
        {
            var hadicapRow = BetsNavigator.Instance.WinlinePage.GetHadicapRow(
                resultViewModel.Team1.Names.Union(resultViewModel.Team2.Names).ToArray());

            resultViewModel.Winline.HandicapWebElement = hadicapRow;
        }
    }
}
