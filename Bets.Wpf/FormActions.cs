using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Bets.Domain;
using Bets.Selenium;
using Bets.Services;
using Xceed.Wpf.Toolkit;

namespace Bets.Wpf
{
    public class FormActions : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<ResultViewModel> ResultViewModels { get; set; }
        public BetsService BetsService { get; set; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsBusy { get; set; }
        public bool IsUpdating { get; set; }

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
                    BetsService.Place(resultViewModel);
                    //if (resultViewModel.IsGoodTotal.Value != 0 && resultViewModel.IsTotalNotPrev())
                    //{
                    //    BetsService.Place(resultViewModel.Team1, resultViewModel.Team2, resultViewModel.IsGoodTotal.Value, resultViewModel.AmountTotal, "TOTAL", resultViewModel.Fonbet.Total.Value, resultViewModel.Winline.Total.Value);
                    //}
                    //if (resultViewModel.IsGoodHc.Value != 0 && resultViewModel.IsHcNotPrev())
                    //{
                    //    BetsService.Place(resultViewModel.Team1, resultViewModel.Team2, resultViewModel.IsGoodHc.Value, resultViewModel.AmountHandicap, "HC", resultViewModel.Fonbet.Handicap.Value, resultViewModel.Winline.Handicap.Value);
                    //}
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
