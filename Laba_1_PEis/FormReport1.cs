
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
    public partial class FormReport1 : Form
    {
        public FormReport1()
        {
            InitializeComponent();
        }

        private void FormOtchets_Load(object sender, EventArgs e)
        {
        }

        public void selectTable(String selectCommand,string select)
        {
            List<ListOtchet> list = ClassSupport.selectValueReport1(selectCommand);
            list.AddRange(ClassSupport.selectValueReport1_dop(select));
            dataGridView1.DataSource = list;
            double sum = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                sum += Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
            }
            dataGridView1.Columns[0].HeaderText = "Дата заявки";
            dataGridView1.Columns[1].HeaderText = "Номер Заявки";
            dataGridView1.Columns[2].HeaderText = "Заявлено товаров на сумму";
            dataGridView1.Columns[3].HeaderText = "Отгружено по заявке";
            textBox1.Text = sum.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string select1 = "select Data,Subcount_credit,Price from Transactions where journal_id<>-1 and Debit_count=62 AND Credit_count=91 AND (((day>=" + dateTimePicker1.Value.Day + " and Day<=" + dateTimePicker2.Value.Day + ") AND (Month = " + dateTimePicker1.Value.Month + " AND Month=" + dateTimePicker2.Value.Month + ") AND (Year = " + dateTimePicker1.Value.Year + " AND Year=" + dateTimePicker2.Value.Year + ")) OR (((Month >=" + dateTimePicker1.Value.Month + " and Month <" + dateTimePicker2.Value.Month + ") OR (Month >" + dateTimePicker1.Value.Month + " and Month <=" + dateTimePicker2.Value.Month + ")) AND (Year=" + dateTimePicker1.Value.Year + " and Year=" + dateTimePicker2.Value.Year + ")) OR ((Month>=" + dateTimePicker1.Value.Month + ") AND (Year>=" + dateTimePicker1.Value.Year + " AND Year < " + dateTimePicker2.Value.Year + ")) OR ((Month<=" + dateTimePicker2.Value.Month + ") AND (Year>" + dateTimePicker1.Value.Year + " AND Year <= " + dateTimePicker2.Value.Year + ")))";

            string select = "select DISTINCT ap.Application_id, appprod.Sum from Application ap join Application_Product appprod join Transactions t where appprod.Application_id=ap.Application_id and t.journal_id<>-1 and t.Debit_count=62 AND t.Credit_count=91  AND t.Subcount_credit<>ap.Application_id GROUP BY ap.Application_id";
            selectTable(select1,select);
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

                Microsoft.Office.Interop.Excel.Range _excelCells1 = (Microsoft.Office.Interop.Excel.Range)ExcelWorkSheet.get_Range("A1", "F1").Cells;
                // Производим объединение
                _excelCells1.Merge(Type.Missing);
                ExcelWorkSheet.Cells[1, 1] = " Ведомость заявок за период с " + dateTimePicker1.Text + " по " + dateTimePicker2.Text;


                ExcelApp.Cells[2, 1] = "Дата заявки";
                ExcelApp.Cells[2, 2] = "Номер Заявки";
                ExcelApp.Cells[2, 3] = "Заявлено товаров на сумму";
                ExcelApp.Cells[2, 4] = "Отгружено по заявке";
                for (int i = 2; i < dataGridView1.Rows.Count + 2; i++)
                {
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        ExcelApp.Cells[i + 1, j + 1] = dataGridView1.Rows[i - 2].Cells[j].Value;
                    }
                }
                ExcelApp.Cells[dataGridView1.Rows.Count+4, 3] = "ИТОГО";
                ExcelApp.Cells[dataGridView1.Rows.Count + 4, 4] = textBox1.Text;

                Microsoft.Office.Interop.Excel.Range tRange = ExcelWorkSheet.get_Range("A2", "D" + (Convert.ToInt32(dataGridView1.Rows.Count) + 2).ToString()).Cells;
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
