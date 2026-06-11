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
    public partial class frmManageUsers : Form
    {
        public frmManageUsers()
        {
            InitializeComponent();
        }

        private void LoadUsers()
        {
            DataTable UsersData = clsUser.GetUsers();
            dgvUsersData.DataSource = UsersData;
            lblRecords.Text = UsersData.Rows.Count.ToString();
        }

        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddUser frmAddUser = new frmAddUser();
            frmAddUser.ShowDialog();
        }
    }
}
