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
    public partial class FormReport2 : Form
    {
        public FormReport2()
        {
            InitializeComponent();
        }

        public void selectTable(String selectCommand)
        {
            
            dataGridView1.DataSource = ClassSupport.selectValueReport2(selectCommand);
            double sum = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                sum += Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value);
            }
            textBox1.Text = sum.ToString();
            dataGridView1.Columns[0].HeaderText = "Материал";
            dataGridView1.Columns[1].HeaderText = "Дата продажи";
            dataGridView1.Columns[2].HeaderText = "По заявке";
            dataGridView1.Columns[3].HeaderText = "Количество";
            dataGridView1.Columns[4].HeaderText = "Продажная стоимость";
            dataGridView1.Columns[5].HeaderText = "Себестоимость";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string select1 = "select prod.Name,st.Data,st.Subcount_credit,st.Count,prod.Price,prod.PriceZakyp from Product prod join Transactions st where st.journal_id<>-1 and st.Subcount_credit=prod.Product_id and st.Debit_count=91 AND st.Credit_count=10 AND (((day>=" + dateTimePicker1.Value.Day + " and Day<=" + dateTimePicker2.Value.Day + ") AND (Month = " + dateTimePicker1.Value.Month + " AND Month=" + dateTimePicker2.Value.Month + ") AND (Year = " + dateTimePicker1.Value.Year + " AND Year=" + dateTimePicker2.Value.Year + ")) OR (((Month >=" + dateTimePicker1.Value.Month + " and Month <" + dateTimePicker2.Value.Month + ") OR (Month >" + dateTimePicker1.Value.Month + " and Month <=" + dateTimePicker2.Value.Month + ")) AND (Year=" + dateTimePicker1.Value.Year + " and Year=" + dateTimePicker2.Value.Year + ")) OR ((Month>=" + dateTimePicker1.Value.Month + ") AND (Year>=" + dateTimePicker1.Value.Year + " AND Year < " + dateTimePicker2.Value.Year + ")) OR ((Month<=" + dateTimePicker2.Value.Month + ") AND (Year>" + dateTimePicker1.Value.Year + " AND Year <= " + dateTimePicker2.Value.Year + ")))";
            selectTable(select1);
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
                Microsoft.Office.Interop.Excel.Range _excelCells1 = (Microsoft.Office.Interop.Excel.Range)ExcelWorkSheet.get_Range("A1", "D1").Cells;
                // Производим объединение
                _excelCells1.Merge(Type.Missing);
                ExcelWorkSheet.Cells[1, 1] = " Ведомость проданных товаров за период с " +dateTimePicker1.Text + " по " + dateTimePicker2.Text ;

                
                ExcelApp.Cells[2, 1] = dataGridView1.Columns[0].HeaderText;
                ExcelApp.Cells[2, 2] = dataGridView1.Columns[1].HeaderText;
                ExcelApp.Cells[2, 3] = dataGridView1.Columns[2].HeaderText;
                ExcelApp.Cells[2, 4] = dataGridView1.Columns[3].HeaderText;
                ExcelApp.Cells[2, 5] = dataGridView1.Columns[4].HeaderText;
                for (int i = 2; i < dataGridView1.Rows.Count + 2; i++)
                {
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        ExcelApp.Cells[i + 1, j + 1] = dataGridView1.Rows[i - 2].Cells[j].Value;
                    }
                }
                ExcelApp.Cells[dataGridView1.Rows.Count+4,  4] = "ИТОГО";
                ExcelApp.Cells[dataGridView1.Rows.Count + 4, 5] = textBox1.Text;


                Microsoft.Office.Interop.Excel.Range tRange = ExcelWorkSheet.get_Range("A2", "E" + (Convert.ToInt32(dataGridView1.Rows.Count) + 2).ToString()).Cells;
                tRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                tRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                tRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                tRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                tRange.WrapText = true;
                tRange.WrapText = true;
                //Вызываем нашу созданную эксельку.
                ExcelApp.Visible = true;
                ExcelApp.UserControl = true;
            }
        }
    }
}
