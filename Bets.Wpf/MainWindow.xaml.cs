using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Bets.Domain;
using Bets.Selenium;
using Xceed.Wpf.Toolkit;

namespace Bets.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public readonly ObservableCollection<ResultViewModel> ResultViewModels;
        public FormActions FormActions { get; set; }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            ResultViewModels = new ObservableCollection<ResultViewModel>();
            ResultListView.ItemsSource = ResultViewModels;

            //FormActions = new FormActions();
        }

        private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Fetch();
        }
        
        private void Fetch()
        {
            ResultViewModels.Add(new ResultViewModel());
            ResultViewModels.Add(new ResultViewModel());
            ResultViewModels.Add(new ResultViewModel());
            ResultViewModels.Add(new ResultViewModel());
            ResultViewModels.Add(new ResultViewModel());
            ResultViewModels.Add(new ResultViewModel());
            ResultViewModels.Add(new ResultViewModel());
            ResultViewModels.Add(new ResultViewModel());

            return;


            if (BusyIndicator.IsBusy)
            {
                return;
            }

            FormActions.RestartTimer();
            BusyIndicator.IsBusy = true;

            Task.Run(() =>
            {
                var errorBuilder = new StringBuilder();
                var results = BetsNavigator.Instance.GetResults(errorBuilder);

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    ResultViewModels.Clear();
                    foreach (var result in results)
                    {
                        ResultViewModels.Add(result);
                    }

                    if (errorBuilder.Length > 0)
                    {
                        MessageBox.Show(this, errorBuilder.ToString());
                    }
                    BusyIndicator.IsBusy = false;
                }));
            });
        }
    }
}
