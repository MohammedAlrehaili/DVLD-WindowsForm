using BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class frmManageApplicationTypes : Form
    {
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            _LoadApplicationTypes();
        }

        private void _LoadApplicationTypes()
        {
            DataTable dt = clsApplicationTypes.GetApplicationTypes();
            dgvApplicationTypes.DataSource = dt;
            _FormatGrid();
            lblRecords.Text = dt.Rows.Count.ToString();
        }

        private void _FormatGrid()
        {

            dgvApplicationTypes.Columns["ApplicationTypeID"].HeaderText = "ID";
            dgvApplicationTypes.Columns["ApplicationTypeTitle"].HeaderText = "Title";
            dgvApplicationTypes.Columns["ApplicationFees"].HeaderText = "Fees";

            dgvApplicationTypes.Columns["ApplicationTypeID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvApplicationTypes.Columns["ApplicationTypeTitle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvApplicationTypes.Columns["ApplicationFees"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdateApplicationType frm = new frmUpdateApplicationType((int)dgvApplicationTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _LoadApplicationTypes();
        }
    }
}
