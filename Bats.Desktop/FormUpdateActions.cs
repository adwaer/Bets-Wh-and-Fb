using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bets.Domain;
using Bets.Domain.PageElements;
using Bets.Selenium;
using OpenQA.Selenium;

namespace Bats.Desktop
{
    public class FormUpdateActions
    {
        public int WaitSeconds;
        private readonly MainForm1 _form;
        public bool IsLoading;
        public bool IsRefetch;
        private FetchTransactions _currentTransaction;
        public ListViewGroup DefaultGroup;
        public List<FetchTransactions> FetchTransactionses;

        public void RunWithInvoke(Action action, Control control)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        private BetsNavigator _betsNavigator;

        public BetsNavigator BetsNavigator
        {
            get
            {
                if (_betsNavigator == null)
                {
                    lock (this)
                    {
                        try
                        {
                            _betsNavigator = BetsNavigator.Instance;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }

                return _betsNavigator;
            }
        }

        public FormUpdateActions(MainForm1 form)
        {
            _form = form;
            FetchTransactionses = new List<FetchTransactions>();
            Task.Run(() => RunWithInvoke(Fetch, form.listView));
        }

        public void WaitTimerOnTick(object sender, EventArgs eventArgs)
        {
            _form.FetchCountLabel.Text = WaitSeconds++.ToString();
            if (!IsRefetch && !IsLoading)
            {
                WaitSeconds = 0;
                IsRefetch = true;
                Task.Run(() =>
                {
                    RefetchListView();
                });
            }
        }

        public void Fetch()
        {
            if (IsLoading)
            {
                return;
            }
            IsLoading = true;

            _form.CounterLabel.Text = "Выполняется";
            WaitSeconds = 1;
            _form.FetchCountLabel.Text = "0";

            _currentTransaction = new FetchTransactions
            {
                ErrorBuilder = new StringBuilder()
            };
            FetchTransactionses.Add(_currentTransaction);

            var listViewItem = new ListViewItem
            {
                Text = $"тран:{_form.listView.Items.Count}",
                BackColor = Color.AliceBlue,
                Tag = _currentTransaction
            };
            _form.HistoryListView.Items.Add(listViewItem);

            var taskWn = Task.Run(() => BetsNavigator.WinlinePage.GetTotalRows(_currentTransaction.ErrorBuilder));
            var taskWn1 = Task.Run(() => BetsNavigator.WinlinePage1.GetHadicapRows(_currentTransaction.ErrorBuilder));
            var taskFb = Task.Run(() => BetsNavigator.FonbetPage.GetFonbetRows());

            Task.Run(() =>
            {
                _currentTransaction.WinlineRows = taskWn.Result;
                _currentTransaction.WinlineRows1 = taskWn1.Result;
                _currentTransaction.FonbetRows = taskFb.Result;

                CompleteRequest(listViewItem);
            });
        }

        private void RefetchListView()
        {
            RunWithInvoke(() =>
            {
                foreach (ListViewItem item in _form.listView.Items)
                {
                    var totalFbSubItem = item.SubItems[2];
                    string totalFbText;
                    try
                    {
                        var webElement = (IWebElement)totalFbSubItem.Tag;
                        totalFbText = webElement.Text;
                    }
                    catch
                    {
                        totalFbText = "-";
                    }
                    totalFbSubItem.BackColor = totalFbSubItem.Text == totalFbText ? Color.Empty : Color.GreenYellow;
                    totalFbSubItem.Text = totalFbText;

                    //totalFb.Text

                    var handicapFbSubItem = item.SubItems[3];
                    string handicapFbText;
                    try
                    {
                        var webElement = (IWebElement)handicapFbSubItem.Tag;
                        handicapFbText = webElement.Text;
                    }
                    catch
                    {
                        handicapFbText = "-";
                    }
                    handicapFbSubItem.BackColor = handicapFbSubItem.Text == handicapFbText ? Color.Empty : Color.GreenYellow;
                    handicapFbSubItem.Text = handicapFbText;

                    var totalWnSubItem = item.SubItems[4];
                    string totalWnText;
                    try
                    {
                        var webElement = (IWebElement)totalWnSubItem.Tag;
                        totalWnText = webElement.Text;
                    }
                    catch
                    {
                        totalWnText = "-";
                    }
                    totalWnSubItem.BackColor = totalWnSubItem.Text == totalWnText ? Color.Empty : Color.GreenYellow;
                    totalWnSubItem.Text = totalWnText;

                    var handicapWnSubItem = item.SubItems[5];
                    string handicaplWnText;
                    try
                    {
                        var webElement = (IWebElement)handicapWnSubItem.Tag;
                        handicaplWnText = webElement.Text.Split(new[] { Environment.NewLine },
                            StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    catch
                    {
                        handicaplWnText = (IWebElement)handicapWnSubItem.Tag != null ? "-" : "not found";
                    }
                    handicapWnSubItem.BackColor = handicapWnSubItem.Text == handicaplWnText ? Color.Empty : Color.GreenYellow;
                    handicapWnSubItem.Text = handicaplWnText;
                }

                IsRefetch = false;
            }, _form.listView);
        }

        private void CompleteRequest(ListViewItem listViewItem)
        {
            RunWithInvoke(() =>
            {
                listViewItem.BackColor = Color.DodgerBlue;

                if (_form.HistoryListView.Items.Count > 40)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        _form.HistoryListView.Items.RemoveAt(i);
                    }
                }

                _form.listView.Items.Clear();
                _currentTransaction.FonbetRows = _currentTransaction.FonbetRows
                    .OrderBy(r => r.Team1.ToString());

                foreach (var fonbetGame in _currentTransaction.FonbetRows)
                {
                    var gamesWl = _currentTransaction.WinlineRows
                        .Where(r => r.Team1.Equals(fonbetGame.Team1) || r.Team2.Equals(fonbetGame.Team2))
                        .ToArray();

                    if (gamesWl.Length > 1)
                    {
                        _currentTransaction.ErrorBuilder.Append("Найдено слишком много матчей по названиям: ");
                        FillTeamsNames(fonbetGame, _currentTransaction.ErrorBuilder);
                        _currentTransaction.ErrorBuilder.AppendLine();

                        continue;
                    }
                    if (!gamesWl.Any())
                    {
                        _currentTransaction.ErrorBuilder.Append("Не найдены совпадения: ");
                        FillTeamsNames(fonbetGame, _currentTransaction.ErrorBuilder);
                        _currentTransaction.ErrorBuilder.AppendLine();

                        continue;
                    }

                    var gameWh = gamesWl.Single();
                    var gameWl1 = _currentTransaction.WinlineRows1
                        .SingleOrDefault(r => r.Team1.Equals(fonbetGame.Team1) || r.Team2.Equals(fonbetGame.Team2));


                    string team1 = null;
                    string team2 = null;
                    string totalFb = null;
                    string totalWh = null;
                    string handicapFb = null;
                    string handicapWh = null;
                    try
                    {
                        team1 = gameWh.Team1.ToString();
                    }
                    catch
                    {
                        // ignored
                    }
                    try
                    {
                        team2 = gameWh.Team2.ToString();
                    }
                    catch
                    {
                        // ignored
                    }
                    try
                    {
                        totalFb = fonbetGame.TotalElement.Text;
                    }
                    catch
                    {
                        // ignored
                    }
                    try
                    {
                        totalWh = gameWh.TotalElement.Text;
                    }
                    catch
                    {
                        // ignored
                    }
                    try
                    {
                        handicapFb = fonbetGame.HandicapElement.Text;
                    }
                    catch
                    {
                        // ignored
                    }
                    try
                    {
                        handicapWh = gameWl1?.HandicapElement.Text.Split(new[] { Environment.NewLine },
                            StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    catch
                    {
                        // ignored
                    }

                    var gamesListViewItem = new ListViewItem { Text = team1 ?? "-" };
                    gamesListViewItem.SubItems.Add(team2 ?? "-");

                    gamesListViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(gamesListViewItem, totalFb ?? "-")
                    {
                        BackColor = totalFb == null ? Color.DeepPink : Color.Empty,
                        Tag = fonbetGame.TotalElement
                    });
                    gamesListViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(gamesListViewItem, handicapFb ?? "-")
                    {
                        BackColor = handicapFb == null ? Color.DeepPink : Color.Empty,
                        Tag = fonbetGame.HandicapElement
                    });
                    gamesListViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(gamesListViewItem, totalWh ?? "-")
                    {
                        BackColor = totalWh == null ? Color.DeepPink : Color.Empty,
                        Tag = gameWh.TotalElement
                    });
                    gamesListViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(gamesListViewItem, handicapWh ?? (gameWl1 != null ? "-" : "not found"))
                    {
                        BackColor = handicapWh == null ? Color.DeepPink : Color.Empty,
                        Tag = gameWl1?.HandicapElement
                    });

                    _form.listView.Items.Add(gamesListViewItem);
                }
                listViewItem.ForeColor = _currentTransaction.ErrorBuilder.Length > 0 ? Color.Red : Color.Green;
                listViewItem.Tag = _currentTransaction;
                
                _currentTransaction.ErrorBuilder.AppendLine();
                _currentTransaction.ErrorBuilder.AppendLine($"Время выполнения: {WaitSeconds}");

                WaitSeconds = 0;
                IsLoading = false;
            }, _form.listView);
        }

        private static void FillTeamsNames(IRow row, StringBuilder errorsBuilder)
        {
            foreach (var name in row.Team1?.Names.Union(row.Team2?.Names))
            {
                errorsBuilder.Append($"{name};");
            }
        }

    }
}
