﻿using System;
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
            string ConnectionString = @"Data Source=" + ClassSupport.sPath +
           ";New=False;Version=3";
            String selectCommand = "Select * from Customer";
            selectTable(ConnectionString, selectCommand);
        }

        public void selectTable(string ConnectionString, String selectCommand)
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
            refreshForm(ClassSupport.ConnectionString, selectCommand);

        }
        public void refreshForm(string ConnectionString, String selectCommand)
        {
            selectTable(ConnectionString, selectCommand);
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
                string ConnectionString = @"Data Source=" + ClassSupport.sPath +
               ";New=False;Version=3";
                ClassSupport.changeValue(selectCommand);
                //обновление dataGridView1
                selectCommand = "select * from Customer";
                refreshForm(ConnectionString, selectCommand);
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
            string ConnectionString = @"Data Source=" + ClassSupport.sPath + ";New=False;Version=3";
            ClassSupport.changeValue(selectCommand);
            //обновление dataGridView1
            selectCommand = "select * from Customer";
            refreshForm(ConnectionString, selectCommand);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение Name выбранной строки
            string nameId = dataGridView1[1, CurrentRow].Value.ToString();
     
            textBoxName.Text = nameId;
     
        }
    }
}