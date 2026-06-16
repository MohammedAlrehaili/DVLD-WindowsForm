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
    public partial class frmMain : Form
    {
        // ----- Private Fields -----
        private clsUser _User;

        // ----- Constructors -----
        public frmMain()
        {
            InitializeComponent();
        }

        public frmMain(clsUser User)
        {
            InitializeComponent();
            _User = User;
        }

        // ----- Form Events -----
        private void frmMain_Load(object sender, EventArgs e)
        {
        }

        // ----- Click Methods -----

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to sign out?", "Sign Out",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            if (result == DialogResult.Yes)
            {
                clsUser.CurrentUser = null;

                this.Hide();

                frmLogin loginform = new frmLogin();
                loginform.ShowDialog();

                if (clsUser.CurrentUser == null)
                {
                    Application.Exit();
                }
                else
                {
                    this.Show();
                }
            }
        }

        private void PeopleToolStrixpMenuItem_Click(object sender, EventArgs e)
        {
            frmPeople frmPeople = new frmPeople();
            frmPeople.ShowDialog();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageUsers frmManageUsers = new frmManageUsers();
            frmManageUsers.ShowDialog();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo frmUserInfo = new frmUserInfo(clsUser.CurrentUser.UserID);
            frmUserInfo.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frmChangePassword = new frmChangePassword(clsUser.CurrentUser.UserID);
            frmChangePassword.ShowDialog();
        }

        private void manageApplicationsTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageApplicationTypes frmManageApplications = new frmManageApplicationTypes();
            frmManageApplications.ShowDialog();
        }
    }
}
