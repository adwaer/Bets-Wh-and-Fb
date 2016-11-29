using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bets.Services;

namespace Bats.Desktop
{
    public partial class EditNamesForm : Form
    {
        public EditNamesForm()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            var source = new AutoCompleteStringCollection();
            var teams = TeamsHolder.Instance.GetTeams();

            foreach (var team in teams)
            {
                source.Add(string.Concat(team.Names));
            }

            sourceTextBox.AutoCompleteCustomSource = source;
            sourceTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            sourceTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }
    }
}
