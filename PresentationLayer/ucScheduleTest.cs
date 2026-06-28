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

        public void FillScheduleTestData()
        {
            if (_App == null) return;

            lblDLAppID.Text = _LDLApp.LocalDrivingLicenseApplicationID.ToString();
            lblDClass.Text = clsLicenseClasses.GetLicenseClassesByID(_LDLApp.LicenseClassID).ClassName;
            lblName.Text = clsPeople.FindPersonByID(clsApplications.FindByApplicationID(_LDLApp.ApplicationID).ApplicantPersonID).GetFullName();
            lblTrial.Text = clsLocalDrivingLicenseApplications.GetPassedTestsCount(_LDLApp.LocalDrivingLicenseApplicationID).ToString();
            lblFees.Text = clsTestTypes.GetTestTypeByID(1).TestTypeFees.ToString();
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

            clsTestAppointments testApp = new clsTestAppointments();

            testApp.TestTypeID = 1;
            testApp.LocalDrivingLicenseApplicationID = _LDLApp.LocalDrivingLicenseApplicationID;
            testApp.AppointmentDate = dtpDate.Value;
            testApp.PaidFees = 10;
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
