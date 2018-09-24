using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba_1_PEis
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void планСчетовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new chartOfAccounts();
            form.ShowDialog();
        }

        private void материалыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormProduct();
            form.ShowDialog();
        }

        private void покупательToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormCustomer();
            form.ShowDialog();
        }

        private void заявкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormApplications();
            form.ShowDialog();
        }
    }
}
