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

    }




}
