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
    public partial class JournalEntries : Form
    {
        public JournalEntries()
        {
            InitializeComponent();
        }

        private void JournalEntries_Load(object sender, System.EventArgs e)
        {

            String selectCommand = "Select * from Transactions";
            selectTable(selectCommand);
            selectCommand = "Select * from Product";
            selectComboboxProduct(selectCommand);
            selectCommand = "Select * from Customer";
            selectComboboxCustomer(selectCommand);
        }

        public void selectTable(String selectCommand)
        {
            dataGridView1.DataSource = ClassSupport.Connections(selectCommand);
            dataGridView1.DataMember = ClassSupport.Connections(selectCommand).Tables[0].ToString();
        }

        public void selectComboboxProduct(String selectCommand)
        {
            comboBoxProduct.ValueMember = "Product_id";
            comboBoxProduct.DisplayMember = "Name";
            comboBoxProduct.DataSource = ClassSupport.Connections(selectCommand).Tables[0];
            comboBoxProduct.SelectedItem = null;

        }
        public void selectComboboxCustomer(String selectCommand)
        {
            comboBoxCustom.ValueMember = "Customer_id";
            comboBoxCustom.DisplayMember = "Name";
            comboBoxCustom.DataSource = ClassSupport.Connections(selectCommand).Tables[0];
            comboBoxCustom.SelectedItem = null;

        }
        public void refreshForm(String selectCommand)
        {
            selectTable(selectCommand);
            dataGridView1.Update();
            dataGridView1.Refresh();
            textBoxCount.Text = "";
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxCustom.SelectedValue == null)
            {
                MessageBox.Show("Заполните ФИО Поставщика", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(dateTimePicker1.Text))
            {
                MessageBox.Show("Выберете дату", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxProduct.SelectedValue == null)
            {
                MessageBox.Show("Выберите товар", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            String selectCommand = "select PriceZakyp from Product where Product_id=" + comboBoxProduct.SelectedValue;
            int price = Convert.ToInt32(ClassSupport.selectValue(selectCommand));

            selectCommand = "select Name from Product where Product_id=" + comboBoxProduct.SelectedValue;
            string name = ClassSupport.selectValue(selectCommand).ToString();

            selectCommand = "select MAX(Transactions_id) from Transactions";
            object maxValue = ClassSupport.selectValue(selectCommand);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;
            string txtSQLQuery = "insert into Transactions (Transactions_id, Debit_count, Credit_count, Count,Price, Data, Subcount_debet, Subcount_credit,Day,Month,Year,Journal_id) values ('" +
           (Convert.ToInt32(maxValue) + 1) + "', '" + "10" + "', '" + "60"+"', '" + textBoxCount.Text + "', '" + (Convert.ToInt32(textBoxCount.Text)*price)+ "', '"+ dateTimePicker1.Text + "', '" + Convert.ToInt32(comboBoxProduct.SelectedValue) + "', '" + Convert.ToInt32(comboBoxCustom.SelectedValue) + "', '" + dateTimePicker1.Value.Day + "', '" + dateTimePicker1.Value.Month + "', '" + dateTimePicker1.Value.Year + "', '"+ "-1" + "')";
            ClassSupport.ExecuteQuery(txtSQLQuery);

            selectCommand = "select * from Transactions";
            refreshForm(selectCommand);
        }

        private void Del_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                string add = "delete from Transactions where Transactions_id=" + Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value) + " And Journal_id=-1";
                ClassSupport.changeValue(add);
                String selectCommand = "Select * from Transactions";
                selectTable(selectCommand);

            }
        }
    }
}
