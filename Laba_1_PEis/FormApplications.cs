using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba_1_PEis
{
    public partial class FormApplications : Form
    {
        
        public FormApplications()
        {
            InitializeComponent();
        }
        private class product {
            public int Product_id { set; get; }
            public int count { set; get; }
         //   public int Sum { set; get; }
        }
        List<product> Products=new List<product>();
        int CurrentRowBase;
        private void FormApplications_Load(object sender, System.EventArgs e)
        {
     
           
            String selectCommand = "Select * from Application";
            selectTable(selectCommand);
            selectCommand = "Select * from Customer";
            selectComboboxCustomer( selectCommand);
            selectCommand = "Select * from Product";
            selectComboboxProduct( selectCommand);
        }

        public void selectTable( String selectCommand)
        {

            dataGridView1.DataSource = ClassSupport.Connections(selectCommand);
            dataGridView1.DataMember = ClassSupport.Connections( selectCommand).Tables[0].ToString();

        }

        public void selectComboboxCustomer(String selectCommand)
        {
            comboBoxCustomer.ValueMember = "Customer_id";
            comboBoxCustomer.DisplayMember = "Name";
            comboBoxCustomer.DataSource = ClassSupport.Connections(selectCommand).Tables[0];
            comboBoxCustomer.SelectedItem = null;
            
        }

        public void selectComboboxProduct(String selectCommand)
        {
            comboBoxProduct.ValueMember = "Product_id";
            comboBoxProduct.DisplayMember = "Name";
            comboBoxProduct.DataSource = ClassSupport.Connections( selectCommand).Tables[0];
            comboBoxProduct.SelectedItem = null;

        }

        private void UpdateDatagridProduct(object ds) {

            dataGridView2.DataSource = null;
            dataGridView2.DataSource = ds;
        }

        private void UpdateDatagridProductBD(object ds)
        {

            dataGridView3.DataSource = null;
            dataGridView3.DataSource = ds;
        }

        private void addProduct_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxProduct.SelectedValue == null)
            {
                MessageBox.Show("Выберите товар", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //     String selectCommand = "Select Price from Product where Product_id=" + Convert.ToInt32(comboBoxProduct.SelectedValue);
            if (Products.Find(rec => rec.Product_id == Convert.ToInt32(comboBoxProduct.SelectedValue)) == null)
            {
                Products.Add(new product
                {
                    Product_id = Convert.ToInt32(comboBoxProduct.SelectedValue),
                    count = Convert.ToInt32(textBoxCount.Text)
                    //  Sum = Convert.ToInt32(ClassSupport.selectValue(selectCommand)) * Convert.ToInt32(textBoxCount.Text)
                });
            }
            UpdateDatagridProduct(Products);
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (comboBoxCustomer.SelectedValue == null)
            {
                MessageBox.Show("Выберите покупателя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Products.Count==0) {
                MessageBox.Show("Выберите товар", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            String selectCommand = "select MAX(Application_id) from Application";
            object maxValue = ClassSupport.selectValue(selectCommand);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;
            string txtSQLQuery = "insert into Application (Application_id, Customer_id) values (" +
           (Convert.ToInt32(maxValue) + 1) + ", '" + Convert.ToInt32(comboBoxCustomer.SelectedValue) + "')";
            ClassSupport.ExecuteQuery(txtSQLQuery);


           

            for (int i = 0; i < Products.Count; i++) {
                selectCommand = "SELECT Product_id FROM Product where Product_id=" + Products[i].Product_id ;
                object id = ClassSupport.selectValue( selectCommand);
                selectCommand = "SELECT Price FROM Product where Product_id=" + Products[i].Product_id;
                object price = ClassSupport.selectValue( selectCommand);
                txtSQLQuery = "insert into Application_Product (Application_id,Product_id,Count,Sum) values (" +
            (Convert.ToInt32(maxValue) + 1)+ ", '" + id + "', '" + Products[i].count + "', '" + Products[i].count*Convert.ToDouble(price) + "')";
                ClassSupport.ExecuteQuery(txtSQLQuery);
            }
            Products.Clear();
            UpdateDatagridProduct(Products);
            //обновление dataGridView1
            selectCommand = "select * from Application";
            refreshForm(selectCommand);
        }
        public void refreshForm( String selectCommand)
        {
            selectTable(selectCommand);
            dataGridView1.Update();
            dataGridView1.Refresh();

        }

        private void del_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;

                string valueId = dataGridView1[0, CurrentRow].Value.ToString();
                String selectCommand = "delete from Application where Application_id=" + valueId;

                ClassSupport.changeValue(selectCommand);
                //обновление dataGridView1
                selectCommand = "select * from Application";
                refreshForm(selectCommand);
            }
        }

        private void redaction_Click(object sender, EventArgs e)
        {
            if (comboBoxCustomer.SelectedValue == null)
            {
                MessageBox.Show("Выберите покупателя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }




            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение Name выбранной строки
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
    

            //обновление Name
            String selectCommand = "update Application set Customer_id='" + Convert.ToInt32(comboBoxCustomer.SelectedValue) + "' where Application_id = " + valueId;
          
            ClassSupport.changeValue( selectCommand);
            selectCommand = "SELECT Product_id  FROM Application_Product where Application_id=" + valueId;
            if (Products.Find(rec => rec.Product_id == Convert.ToInt32(ClassSupport.selectValue(selectCommand))) != null) {
                MessageBox.Show("Такой товар уже есть в бд изменяйте в другой форме", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < Products.Count; i++)
            {
                selectCommand = "SELECT Product_id FROM Product where Product_id=" + Products[i].Product_id;
                object id = ClassSupport.selectValue(selectCommand);
                selectCommand = "SELECT Price FROM Product where Product_id=" + Products[i].Product_id;
                object price = ClassSupport.selectValue(selectCommand);
                String txtSQLQuery = "insert into Application_Product (Application_id,Product_id,Count,Sum) values (" +
            Convert.ToInt32(valueId) + ", '" + id + "', '" + Products[i].count + "', '" + Products[i].count * Convert.ToDouble(price) + "')";
                ClassSupport.ExecuteQuery(txtSQLQuery);
            }
            Products.Clear();
            UpdateDatagridProduct(Products);
            //обновление dataGridView
            selectCommand = "select * from Application";
            refreshForm( selectCommand);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            
            //получить значение Name выбранной строки
            string customerId = dataGridView1[1, CurrentRow].Value.ToString();
            CurrentRowBase = Convert.ToInt32(valueId);

            comboBoxCustomer.SelectedItem = comboBoxCustomer.ValueMember.FirstOrDefault(rec=>rec==Convert.ToInt32(customerId));

            string selectCommand = "Select Product_id,Count from Application_Product where Application_id="+ Convert.ToInt32(valueId);
            UpdateDatagridProductBD(ClassSupport.Connections(selectCommand).Tables[0]);

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int CurrentRow = dataGridView2.SelectedCells[0].RowIndex;
            string valueId = dataGridView2[0, CurrentRow].Value.ToString();
            //получить значение Name выбранной строки
            textBoxCount.Text = Products[CurrentRow].count.ToString();

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int CurrentRow = dataGridView3.SelectedCells[0].RowIndex;
            string valueId = dataGridView3[0, CurrentRow].Value.ToString();

            string selectCommand = "Select Count from Application_Product where Product_id=" + Convert.ToInt32(valueId);

            textBox1.Text = ClassSupport.selectValue(selectCommand).ToString();

        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 1)
            {

                int CurrentRow = dataGridView2.SelectedCells[0].RowIndex;


                string valueId = dataGridView2[0, CurrentRow].Value.ToString();

                Products.RemoveAt(CurrentRow);
                UpdateDatagridProduct(Products);
            }
            

        }


        private void textBoxCount_KeyPress(object sender, KeyPressEventArgs e)
        {



            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count == 1)
            {
                int CurrentRow = dataGridView3.SelectedCells[0].RowIndex;


                string valueId = dataGridView3[0, CurrentRow].Value.ToString();
                if (checkBox1.Checked)
                {

                    String selectCommand = "Select * from Application_Product where Product_id=" + valueId;

                    if (ClassSupport.selectValue(selectCommand) == null)
                    {
                        MessageBox.Show("этого товара ещё нет в бд", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    };

                    selectCommand = "delete from Application_Product where Product_id=" + valueId + " AND Application_id=" + Convert.ToInt32(CurrentRowBase);

                    ClassSupport.changeValue(selectCommand);

                    selectCommand = "Select Product_id,Count from Application_Product where Application_id=" + Convert.ToInt32(valueId);
                    UpdateDatagridProductBD(ClassSupport.Connections(selectCommand).Tables[0]);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 1)
            {
                int CurrentRow = dataGridView2.SelectedCells[0].RowIndex;


                string valueId = dataGridView2[0, CurrentRow].Value.ToString();

                Products[CurrentRow].count = Convert.ToInt32(textBoxCount.Text);
                UpdateDatagridProduct(Products);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int CurrentRow = dataGridView3.SelectedCells[0].RowIndex;
            string valueId = dataGridView3[0, CurrentRow].Value.ToString();
            string selectCommand = "Update  Application_Product set Count="+ Convert.ToInt32(textBox1.Text) +" where Product_id=" + valueId;

            ClassSupport.changeValue(selectCommand);

            selectCommand = "Select Product_id,Count from Application_Product where Application_id=" + CurrentRowBase;
            UpdateDatagridProductBD(ClassSupport.Connections(selectCommand).Tables[0]);
        }
    }
}
