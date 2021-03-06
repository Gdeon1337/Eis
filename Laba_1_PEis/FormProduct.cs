﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba_1_PEis
{
    public partial class FormProduct : Form
    {

       



        public FormProduct()
        {
            InitializeComponent();
        }

        private void FormProduct_Load(object sender, System.EventArgs e)
        {
           
            String selectCommand = "Select * from Product";
            selectTable(selectCommand);
        }

        public void selectTable(String selectCommand)
        {
            dataGridView1.DataSource = ClassSupport.Connections( selectCommand);
            dataGridView1.DataMember = ClassSupport.Connections(selectCommand).Tables[0].ToString();
            dataGridView1.Columns[3].Visible = false;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPriceZakyp.Text))
            {
                MessageBox.Show("Заполните закупочную цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            String selectCommand = "select MAX(Product_id) from Product";
            object maxValue = ClassSupport.selectValue( selectCommand);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;
            string txtSQLQuery = "insert into Product (Product_id, Name,Price,PriceZakyp) values (" +
           (Convert.ToInt32(maxValue) + 1) + ", '" + textBoxName.Text + "', '" + Convert.ToDouble(textBoxPrice.Text)+ "', '" + Convert.ToDouble(textBoxPriceZakyp.Text) + "')";
            ClassSupport.ExecuteQuery(txtSQLQuery);
            //обновление dataGridView1
            selectCommand = "select * from Product";
            refreshForm(selectCommand);
           
        }
        public void refreshForm(String selectCommand)
        {
            selectTable(selectCommand);
            dataGridView1.Update();
            dataGridView1.Refresh();
            textBoxName.Text = "";
            textBoxPrice.Text = "";
            textBoxPriceZakyp.Text = "";
        }

        


        

        private void del_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                //выбрана строка CurrentRow
                int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;

                string valueId = dataGridView1[0, CurrentRow].Value.ToString();
                String selectCommand = "delete from Product where Product_id=" + valueId;
                ClassSupport.changeValue(selectCommand);

                selectCommand = "delete from Application_Product where Product_id=" + valueId;
                ClassSupport.changeValue(selectCommand);

                selectCommand = "select * from Product";
                refreshForm(selectCommand);
            }
        }


        

        private void textBoxPrice_KeyPress(object sender, KeyPressEventArgs e)
        {

            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8 && number != 44)
            {
                e.Handled = true;
            }

            if (textBoxPrice.SelectionStart == 0 & e.KeyChar == ',')
            {
                e.Handled = true;
            }
           
            if (textBoxPrice.Text == "0")
            {
                if (e.KeyChar != ',' & e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }

            if (textBoxPrice.Text.IndexOf(',') > 0)
            {
                if (textBoxPrice.Text.Substring(textBoxPrice.Text.IndexOf(',')).Length > 2)
                {
                    if (e.KeyChar != (char)Keys.Back)
                    {
                        e.Handled = true;
                    }
                }
            }
           
            if (e.KeyChar == ',')
            {
                if (textBoxPrice.Text.IndexOf(',') != -1)
                {
                    e.Handled = true;
                }

            }


        }

        private void redaction_Click(object sender, EventArgs e)
            {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPriceZakyp.Text))
            {
                MessageBox.Show("Заполните закупочную цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение Name выбранной строки
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            string changeName = textBoxName.Text;
            string changePrice = Convert.ToDouble(textBoxPrice.Text).ToString();
            string changePriceZakyp = Convert.ToDouble(textBoxPriceZakyp.Text).ToString();
            //обновление Name
            String selectCommand = "update Product set Name='" + changeName + "' ,Price='"+ changePrice + "' ,PriceZakyp='" + changePriceZakyp + "' where Product_id = " + valueId;
             string ConnectionString = @"Data Source=" + ClassSupport.sPath +";New=False;Version=3";
            ClassSupport.changeValue(selectCommand);
            //обновление dataGridView1
            selectCommand = "select * from Product";
            refreshForm(selectCommand);
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение Name выбранной строки
            string nameId = dataGridView1[1, CurrentRow].Value.ToString();
            string price = dataGridView1[2, CurrentRow].Value.ToString();
            string priceZakyp = dataGridView1[3, CurrentRow].Value.ToString();
            textBoxName.Text = nameId;
            textBoxPrice.Text = price;
            textBoxPriceZakyp.Text = priceZakyp;
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

