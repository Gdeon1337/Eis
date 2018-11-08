using Microsoft.Reporting.WinForms;
using Microsoft.Reporting.WinForms;
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
    public partial class FormOtchets : Form
    {
        public FormOtchets()
        {
            InitializeComponent();
            ListOtchet dataSource = ClassSupport.list_load();
            ReportDataSource source = new ReportDataSource("DataSet1", dataSource);
            reportViewer1.LocalReport.DataSources.Add(source);
        }
    }
}
