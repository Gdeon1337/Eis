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
    public partial class FormJournal : Form
    {
        public FormJournal()
        {
            InitializeComponent();
        }
        int Baseid;

        private void FormJournal_Load(object sender, System.EventArgs e)
        {


            String selectCommand = "Select * from Application";
            selectTable(selectCommand);
            selectCommand = "Select * from Product";
            selectTableProduct(selectCommand);
        }

        public void selectTable(String selectCommand)
        {

            dataGridView1.DataSource = ClassSupport.Connections(selectCommand);
            dataGridView1.DataMember = ClassSupport.Connections(selectCommand).Tables[0].ToString();

        }

        public void selectTableProduct(String selectCommand)
        {

            dataGridView2.DataSource = ClassSupport.Connections(selectCommand);
            dataGridView2.DataMember = ClassSupport.Connections(selectCommand).Tables[0].ToString();
        }

        private void UpdateDatagridProductBD(object ds)
        {

            dataGridView3.DataSource = null;
            dataGridView3.DataSource = ds;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            Baseid = Convert.ToInt32(valueId);
            string selectCommand = "Select Product_id,Count from Application_Product where Application_id=" + Convert.ToInt32(valueId);
            UpdateDatagridProductBD(ClassSupport.Connections(selectCommand).Tables[0]);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String selectCommand = "select Product_id,count from Application_Product where Application_id=" + Baseid;
            Dictionary<int, int> countApp = ClassSupport.selectValueList(selectCommand);
            Dictionary<int, int> countProd = new Dictionary<int, int>();
            foreach (int key in countApp.Keys)
            {
                selectCommand = "select Product_Count from Product where Product_id=" + key;
                countProd.Add(key,Convert.ToInt32(ClassSupport.selectValue(selectCommand)));
            }
            int check = 0;
            string nehtovar = "";
            foreach (int key in countApp.Keys)
            {
                if ( countApp[key]> countProd[key]) {
                       check = -1;
                    nehtovar += "нехвотает товара под id=" + key + "в количестве" + (countApp[key] - countProd[key]).ToString()+ "/n";
                }
            }
               if (check == -1)
               {
                   MessageBox.Show(nehtovar, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   return;
               }
               else {
                    foreach (int key in countApp.Keys)
                    {
                        selectCommand = "update Product set Product_Count='" + (Convert.ToInt32(countProd[key]) - Convert.ToInt32(countApp[key])) + "' where Product_id = " + key;
                        ClassSupport.changeValue(selectCommand);
                    }
                selectCommand = "select Sum(Sum) from Application_Product where Application_id=" + Baseid;
                int price = Convert.ToInt32(ClassSupport.selectValue(selectCommand));

                selectCommand = "select MAX(Journal_id) from Journal";
                object maxValue = ClassSupport.selectValue(selectCommand);
                if (Convert.ToString(maxValue) == "")
                    maxValue = 0;
                string txtSQLQuery = "insert into Journal (Journal_id,Data,Price,Application_id) values (" +
               (Convert.ToInt32(maxValue) + 1) + ", '" + dateTimePicker1.Text + "', '"+ price+ "', '"+ Baseid + "')";
                ClassSupport.ExecuteQuery(txtSQLQuery);
                int Jornal_id = (Convert.ToInt32(maxValue) + 1);

                selectCommand = "select MAX(Transactions_id) from Transactions";
                maxValue = ClassSupport.selectValue(selectCommand);
                   if (Convert.ToString(maxValue) == "")
                       maxValue = 0;
                selectCommand = "select Customer_id from Application where Application_id=" + Baseid;

                string customerid = ClassSupport.selectValue(selectCommand).ToString();
                
                txtSQLQuery = "insert into Transactions (Transactions_id, Debit_count, Credit_count, Count,Price, Data, Subcount_debet, Subcount_credit,Journal_id) values ('" +
                        (Convert.ToInt32(maxValue) + 1) + "', '" + "62" + "', '" + "91" + "', '" + "1" + "', '" + price + "', '" + dateTimePicker1.Text + "', '" + customerid + "', '" + Baseid + "', '" + Jornal_id+"')";
                ClassSupport.ExecuteQuery(txtSQLQuery);

                foreach (int key in countApp.Keys)
                {
                    selectCommand = "select PriceZakyp from Product where Product_id=" + key;
                    int summ = Convert.ToInt32(ClassSupport.selectValue(selectCommand)) * countApp[key];
                    selectCommand = "select MAX(Transactions_id) from Transactions";
                    maxValue = ClassSupport.selectValue(selectCommand);
                    
                    if (Convert.ToString(maxValue) == "")
                        maxValue = 0;
                    txtSQLQuery = "insert into Transactions (Transactions_id, Debit_count, Credit_count, Count,Price, Data, Subcount_debet, Subcount_credit, Journal_id) values ('" +
                        (Convert.ToInt32(maxValue) + 1) + "', '" + "91" + "', '" + "10" + "', '" + countApp[key] + "', '" + summ + "', '" + dateTimePicker1.Text + "', '" + Baseid + "', '" + key + "', '" + Jornal_id + "')";
                    ClassSupport.ExecuteQuery(txtSQLQuery);
                }
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            selectCommand = "Select * from Product";
            selectTableProduct(selectCommand);

        }
    }
}
