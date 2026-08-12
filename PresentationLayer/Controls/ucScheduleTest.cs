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
    public partial class ucScheduleTest : UserControl
    {

        private clsApplications _App;
        private clsLocalDrivingLicenseApplications _LDLApp;
        private short _TestFees = 0;

        public clsApplications App
        {
            get { return _App; }
            set
            {
                _App = value;
            }
        }

        public clsLocalDrivingLicenseApplications LDLApp
        {
            get { return _LDLApp; }
            set
            {
                _LDLApp = value;
                FillScheduleTestData();
            }
        }

        public clsTestAppointments TestAppointment
        {
            get { return _TestAppointment; }
            set
            {
                _TestAppointment = value;
                if (_TestAppointment != null)
                {
                    dtpDate.Value = _TestAppointment.AppointmentDate;
                    btnSave.Text = "Update";
                }
            }
        }
        private clsTestAppointments _TestAppointment = null;

        public void FillScheduleTestData()
        {
            if (_App == null) return;

            lblDLAppID.Text = _LDLApp.LocalDrivingLicenseApplicationID.ToString();
            lblDClass.Text = clsLicenseClasses.GetLicenseClassesByID(_LDLApp.LicenseClassID).ClassName;
            lblName.Text = clsPeople.FindPersonByID(clsApplications.FindByApplicationID(_LDLApp.ApplicationID).ApplicantPersonID).GetFullName();
            lblTrial.Text = clsLocalDrivingLicenseApplications.GetPassedTestsCount(_LDLApp.LocalDrivingLicenseApplicationID).ToString();
            clsTestTypes testType = clsTestTypes.GetTestTypeByID(1);
            _TestFees = testType.TestTypeFees;
            lblFees.Text = _TestFees.ToString();
        }
        public ucScheduleTest()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if(_App == null || _LDLApp == null)
            {
                MessageBox.Show("Error","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            if (_TestAppointment != null)
            {
                _TestAppointment.AppointmentDate = dtpDate.Value;
                try
                {
                    if (_TestAppointment.Save())
                        MessageBox.Show("Appointment updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Update failed.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in DataBase", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                clsTestAppointments testApp = new clsTestAppointments();

                testApp.TestTypeID = 1;
                testApp.LocalDrivingLicenseApplicationID = _LDLApp.LocalDrivingLicenseApplicationID;
                testApp.AppointmentDate = dtpDate.Value;
                testApp.PaidFees = _TestFees;
                testApp.CreatedByUserID = _App.CreatedByUserID;
                testApp.IsLocked = true;

                try
                {
                    if (testApp.Save())
                    {
                        MessageBox.Show("Data Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Data Failed To Save", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in DataBase", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
