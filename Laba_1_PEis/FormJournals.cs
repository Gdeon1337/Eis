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
    public partial class FormJournals : Form
    {
        public FormJournals()
        {
            InitializeComponent();
        }
        private void FormJournals_Load(object sender, System.EventArgs e)
        {
            LoadData();
            
        }
        private void LoadData() {
            dataGridView1.CurrentCell = null;
            String selectCommand = "Select * from Journal";
            selectTable(selectCommand);
            dataGridView2.Enabled=false;
        }

        public void selectTable(String selectCommand)
        {
            dataGridView1.DataSource = ClassSupport.Connections(selectCommand);
            dataGridView1.DataMember = ClassSupport.Connections(selectCommand).Tables[0].ToString();
        }

        public void UpdateDatagridTransactions(String selectCommand)
        {
            dataGridView2.DataSource = ClassSupport.Connections(selectCommand);
            dataGridView2.DataMember = ClassSupport.Connections(selectCommand).Tables[0].ToString();
            dataGridView2.CurrentCell = null;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormJournal();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void buttonRed_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                var form = new FormJournalRed();
                form.Id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            string selectCommand = "Select * from Transactions where Journal_id=" + Convert.ToInt32(valueId);
            UpdateDatagridTransactions(selectCommand);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                string del = "delete from Journal where Journal_id=" + Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                ClassSupport.changeValue(del);
                del = "delete from Transactions where Journal_id=" + Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                ClassSupport.changeValue(del);
                del  = "update Journal_Product set Journal_id = -1 where Journal_id="+Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                ClassSupport.changeValue(del);
                LoadData();
            }
        }
    }
}
