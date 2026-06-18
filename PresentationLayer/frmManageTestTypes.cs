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
    public partial class frmManageTestTypes : Form
    {

        public frmManageTestTypes()
        {
            InitializeComponent();
        }

        public void LoadTestTypes()
        {
            DataTable TestTypes = clsTestTypes.GetTestTypes();
            dgvTestTypes.DataSource = TestTypes;
            _FormatGrid();
            lblRecord.Text = TestTypes.Rows.Count.ToString();
        }

        private void _FormatGrid()
        {

            dgvTestTypes.Columns["TestTypeID"].HeaderText = "ID";
            dgvTestTypes.Columns["TestTypeTitle"].HeaderText = "Title";
            dgvTestTypes.Columns["TestTypeDescription"].HeaderText = "Description";
            dgvTestTypes.Columns["TestTypeFees"].HeaderText = "Fees";

            dgvTestTypes.Columns["TestTypeID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvTestTypes.Columns["TestTypeTitle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvTestTypes.Columns["TestTypeDescription"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvTestTypes.Columns["TestTypeFees"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageTestTypes_Load(object sender, EventArgs e)
        {
            LoadTestTypes();
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdateTestTypes frmUpdateTestTypes = new frmUpdateTestTypes((int)dgvTestTypes.CurrentRow.Cells[0].Value);
            frmUpdateTestTypes.ShowDialog();
            LoadTestTypes();
        }
    }
}
