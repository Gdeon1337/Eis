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
    public partial class FormJournalRed : Form
    {
        public int Id { set { id = value; } }
        private int? id;

        public FormJournalRed()
        {
            InitializeComponent();
        }

        private void FormJournalRed_Load(object sender, System.EventArgs e)
        {


            String selectCommand = "Select * from Journal where Journal_id=" + id;
            selectTable(selectCommand);
            selectCommand = "Select Data from Journal where Journal_id = " + id;

            dateTimePicker1.Value = DateTime.Parse(ClassSupport.selectValue(selectCommand).ToString());
        }

        public void selectTable(String selectCommand)
        {
            dataGridView1.DataSource = ClassSupport.Connections(selectCommand);
            dataGridView1.DataMember = ClassSupport.Connections(selectCommand).Tables[0].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String selectCommand = "update Journal set Data='" + dateTimePicker1.Text + "' where Journal_id = " + id;
            ClassSupport.changeValue(selectCommand);
            //обновление dataGridView1
            selectCommand = "update Transactions set Data='" + dateTimePicker1.Text + "' where Journal_id = " + id;
            ClassSupport.changeValue(selectCommand);

            selectCommand = "Select * from Journal where Journal_id=" + id;
            selectTable(selectCommand);
            MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
