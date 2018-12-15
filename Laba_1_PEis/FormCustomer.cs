using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba_1_PEis
{
    public partial class FormCustomer : Form
    {
       
        



        public FormCustomer()
        {
            InitializeComponent();
        }

        private void FormCustomer_Load(object sender, System.EventArgs e)
        {
            String selectCommand = "Select * from Customer";
            selectTable(selectCommand);
        }

        public void selectTable(String selectCommand)
        {

            dataGridView1.DataSource = ClassSupport.Connections( selectCommand);
            dataGridView1.DataMember = ClassSupport.Connections( selectCommand).Tables[0].ToString();

        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            
            String selectCommand = "select MAX(Customer_id) from Customer";
            object maxValue = ClassSupport.selectValue( selectCommand);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;
            string txtSQLQuery = "insert into Customer (Customer_id, Name) values (" +
           (Convert.ToInt32(maxValue) + 1) + ", '" + textBoxName.Text  + "')";
            ClassSupport.ExecuteQuery(txtSQLQuery);
            //обновление dataGridView1
            selectCommand = "select * from Customer";
            refreshForm(selectCommand);

        }
        public void refreshForm(String selectCommand)
        {
            selectTable(selectCommand);
            dataGridView1.Update();
            dataGridView1.Refresh();
            textBoxName.Text = "";

        }

       


        

        private void del_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                //выбрана строка CurrentRow
                int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;

                string valueId = dataGridView1[0, CurrentRow].Value.ToString();
                String selectCommand = "delete from Customer where Customer_id=" + valueId;
                ClassSupport.changeValue(selectCommand);
                //обновление dataGridView1
                selectCommand = "select * from Customer";
                refreshForm(selectCommand);
            }
        }


        



        private void redaction_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }




            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение Name выбранной строки
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            string changeName = textBoxName.Text;

            //обновление Name
            String selectCommand = "update Customer set Name='" + changeName + "' where Customer_id = " + valueId;
            ClassSupport.changeValue(selectCommand);
            //обновление dataGridView1
            selectCommand = "select * from Customer";
            refreshForm(selectCommand);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение Name выбранной строки
            string nameId = dataGridView1[1, CurrentRow].Value.ToString();
     
            textBoxName.Text = nameId;
     
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
