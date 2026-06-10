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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(tbPassword.Text == "" || tbUsername.Text == "")
            {
                MessageBox.Show("Please enter username and password!","Validation",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            clsUser user = clsUser.Login(tbUsername.Text,tbPassword.Text);

            if(user != null)
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
    }
}
