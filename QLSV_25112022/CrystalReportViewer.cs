using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV_25112022
{
    public partial class CrystalReportViewer : Form
    {
        public CrystalReportViewer()
        {
            InitializeComponent();
        }

        private void CrystalReportViewer_Load(object sender, EventArgs e)
        {
            ReportDocument report = new ReportDocument();

            string path = string.Format("{0}\\Report\\CrystalReport.rpt",Application.StartupPath);

            report.Load(path);

            crystalReportViewer1.ReportSource = report;

        }
    }
}
