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
    public partial class frmLocalDrivingLicenseApplications : Form
    {
        public frmLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        private void LoadLocalLicenseApplications()
        {
            DataTable table = clsLocalDrivingLicenseApplications.GetAllLocalDrivingLicenseApplications();
            dgvLicenseApplications.DataSource = table;
            _FormatGrid();
            lblRecords.Text = table.Rows.Count.ToString();
        }

        private void _FormatGrid()
        {
            if (dgvLicenseApplications.Columns["L.D.L.AppID"] == null) return;

            dgvLicenseApplications.Columns["L.D.L.AppID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvLicenseApplications.Columns["ApplicationID"].Visible = false; // hidden, used internally for actions like Cancel
            dgvLicenseApplications.Columns["Driving Class"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvLicenseApplications.Columns["National No."].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvLicenseApplications.Columns["Full Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvLicenseApplications.Columns["Application Date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvLicenseApplications.Columns["Passed Tests"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvLicenseApplications.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void frmLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            LoadLocalLicenseApplications();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvLicenseApplications.CurrentRow == null) return;

            int applicationID = (int)dgvLicenseApplications.CurrentRow.Cells["ApplicationID"].Value;

            DialogResult result = MessageBox.Show("Are you sure you want to cancel this application?",
                                                  "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                if (clsApplications.CancelApplication(applicationID))
                {
                    MessageBox.Show("Application cancelled successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadLocalLicenseApplications(); // refresh the grid
                }
                else
                {
                    MessageBox.Show("Cannot cancel this application. It may already be completed.",
                                    "Cancel Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
