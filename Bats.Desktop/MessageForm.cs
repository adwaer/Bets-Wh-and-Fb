using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bats.Desktop
{
    public partial class MessageForm : Form
    {
        public MessageForm(string msg)
        {
            InitializeComponent();
            richTextBox1.Text = msg;
        }
    }
}
