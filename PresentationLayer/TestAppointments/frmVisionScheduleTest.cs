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
    public partial class frmVisionScheduleTest : Form
    {

        private int _ApplicationID;
        private int _LDLAppID;
        private int _TestAppointmentID = -1;

        public frmVisionScheduleTest(int ApplicationID, int LDLAppID, int TestAppointmentID)
        {
            InitializeComponent();
            _ApplicationID = ApplicationID;
            _LDLAppID = LDLAppID;
            _TestAppointmentID = TestAppointmentID;
        }

        public frmVisionScheduleTest()
        {
            InitializeComponent();
        }

        public frmVisionScheduleTest(int App, int LDLApp)
        {
            InitializeComponent();
            _ApplicationID = App;
            _LDLAppID = LDLApp;
        }

        private void frmVisionScheduleTest_Load(object sender, EventArgs e)
        {
            ucScheduleTest1.App = clsApplications.FindByApplicationID(_ApplicationID);
            ucScheduleTest1.LDLApp = clsLocalDrivingLicenseApplications.GetLocalDrivingLicenseApplicationByID(_LDLAppID);

            if (_TestAppointmentID != -1)
            {
                ucScheduleTest1.TestAppointment = clsTestAppointments.FindByID(_TestAppointmentID);
            }
        }
    }
}
