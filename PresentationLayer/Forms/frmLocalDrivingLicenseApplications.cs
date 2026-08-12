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

            cbFilter.Items.Add("None");
            cbFilter.Items.Add("L.D.L.AppID");
            cbFilter.Items.Add("National No.");
            cbFilter.Items.Add("Full Name");
            cbFilter.Items.Add("Status");

            cbFilter.SelectedIndex = 0;
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmLocalLicenseApplication frm = new frmLocalLicenseApplication();
            frm.ShowDialog();
            LoadLocalLicenseApplications();
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {

            string FilterColumn = "";

            switch(cbFilter.SelectedItem)
            {
                case "L.D.L.AppID":
                    FilterColumn = "L.D.L.AppID";
                    break;
                case "National No.":
                    FilterColumn = "National No.";
                    break;
                case "Full Name":
                    FilterColumn = "Full Name";
                    break;
                case "Status":
                    FilterColumn = "Status";
                    break;
            }

            DataTable table = clsLocalDrivingLicenseApplications.GetLocalDrivingLicenseApplicationsByFilter(FilterColumn, tbFilter.Text);
            dgvLicenseApplications.DataSource = table;
            lblRecords.Text = table.Rows.Count.ToString();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

            if(cbFilter.SelectedItem != "None")
            {
                tbFilter.Visible = true;
            }    
            else
            {
                tbFilter.Visible = false;
                LoadLocalLicenseApplications();
            }
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (cbFilter.SelectedItem.ToString())
            {
                case "L.D.L.AppID":
                    e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back;
                    break;
            }
        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dgvLicenseApplications.CurrentRow == null) return;

            frmVisionTestAppointments frm = new frmVisionTestAppointments((int)dgvLicenseApplications.CurrentRow.Cells["ApplicationID"].Value, (int)dgvLicenseApplications.CurrentRow.Cells["L.D.L.AppID"].Value);
            frm.ShowDialog();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvLicenseApplications.CurrentRow == null) return;

            int ldlAppID = (int)dgvLicenseApplications.CurrentRow.Cells["L.D.L.AppID"].Value;

            DialogResult result = MessageBox.Show("Are you sure you want to delete this application? This cannot be undone.",
                                                  "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                if (clsLocalDrivingLicenseApplications.DeleteLocalDrivingLicenseApplication(ldlAppID))
                {
                    MessageBox.Show("Application deleted successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadLocalLicenseApplications();
                }
                else
                {
                    MessageBox.Show("Cannot delete this application. Only New applications can be deleted.",
                                    "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}