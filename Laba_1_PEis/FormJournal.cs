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
            selectCommand = "Select Subcount_debet,Count,Data,Day,Month,Year from Transactions where Credit_count=60 AND (((day = " + dateTimePicker1.Value.Day + " OR day < " + dateTimePicker1.Value.Day + ") AND (Month = " + dateTimePicker1.Value.Month + ") And ((Year = " + dateTimePicker1.Value.Year + ") OR (Year < " + dateTimePicker1.Value.Year + "))) OR ((Month < " + dateTimePicker1.Value.Month + ") AND (Year = " + dateTimePicker1.Value.Year + ")) OR (Year < " + dateTimePicker1.Value.Year + "))";
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
                    }
                }
            }
            product_Counts.RemoveAll(rec => rec.Material_id == -1);

            selectCommand = "Select Product_id,Sum(Count) from Journal_Product GROUP BY Product_id";
            if (ClassSupport.selectValueCheck(selectCommand).ToString() != "")
            {
                Dictionary<int, int> valuePairs = ClassSupport.selectValueCheck(selectCommand);
                foreach (int key in valuePairs.Keys)
                {
                    for (int i = 0; i < product_Counts.Count; i++)
                    {
                        if (Convert.ToInt32(product_Counts[i].Material_id) == key)
                        {
                            product_Counts[i].Count -= valuePairs[key];
                        }
                    }
                }

                product_Counts.RemoveAll(rec => rec.Count <= 0);
            }

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
            Dictionary<int, int> valuePairs = ClassSupport.selectValueCheck(selectCommand);
            foreach (int key in valuePairs.Keys) {
                if (product_Counts.FirstOrDefault(rec => rec.Material_id == key) == null) {
                    MessageBox.Show("Не хватает материала под id" + key, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (product_Counts.Find(rec => rec.Material_id == key).Count < valuePairs[key]) {
                    MessageBox.Show("Не хватает материала под id"+key , "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
           
            selectCommand = "select MAX(Journal_id) from Journal";
            object maxValue = ClassSupport.selectValue(selectCommand);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;
            int Jornal_id = Convert.ToInt32(maxValue)+1;

            string txtSQLQuery;
            int sum_vsego = 0;
            foreach (int key in valuePairs.Keys)
            {
                selectCommand = "select PriceZakyp from Product where Product_id=" + key;
                int summ = Convert.ToInt32(ClassSupport.selectValue(selectCommand)) * valuePairs[key];
                selectCommand = "select MAX(Transactions_id) from Transactions";
                maxValue = ClassSupport.selectValue(selectCommand);

                if (Convert.ToString(maxValue) == "")
                    maxValue = 0;
                txtSQLQuery = "insert into Transactions (Transactions_id, Debit_count, Credit_count, Count,Price, Data, Subcount_debet, Subcount_credit, Journal_id,Day,Month,Year) values ('" +
                                        (Convert.ToInt32(maxValue) + 1) + "', '" + "91" + "', '" + "10" + "', '" + valuePairs[key] + "', '" + summ + "', '" + dateTimePicker1.Text + "', '" + Baseid + "', '" + key + "', '" + Jornal_id + "', '"+dateTimePicker1.Value.Day+ "', '" + dateTimePicker1.Value.Month+ "', '" + dateTimePicker1.Value.Year + "')";
                ClassSupport.ExecuteQuery(txtSQLQuery);

                txtSQLQuery = "insert into Journal_Product (Journal_id,Data,Sum,Product_id,Count) values (" +
                           Jornal_id + ", '" + dateTimePicker1.Text + "', '" + summ + "', '" + key + "', '"+ valuePairs[key]+ "')";
                ClassSupport.ExecuteQuery(txtSQLQuery);
                sum_vsego += summ;
            }

            txtSQLQuery = "insert into Journal (Journal_id,Data,Price,Application_id) values (" +
                           Jornal_id + ", '" + dateTimePicker1.Text + "', '" + sum_vsego + "', '" + Baseid + "')";
            ClassSupport.ExecuteQuery(txtSQLQuery);




            selectCommand = "select MAX(Transactions_id) from Transactions";
            maxValue = ClassSupport.selectValue(selectCommand);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;
            selectCommand = "select Customer_id from Application where Application_id=" + Baseid;

            string customerid = ClassSupport.selectValue(selectCommand).ToString();

            txtSQLQuery = "insert into Transactions (Transactions_id, Debit_count, Credit_count, Count,Price, Data, Subcount_debet, Subcount_credit,Journal_id,Day,Month,Year) values ('" +
                    (Convert.ToInt32(maxValue) + 1) + "', '" + "62" + "', '" + "91" + "', '" + "1" + "', '" + sum_vsego + "', '" + dateTimePicker1.Text + "', '" + customerid + "', '" + Baseid + "', '" + Jornal_id + "', '" + dateTimePicker1.Value.Day + "', '" + dateTimePicker1.Value.Month + "', '" + dateTimePicker1.Value.Year + "')";
            ClassSupport.ExecuteQuery(txtSQLQuery);


            MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            string selectCommand = "Select Subcount_debet,Count,Data,Day,Month,Year from Transactions where Credit_count=60 AND (((day = " + dateTimePicker1.Value.Day + " OR day < " + dateTimePicker1.Value.Day + ") AND (Month = " + dateTimePicker1.Value.Month + ") And ((Year = " + dateTimePicker1.Value.Year + ") OR (Year < " + dateTimePicker1.Value.Year + "))) OR ((Month < " + dateTimePicker1.Value.Month + ") AND (Year = " + dateTimePicker1.Value.Year + ")) OR (Year < " + dateTimePicker1.Value.Year + "))";
            if (ClassSupport.selectValueProduct(selectCommand).ToString() != "")
            {
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
                        }
                    }
                }
                product_Counts.RemoveAll(rec => rec.Material_id == -1);

                selectCommand = "Select Product_id,Sum(Count) from Journal_Product GROUP BY Product_id";
                if (ClassSupport.selectValueCheck(selectCommand).ToString() != "")
                {
                    Dictionary<int, int> valuePairs = ClassSupport.selectValueCheck(selectCommand);
                    foreach (int key in valuePairs.Keys)
                    {
                        for (int i = 0; i < product_Counts.Count; i++)
                        {
                            if (Convert.ToInt32(product_Counts[i].Material_id) == key)
                            {
                                product_Counts[i].Count -= valuePairs[key];
                            }
                        }
                    }

                    product_Counts.RemoveAll(rec => rec.Count <= 0);
                }
            }
            else {
                product_Counts.Clear();
            }
            selectTableProduct(product_Counts);

        }
    }
}
