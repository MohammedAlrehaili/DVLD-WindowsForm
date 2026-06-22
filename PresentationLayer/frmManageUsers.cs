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

        // ----- Private Functions -----

        private void LoadUsers()
        {
            DataTable UsersData = clsUser.GetUsers();
            dgvUsersData.DataSource = UsersData;
            _FormatGrid();
            lblRecords.Text = UsersData.Rows.Count.ToString();
        }

        private void _FormatGrid()
        {

            if (dgvUsersData.Columns["Password"] == null) return;

            dgvUsersData.Columns["Password"].Visible = false;

            dgvUsersData.Columns["UserID"].HeaderText = "User ID";
            dgvUsersData.Columns["PersonID"].HeaderText = "Person ID";
            dgvUsersData.Columns["FullName"].HeaderText = "Full Name";
            dgvUsersData.Columns["UserName"].HeaderText = "UserName";
            dgvUsersData.Columns["IsActive"].HeaderText = "Is Active";

            dgvUsersData.Columns["UserID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvUsersData.Columns["PersonID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvUsersData.Columns["FullName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvUsersData.Columns["UserName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvUsersData.Columns["IsActive"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }


        // ----- Constructors -----
        public frmManageUsers()
        {
            InitializeComponent();
        }
        
        // ----- Form Events -----
        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            LoadUsers();

            cbFilter.Items.Add("None");
            cbFilter.Items.Add("User ID");
            cbFilter.Items.Add("UserName");
            cbFilter.Items.Add("Person ID");
            cbFilter.Items.Add("Full Name");
            cbFilter.Items.Add("Is Active");
            cbFilter.SelectedIndex = 0;

            cbIsActive.Items.Add("All");
            cbIsActive.Items.Add("Yes");
            cbIsActive.Items.Add("No");
            cbIsActive.SelectedIndex = 0;
        }

        // ----- Click Methods -----
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddUser frmAddUser = new frmAddUser();
            frmAddUser.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvUsersData.CurrentRow == null) return;

            frmAddUser frmAddUser = new frmAddUser((int)dgvUsersData.CurrentRow.Cells[0].Value);
            frmAddUser.ShowDialog();
            LoadUsers();
        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUser frmAddPerson = new frmAddUser();
            frmAddPerson.ShowDialog();
            LoadUsers();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvUsersData.CurrentRow == null) return;

            int userID = (int)dgvUsersData.CurrentRow.Cells[0].Value;

            DialogResult result = MessageBox.Show("Are you sure you want to delete this user?",
                                                  "Confirm Delete",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                if (clsUser.DeleteUser(userID))
                {
                    MessageBox.Show("User deleted successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("Failed to delete user!", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (System.Data.SqlClient.SqlException ex) when (ex.Number == 547)
            {
                MessageBox.Show("This user cannot be deleted because they have created records in the system.",
                                "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (dgvUsersData.CurrentRow == null) return;

            frmChangePassword frmChangePassword = new frmChangePassword((int)dgvUsersData.CurrentRow.Cells[0].Value);
            frmChangePassword.ShowDialog();
            LoadUsers();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvUsersData.CurrentRow == null) return;

            frmUserInfo frmUserInfo = new frmUserInfo((int)dgvUsersData.CurrentRow.Cells[0].Value);
            frmUserInfo.ShowDialog();
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Future is not Implemeneted Yet!");
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Future is not Implemeneted Yet!");
        }

        // ----- Filter Event -----
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cbFilter.SelectedItem.ToString();

            if (selected == "None")
            {
                tbFilter.Visible = false;
                cbIsActive.Visible = false;
                LoadUsers();
            }
            else if (selected == "Is Active")
            {
                tbFilter.Visible = false;
                cbIsActive.Visible = true;
            }
            else
            {
                tbFilter.Visible = true;
                cbIsActive.Visible = false;
            }
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            string filterColumn = "";

            switch (cbFilter.SelectedItem.ToString())
            {
                case "User ID": filterColumn = "UserID"; break;
                case "UserName": filterColumn = "UserName"; break;
                case "Person ID": filterColumn = "PersonID"; break;
                case "Full Name": filterColumn = "FullName"; break;
            }

            DataTable dt = clsUser.GetUsersByFilter(filterColumn, tbFilter.Text);
            dgvUsersData.DataSource = dt;
            _FormatGrid();
            lblRecords.Text = dt.Rows.Count.ToString();
        }

        // Restrict input to digits (Backspace still allowed) when filtering by
        // Person ID or User ID, since those columns are numeric.
        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (cbFilter.SelectedItem.ToString())
            {
                case "Person ID":
                case "User ID":
                    e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back;
                    break;
            }
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt;

            switch (cbIsActive.SelectedItem.ToString())
            {
                case "Yes":
                    dt = clsUser.GetUsersByFilter("IsActive", "1");
                    break;
                case "No":
                    dt = clsUser.GetUsersByFilter("IsActive", "0");
                    break;
                default: // "All"
                    dt = clsUser.GetUsers();
                    break;
            }

            dgvUsersData.DataSource = dt;
            _FormatGrid();
            lblRecords.Text = dt.Rows.Count.ToString();
        }

    }
}
