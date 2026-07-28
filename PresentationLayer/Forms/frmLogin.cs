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
    public partial class frmLogin : Form
    {

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.RememberMe)
            {
                cbRememberMe.Checked = true;
                tbUsername.Text = Properties.Settings.Default.SavedUsername;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (tbPassword.Text == "" || tbUsername.Text == "")
            {
                MessageBox.Show("Please enter username and password!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbRememberMe.Checked)
            {
                Properties.Settings.Default.RememberMe = true;
                Properties.Settings.Default.SavedUsername = tbUsername.Text;
                Properties.Settings.Default.SavedPassword = clsHelper.HashPassword(tbPassword.Text);
            }
            else
            {
                Properties.Settings.Default.RememberMe = false;
                Properties.Settings.Default.SavedUsername = "";
                Properties.Settings.Default.SavedPassword = "";
            }
            Properties.Settings.Default.Save();

            try
            {
                clsUser user = clsUser.Login(tbUsername.Text, tbPassword.Text);

                if (user != null)
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbPassword.Clear();
                    tbUsername.Clear();
                }
            }
            catch (clsUser.InactiveAccountException)
            {
                MessageBox.Show("Your account is inactive. Please contact the administrator.", "Account Inactive", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbPassword.Clear();
            }
        }
    }
}
