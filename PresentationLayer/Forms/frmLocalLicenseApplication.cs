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
    public partial class frmLocalLicenseApplication : Form
    {
        public frmLocalLicenseApplication()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLocalLicenseApplication_Load(object sender, EventArgs e)
        {
            cbLicenseClass.DataSource = clsLicenseClasses.GetLicenseClasses();
            cbLicenseClass.DisplayMember = "ClassName";
            cbLicenseClass.ValueMember = "LicenseClassID";

            lblDate.Text = DateTime.Now.ToShortDateString();
            clsApplicationTypes appType = clsApplicationTypes.GetApplicationTypeByID(1);
            if(appType != null)
                lblFees.Text = appType.ApplicationFees.ToString();
            lblUsername.Text = clsUser.CurrentUser.UserName;

        }

        private void btnNext_Click(object sender, EventArgs e)
        {

            if (ucPersonDetailsWithFilter1.Person == null)
            {
                MessageBox.Show("Please Find a person first!","Required",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            tcPerson.SelectedTab = tpApplicationInfo;

        }

        private void tcPerson_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if(e.TabPage == tpApplicationInfo && (ucPersonDetailsWithFilter1.Person == null))
            {
                e.Cancel = true;
                MessageBox.Show("Please find a person first!", "Required",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ucPersonDetailsWithFilter1.Person == null)
            {
                MessageBox.Show("Please Find a person first!", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(clsLocalDrivingLicenseApplications.DoesPersonHaveActiveApplicationForClass(ucPersonDetailsWithFilter1.Person.PersonID,(int)cbLicenseClass.SelectedValue))
            {
                MessageBox.Show("This person already has an active application for this license class!",
                "Duplicate Application", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            clsApplications app = new clsApplications();
            clsLocalDrivingLicenseApplications LocalDrivingLicenseApp = new clsLocalDrivingLicenseApplications();

            app.ApplicantPersonID = ucPersonDetailsWithFilter1.Person.PersonID;
            app.ApplicationDate = DateTime.Now;
            app.ApplicationTypeID = 1;
            app.ApplicationStatus = 1;
            app.LastStatusDate = DateTime.Now;
            app.PaidFees = clsApplicationTypes.GetApplicationTypeByID(1).ApplicationFees;
            app.CreatedByUserID = clsUser.CurrentUser.UserID;

            try
            {
                if (app.Save())
                {

                    LocalDrivingLicenseApp.ApplicationID = app.ApplicationID;
                    LocalDrivingLicenseApp.LicenseClassID = (int)cbLicenseClass.SelectedValue;

                    if(LocalDrivingLicenseApp.Save())
                    {
                        MessageBox.Show("Application Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lblApplicationID.Text = LocalDrivingLicenseApp.LocalDrivingLicenseApplicationID.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Failed to save license class details. Please contact support.", "Failed",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Application Failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}