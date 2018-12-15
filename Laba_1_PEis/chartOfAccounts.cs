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
   
        public partial class chartOfAccounts : Form
        {

           


            public chartOfAccounts()
            {
                InitializeComponent();
            }
            private void chartOfAccounts_Load(object sender, EventArgs e)
            {
                
               
                String selectCommand = "Select * from Account_chart";
                selectTable(selectCommand);

            }

        public void selectTable( String selectCommand)
            {

            dataGridView1.DataSource = ClassSupport.Connections( selectCommand);
            dataGridView1.DataMember = ClassSupport.Connections( selectCommand).Tables[0].ToString();
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
