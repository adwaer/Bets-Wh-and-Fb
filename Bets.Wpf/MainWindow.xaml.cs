using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Bets.Domain;
using Bets.Selenium;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace Bets.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public FormActions FormActions { get; set; }

        public MainWindow()
        {
            FormActions = new FormActions();
            InitializeComponent();
        }

        public void Fetch(object sender, System.Windows.RoutedEventArgs e)
        {
            if (FormActions.IsBusy)
            {
                return;
            }

            FormActions.RestartTimer();
            BusyIndicator.IsBusy = FormActions.IsBusy = true;

            Task.Run(() =>
            {
                var errorBuilder = new StringBuilder();
                List<ResultViewModel> results = null;
                try
                {
                    results = BetsNavigator.Instance.GetResults(errorBuilder);
                }
                catch (Exception ex)
                {
                    errorBuilder.AppendLine(ex.ToString());
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    FormActions.ResultViewModels.Clear();
                    if (results != null)
                    {
                        foreach (var result in results)
                        {
                            FormActions.ResultViewModels.Add(result);
                        }
                    }

                    if (errorBuilder.Length > 0)
                    {
                        MessageBox.Show(this, errorBuilder.ToString());
                    }
                    BusyIndicator.IsBusy = FormActions.IsBusy = false;
                }));
            });
        }

        private void RemoveBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var teamViewModel = (TeamViewModel)button.Tag;

            var resultViewModel = FormActions.ResultViewModels.First(r => Equals(r.Team1, teamViewModel));
            FormActions.ResultViewModels.Remove(resultViewModel);
        }

        private void UpdateBase_OnClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var teamViewModel = (TeamViewModel)button.Tag;

            var resultViewModel = FormActions.ResultViewModels.First(r => Equals(r.Team1, teamViewModel));
            FormActions.UpdateHandicapWl(resultViewModel);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            BetsNavigator.Instance.WinlinePage.Dispose();
            BetsNavigator.Instance.FonbetPage.Dispose();
            base.OnClosing(e);
        }
    }

}