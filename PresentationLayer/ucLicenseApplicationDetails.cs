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
    public partial class ucLicenseApplicationDetails : UserControl
    {

        private clsApplications _app;
        private clsLocalDrivingLicenseApplications _LDLapp;
        public ucLicenseApplicationDetails()
        {
            InitializeComponent();
        }

        private void _FillApplicationBasicInfoData()
        {
            if (_app == null) return;
           
            lblAppID.Text = _app.ApplicationID.ToString();
            lblStatus.Text = app.ApplicationStatus == 1 ? "New" : app.ApplicationStatus == 2 ? "Cancelled" : "Completed";
            lblFees.Text = app.PaidFees.ToString();
            lblType.Text = clsApplicationTypes.GetApplicationTypeByID(1).ApplicationTypeTitle;
            lblPersonName.Text = clsPeople.FindPersonByID(app.ApplicantPersonID).GetFullName();
            lblDate.Text = app.ApplicationDate.ToShortDateString();
            lblStatusDate.Text = app.LastStatusDate.ToShortDateString();
            lblCreatedBy.Text = clsUser.FindByUserID(app.CreatedByUserID).UserName;
        }

        private void _FillDrivingLicenseApplicationInfo()
        {
            if( _LDLapp == null) return;

            lblDLAppID.Text = _LDLapp.LocalDrivingLicenseApplicationID.ToString();
            lblLicenseClass.Text = clsLicenseClasses.GetLicenseClassesByID(_LDLapp.LicenseClassID).ClassName;
            lblPassedTests.Text = clsLocalDrivingLicenseApplications.GetPassedTestsCount(_LDLapp.LocalDrivingLicenseApplicationID) + "/3";
        }

        public clsApplications app
        {
            get { return _app; }
            set 
            { 
                _app = value;
                _FillApplicationBasicInfoData();
            }
        }

        public clsLocalDrivingLicenseApplications LDLapp
        {
            get { return _LDLapp; }
            set
            {
                _LDLapp = value;
                _FillDrivingLicenseApplicationInfo();
            }
        }

        private void llblViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails(app.ApplicantPersonID);
            frm.ShowDialog();
        }
    }
}
