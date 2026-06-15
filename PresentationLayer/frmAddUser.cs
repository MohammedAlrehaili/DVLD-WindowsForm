using BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BussinessLayer.clsUser;

namespace PresentationLayer
{
    public partial class frmAddUser : Form
    {

        // ----- Private Fields -----

        private enMode _Mode = enMode.AddNew;
        private int _UserID = 0;
        private bool _PersonFound = false;
        private int _PersonID = -1;

        // ---------------------------

        // ----- Constructors -----
        public frmAddUser()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUser(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
            _Mode = enMode.Update;
        }

        // ---------------------------

        // ----- Form Events -----
        private void frmAddUser_Load(object sender, EventArgs e)
        {

            if (_Mode == enMode.AddNew)
            {
                this.Text = "Add New User";
                label1.Text = "Add New User";
                
            }
            else if (_Mode == enMode.Update)
            {
                this.Text = "Update User";
                label1.Text = "Update User";
                gbFilter.Visible = false;

                clsUser user = clsUser.FindByUserID(_UserID);
                if (user != null)
                {
                    ucPersonDetails1.Person = clsPeople.FindPersonByID(user.PersonID);
                    tbUserName.Text = user.UserName;
                    tbPassword.Text = user.Password;
                    tbConfirmPassword.Text = user.Password;
                    cbIsActive.Checked = user.isActive;
                    _PersonFound = true; // so tab switching isn't blocked
                }

            }

            cbFilter.Items.Add("NationalNo");

            cbFilter.SelectedIndex = 0;
        }

        // ---------------------------

        // ----- Click Methods -----
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (tbFilter.Text == "")
            {
                MessageBox.Show("Please fill the filter field", "Empty Field",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            switch (cbFilter.SelectedItem.ToString())
            {
                case "NationalNo":
                    clsPeople person = clsPeople.FindPersonByNationalNo(tbFilter.Text);

                    if (person != null)
                    {
                        ucPersonDetails1.Person = person;
                        _PersonFound = true;
                        _PersonID = person.PersonID;
                    }
                    else
                    {
                        MessageBox.Show("Person not found!", "Not Found",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {

            if(_Mode == enMode.AddNew)
            {
                if (!_PersonFound)
                {
                    MessageBox.Show("Please find a person first!", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (clsUser.IsPersonIDExisit(ucPersonDetails1.Person.PersonID))
                {
                    MessageBox.Show("Selected Person already has a user, choose another one.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            tcUserSetup.SelectedTab = tpLoginInfo;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbUserName.Text == "" || tbPassword.Text == "" || tbConfirmPassword.Text == "")
            {
                MessageBox.Show("Please Fill the fields", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tbPassword.Text != tbConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_Mode == enMode.AddNew)
            {
                if (clsUser.IsPersonIDExisit(ucPersonDetails1.Person.PersonID))
                {
                    MessageBox.Show("This person already has a user account!", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // ✅ load existing user for Update, or create new one for AddNew
            clsUser user = _Mode == enMode.Update
                         ? clsUser.FindByUserID(_UserID)
                         : new clsUser();

            user.PersonID = ucPersonDetails1.Person.PersonID;
            user.UserName = tbUserName.Text;
            user.Password = tbPassword.Text;
            user.isActive = cbIsActive.Checked;

            try
            {
                if (user.Save())
                {
                    MessageBox.Show(_Mode == enMode.AddNew ? "User added successfully!" : "User updated successfully!",
                                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _Mode = enMode.Update;
                    _UserID = user.UserID;
                    this.Text = "Update User";
                    label1.Text = "Update User";
                }
                else
                {
                    MessageBox.Show("Failed to save user. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddPerson frmAddPerson = new frmAddPerson();
            frmAddPerson.ShowDialog();

            if(frmAddPerson.newPersonID != -1)
            {
                clsPeople person = clsPeople.FindPersonByID(frmAddPerson.newPersonID);
                if(person != null)
                {
                    ucPersonDetails1.Person = person;
                    _PersonFound = true;
                }
            }
            
        }
        // --------------------------------

        // Block switching to the Login Info tab until a person has been found/selected,
        // since a user account must be linked to an existing person record.
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tpLoginInfo && !_PersonFound)
            {
                e.Cancel = true;
                MessageBox.Show("Please Find a person first!", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
