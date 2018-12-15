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

        private void сохранитьВЕксельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource != null)
            {
                Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook;
                Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet;
                //Книга.
                ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value);
                //Таблица.
                ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);
                ExcelApp.Cells[1, 1] = dataGridView1.Columns[0].Name;
                ExcelApp.Cells[1, 2] = dataGridView1.Columns[1].Name;
                ExcelApp.Cells[1, 3] = dataGridView1.Columns[2].Name;
                ExcelApp.Cells[1, 4] = dataGridView1.Columns[3].Name;
                ExcelApp.Cells[1, 5] = dataGridView1.Columns[4].Name;
                ExcelApp.Cells[1, 6] = dataGridView1.Columns[5].Name;
                ExcelApp.Cells[1, 7] = dataGridView1.Columns[6].Name;
                ExcelApp.Cells[1, 8] = dataGridView1.Columns[7].Name;
                ExcelApp.Cells[1, 9] = dataGridView1.Columns[8].Name;
                ExcelApp.Cells[1, 10] = dataGridView1.Columns[9].Name;
                ExcelApp.Cells[1, 11] = dataGridView1.Columns[10].Name;
                ExcelApp.Cells[1, 12] = dataGridView1.Columns[11].Name;
                for (int i = 1; i < dataGridView1.Rows.Count + 1; i++)
                {
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        ExcelApp.Cells[i + 1, j + 1] = dataGridView1.Rows[i - 1].Cells[j].Value;
                    }
                }
                //Вызываем нашу созданную эксельку.
                ExcelApp.Visible = true;
                ExcelApp.UserControl = true;
            }
        }
    }
}
