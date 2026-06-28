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
    public partial class frmVisionTestAppointments : Form
    {

        private int _ApplicationID;
        private int _LDLAppID;

        public frmVisionTestAppointments()
        {
            InitializeComponent();
        }

        public frmVisionTestAppointments(int ApplicationID, int DLApp)
        {
            InitializeComponent();
            _ApplicationID = ApplicationID;
            _LDLAppID = DLApp;
        }

        private void _FormatGrid()
        {

            dgvTestAppoitnments.Columns["TestTypeID"].Visible = false;
            dgvTestAppoitnments.Columns["LocalDrivingLicenseApplicationID"].Visible = false;
            dgvTestAppoitnments.Columns["CreatedByUserID"].Visible = false;

            dgvTestAppoitnments.Columns["TestAppointmentID"].HeaderText = "Appointment ID";
            dgvTestAppoitnments.Columns["AppointmentDate"].HeaderText = "Appointment Date";
            dgvTestAppoitnments.Columns["PaidFees"].HeaderText = "Paid Fees";
            dgvTestAppoitnments.Columns["IsLocked"].HeaderText = "Is Locked";

            dgvTestAppoitnments.Columns["TestAppointmentID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvTestAppoitnments.Columns["AppointmentDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvTestAppoitnments.Columns["PaidFees"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvTestAppoitnments.Columns["IsLocked"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        public void LoadAppointments()
        {
            DataTable dt = clsTestAppointments.GetTestAppointmentsByLDLAppID(_LDLAppID);
            dgvTestAppoitnments.DataSource = dt;
            _FormatGrid();
            lblRecords.Text = dt.Rows.Count.ToString();
        }

        private void frmVisionTestAppointments_Load(object sender, EventArgs e)
        {
            ucLicenseApplicationDetails1.app = clsApplications.FindByApplicationID(_ApplicationID);
            ucLicenseApplicationDetails1.LDLapp = clsLocalDrivingLicenseApplications.GetLocalDrivingLicenseApplicationByID(_LDLAppID);

            LoadAppointments();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmVisionScheduleTest frm = new frmVisionScheduleTest(_ApplicationID,_LDLAppID);
            frm.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVisionScheduleTest frm = new frmVisionScheduleTest(_ApplicationID, _LDLAppID);
            frm.ShowDialog();
        }
    }
}
