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
using static BussinessLayer.clsTestAppointments;
namespace PresentationLayer
{
    public partial class frmVisionScheduleTest : Form
    {

        private enMode Mode = enMode.AddNew;
        private int _App;
        private int _LDLApp;

        public frmVisionScheduleTest()
        {
            InitializeComponent();
        }

        public frmVisionScheduleTest(int App, int LDLApp)
        {
            InitializeComponent();
            _App = App;
            _LDLApp = LDLApp;
        }

        private void frmVisionScheduleTest_Load(object sender, EventArgs e)
        {
            ucScheduleTest1.App = clsApplications.FindByApplicationID(_App);
            ucScheduleTest1.LDLApp = clsLocalDrivingLicenseApplications.GetLocalDrivingLicenseApplicationByID(_LDLApp);

        }
    }
}
