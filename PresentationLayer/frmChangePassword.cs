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
    public partial class frmChangePassword : Form
    {
        private int _UserID;
        public frmChangePassword()
        {
            InitializeComponent();
        }

        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            clsUser user = clsUser.FindByUserID(_UserID);

            if (user != null)
            {
                ucPersonDetails1.Person = clsPeople.FindPersonByID(user.PersonID);
                lblUserID.Text = user.UserID.ToString();
                lblUserName.Text = user.UserName;
                lblIsActive.Text = user.isActive ? "Yes" : "No";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (tbCurrentPassword.Text == "" || tbNewPassword.Text == "" || tbConfirmPassword.Text == "")
            {
                MessageBox.Show("Please Fill the fields","Required",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            clsUser user = clsUser.FindByUserID(_UserID);

            if (user != null)
            {
                if(tbCurrentPassword.Text != user.Password)
                {
                    MessageBox.Show("Current password is wrong","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                if (tbNewPassword.Text != tbConfirmPassword.Text)
                {
                    MessageBox.Show("New Password And Confirm Password Doesn't Match", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (tbNewPassword.Text == user.Password)
                {
                    MessageBox.Show("New password must be different from the current password!", "Warning",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (clsUser.UpdatePassword(user.UserID,tbNewPassword.Text))
                {
                    MessageBox.Show("Password Changed Sucessfully", "Suceess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbCurrentPassword.Clear();
                    tbConfirmPassword.Clear();
                    tbNewPassword.Clear();
                }
                else
                {
                    MessageBox.Show("Password Failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                MessageBox.Show("User not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
