using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Bets.Domain;
using Bets.Selenium;
using Bets.Services;

namespace Bets.Wpf
{
    public class FormActions : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<ResultViewModel> ResultViewModels { get; set; }
        private BetsService BetsService { get; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsBusy { get; set; }

        public FormActions()
        {
            BetsService = new BetsService();

            ResultViewModels = new ObservableCollection<ResultViewModel>();
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Start();
        }

        private int _seconds;
        public int Seconds
        {
            get => _seconds;
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

        private int _sync = 0;
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            Seconds++;
            if (IsBusy)
            {
                return;
            }

            if (_sync > 0)
            {
                return;
            }
            _sync++;
            ThreadPool.QueueUserWorkItem(state =>
            {
                try
                {
                    foreach (var resultViewModel in ResultViewModels)
                    {
                        resultViewModel.Update();

                        BetsService
                            .Place(resultViewModel)
                            .ConfigureAwait(false);
                    }

                }
                finally
                {
                    _sync = 0;
                    RestartTimer();
                }
            });
        }

        public void UpdateHandicapWl(ResultViewModel resultViewModel)
        {
            var hadicapRow = BetsNavigator.Instance.WinlinePage.GetHadicapRow(
                resultViewModel.Team1.Names.Union(resultViewModel.Team2.Names).ToArray());

            resultViewModel.Winline.Game.HandicapElement = hadicapRow;
        }
    }
}
