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

namespace PresentationLayer.Controls
{
    public partial class ucTakeTest : UserControl
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
        public ucTakeTest()
        {
            InitializeComponent();
        }
    }
}
