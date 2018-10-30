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

        private List<ClassSupport.Product_count> product_Counts = new List<ClassSupport.Product_count>();

        private void FormJournal_Load(object sender, System.EventArgs e)
        {


            String selectCommand = "Select * from Application";
            selectTable(selectCommand);
            selectCommand = "Select Product_id,Sum,Count,Data,day,Month,God from Journal_Product where (day=" + dateTimePicker1.Value.Day + " OR day <" + dateTimePicker1.Value.Day + ") AND (Month=" + dateTimePicker1.Value.Month + " OR Month <" + dateTimePicker1.Value.Month + ") AND (God = " + dateTimePicker1.Value.Year + " OR God <" + dateTimePicker1.Value.Year + ")";
            product_Counts = ClassSupport.selectValueProduct(selectCommand);
            for (int i = 0; i < product_Counts.Count; i++)
            {
                int stak = product_Counts[i].Material_id;
                for (int j = i + 1; j < product_Counts.Count; j++)
                {
                    if (stak == product_Counts[j].Material_id)
                    {
                        product_Counts[j].Material_id = -1;
                        product_Counts[i].Count += product_Counts[j].Count;
                        product_Counts[i].Sum += product_Counts[j].Sum;
                    }
                }
            }
            product_Counts.RemoveAll(rec => rec.Material_id == -1);
            selectTableProduct(product_Counts);
            dataGridView2.Enabled = false;
            dataGridView3.Enabled = false;
            dataGridView1.CurrentCell = null;

        }

        public void selectTable(String selectCommand)
        {

            dataGridView1.DataSource = ClassSupport.Connections(selectCommand);
            dataGridView1.DataMember = ClassSupport.Connections(selectCommand).Tables[0].ToString();

        }

        public void selectTableProduct(object ds)
        {
            
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = ds;
            dataGridView2.Columns[3].Visible = false;
            dataGridView2.Columns[4].Visible = false;
            dataGridView2.Columns[5].Visible = false;
            dataGridView2.Columns[6].Visible = false;
            dataGridView2.CurrentCell = null;


        }

        private void UpdateDatagridProductBD(object ds)
        {

            dataGridView3.DataSource = null;
            dataGridView3.DataSource = ds;
            dataGridView3.CurrentCell = null;
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
                selectCommand = "select SUM(Count) from Journal_Product where Product_id=" + key + " AND (day=" + dateTimePicker1.Value.Day + " OR day <" + dateTimePicker1.Value.Day + ") AND (Month=" + dateTimePicker1.Value.Month + " OR Month <" + dateTimePicker1.Value.Month + ") AND (God = " + dateTimePicker1.Value.Year + " OR God <" + dateTimePicker1.Value.Year + ")" + " AND Journal_id=-1 ";
                if (ClassSupport.selectValue(selectCommand) !=null)
                {
                    try
                    {
                        countProd.Add(key, Convert.ToInt32(ClassSupport.selectValue(selectCommand)));
                    }
                    catch (Exception er) {
                        MessageBox.Show("Нехватает товара", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
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
                Dictionary<int,int> ostatok = new Dictionary<int, int>();
                selectCommand = "select MAX(Journal_id) from Journal";
                object maxValue = ClassSupport.selectValue(selectCommand);
                if (Convert.ToString(maxValue) == "")
                    maxValue = 0;
                int Jornal_id = (Convert.ToInt32(maxValue) + 1);
                foreach (int key in countApp.Keys)
                {

                    selectCommand = "select id,Count from Journal_Product where Product_id=" + key + " AND (day=" + dateTimePicker1.Value.Day + " OR day <" + dateTimePicker1.Value.Day + ") AND (Month=" + dateTimePicker1.Value.Month + " OR Month <" + dateTimePicker1.Value.Month + ") AND (God = " + dateTimePicker1.Value.Year + " OR God <" + dateTimePicker1.Value.Year + ")" + " AND Journal_id=-1 ";
                    ostatok = ClassSupport.selectValueCheck(selectCommand);
                    int glav_ostatok = Convert.ToInt32(countApp[key]);
                    foreach (int keylog in ostatok.Keys) {
                        if (glav_ostatok != 0)
                        {
                            if (ostatok[keylog] > glav_ostatok)
                            {
                                selectCommand = "update Journal_Product set Count='" + (Convert.ToInt32(ostatok[keylog]) - glav_ostatok) + "' where Product_id = " + key + " AND(day = " + dateTimePicker1.Value.Day + " OR day < " + dateTimePicker1.Value.Day + ") AND(Month = " + dateTimePicker1.Value.Month + " OR Month < " + dateTimePicker1.Value.Month + ") AND(God = " + dateTimePicker1.Value.Year + " OR God < " + dateTimePicker1.Value.Year + ")" + " AND Journal_id = -1 AND id=" + keylog;
                                ClassSupport.changeValue(selectCommand);
                                glav_ostatok = 0;
                            }
                            else
                            {
                                selectCommand = "delete from Journal_Produc where id=" + key;
                                ClassSupport.changeValue(selectCommand);
                                glav_ostatok -= ostatok[keylog];
                            }
                        }
                    }
                    selectCommand = "select PriceZakyp from Product where Product_id=" + key;
                    int summ = Convert.ToInt32(ClassSupport.selectValue(selectCommand)) * countApp[key];

                    selectCommand = "select MAX(id) from Journal_Product";
                    maxValue = ClassSupport.selectValue(selectCommand);
                    if (Convert.ToString(maxValue) == "")
                        maxValue = 0;
                    string txtSQLQueryy = "insert into Journal_Product (Product_id,Journal_id,Sum,Count,Data,day,Month,God,id) values ('" + key + "', '" + Jornal_id + "', '" + summ + "', '" + Convert.ToInt32(countApp[key]) + "', '" + dateTimePicker1.Text + "', '" + dateTimePicker1.Value.Day + "', '" + dateTimePicker1.Value.Month + "', '" + dateTimePicker1.Value.Year + "','" + (Convert.ToInt32(maxValue) + 1) + "')";
                    ClassSupport.ExecuteQuery(txtSQLQueryy);
                }
                selectCommand = "select Sum(Sum) from Application_Product where Application_id=" + Baseid;
                int price = Convert.ToInt32(ClassSupport.selectValue(selectCommand));
                string txtSQLQuery = "insert into Journal (Journal_id,Data,Price,Application_id) values (" +
                               Jornal_id + ", '" + dateTimePicker1.Text + "', '" + price + "', '" + Baseid + "')";
                ClassSupport.ExecuteQuery(txtSQLQuery);
                

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
            selectCommand = "Select Product_id,Sum,Count,Data,day,Month,God from Journal_Product where (day=" + dateTimePicker1.Value.Day + " OR day <" + dateTimePicker1.Value.Day + ") AND (Month=" + dateTimePicker1.Value.Month + " OR Month <" + dateTimePicker1.Value.Month + ") AND (God = " + dateTimePicker1.Value.Year + " OR God <" + dateTimePicker1.Value.Year + ")";
            product_Counts = ClassSupport.selectValueProduct(selectCommand);
            for (int i = 0; i < product_Counts.Count; i++)
            {
                int stak = product_Counts[i].Material_id;
                for (int j = i + 1; j < product_Counts.Count; j++)
                {
                    if (stak == product_Counts[j].Material_id)
                    {
                        product_Counts[j].Material_id = -1;
                        product_Counts[i].Count += product_Counts[j].Count;
                        product_Counts[i].Sum += product_Counts[j].Sum;
                    }
                }
            }
            product_Counts.RemoveAll(rec => rec.Material_id == -1);
            selectTableProduct(product_Counts);

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            
            string selectCommand = "Select Product_id,Sum,Count,Data,day,Month,God from Journal_Product where (day=" + dateTimePicker1.Value.Day+" OR day <"+ dateTimePicker1.Value.Day+") AND (Month=" + dateTimePicker1.Value.Month + " OR Month <" + dateTimePicker1.Value.Month + ") AND (God = " + dateTimePicker1.Value.Year + " OR God <" + dateTimePicker1.Value.Year + ") AND (Journal_id = -1)";
            product_Counts=ClassSupport.selectValueProduct(selectCommand);
            for (int i = 0; i < product_Counts.Count; i++) {
                int stak = product_Counts[i].Material_id;
                for (int j = i+1; j < product_Counts.Count; j++) {
                    if (stak == product_Counts[j].Material_id) {
                        product_Counts[j].Material_id = -1;
                        product_Counts[i].Count += product_Counts[j].Count;
                        product_Counts[i].Sum += product_Counts[j].Sum;
                    }
                }
            }
            product_Counts.RemoveAll(rec => rec.Material_id == -1);
            selectTableProduct(product_Counts);

        }
    }
}
