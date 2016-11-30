using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
    public partial class MainForm1 : Form
    {
        private FormUpdateActions _formUpdateActions;

        public MainForm1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            int height, width;
            if (!int.TryParse(ConfigurationManager.AppSettings["formHeight"], out height))
            {
                height = Height;
            }
            if (!int.TryParse(ConfigurationManager.AppSettings["formWidth"], out width))
            {
                width = Width;
            }

            var columnWidths = ConfigurationManager.AppSettings["columnWidths"];
            if (!string.IsNullOrEmpty(columnWidths))
            {
                try
                {
                    var cw = columnWidths
                        .Split(';')
                        .Select(int.Parse)
                        .ToArray();

                    for (int i = 0; i < listView.Columns.Count && i < cw.Length; i++)
                    {
                        var columnHeader = listView.Columns[i];
                        columnHeader.Width = cw[i];
                    }
                }
                catch
                {
                    MessageBox.Show("Невозможно установить ширину колонок");
                }
            }

            Height = height;
            Width = width;

            _formUpdateActions = new FormUpdateActions(this);

            waitTimer.Tick += _formUpdateActions.WaitTimerOnTick;
            waitTimer.Start();
        }

        private void HistoryListView_ItemActivate(object sender, EventArgs e)
        {
            var listViewItem = HistoryListView.SelectedItems[0];
            var transactions = (FetchTransactions)listViewItem.Tag;

            var messageForm = new MessageForm(transactions.ErrorBuilder.ToString());
            messageForm.ShowDialog(this);
        }

        private void refreshMenuItem_Click(object sender, EventArgs e)
        {
            Task.Run(() => _formUpdateActions.RunWithInvoke(_formUpdateActions.Fetch, listView));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            config.AppSettings.Settings.Remove("formHeight");
            config.AppSettings.Settings.Add("formHeight", Height.ToString());
            config.AppSettings.Settings.Remove("formWidth");
            config.AppSettings.Settings.Add("formWidth", Width.ToString());

            var widths = string.Join(";", listView.Columns.Cast<ColumnHeader>()
                .Select(c => c.Width));
            config.AppSettings.Settings.Remove("columnWidths");
            config.AppSettings.Settings.Add("columnWidths", widths);

            config.Save(ConfigurationSaveMode.Modified);

            _formUpdateActions.BetsNavigator.Dispose();
            base.OnClosing(e);
        }

        private void removeMenuItem_Click(object sender, EventArgs e)
        {
            _formUpdateActions.IsRefetch = true;
            Task.Delay(5000)
                .ContinueWith(task =>
                {
                    try
                    {
                        _formUpdateActions.RunWithInvoke(() =>
                        {
                            foreach (ListViewItem selectedItem in listView.SelectedItems)
                            {
                                listView.Items.Remove(selectedItem);
                            }

                        }, listView);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, "Ошибка удаления");
                    }
                    _formUpdateActions.IsRefetch = false;
                });
        }
    }
}