using System;
using BussinessLayer;
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
    public partial class frmPeople : Form
    {

        // ----- Private Functions -----
        private void LoadPeopleData()
        {
            DataTable dt = clsPeople.GetPeopleData();
            dgvPeopleData.DataSource = dt;
            lblRecords.Text = dt.Rows.Count.ToString();
        }

        // ----- Constructors -----
        public frmPeople()
        {
            InitializeComponent();
        }

        // ----- Form Events -----
        private void frmPeople_Load(object sender, EventArgs e)
        {
            LoadPeopleData();

            cbFilter.Items.Add("None");
            cbFilter.Items.Add("PersonID");
            cbFilter.Items.Add("NationalNo");
            cbFilter.Items.Add("FirstName");
            cbFilter.Items.Add("SecondName");
            cbFilter.Items.Add("ThirdName");
            cbFilter.Items.Add("LastName");
            cbFilter.Items.Add("Nationality");
            cbFilter.Items.Add("Gender");
            cbFilter.Items.Add("Phone");
            cbFilter.Items.Add("Email");
            cbFilter.SelectedIndex = 0;
        }

        // ----- Click Methods -----
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddPerson frmAddPerson = new frmAddPerson();
            frmAddPerson.ShowDialog();
            LoadPeopleData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddPerson frmAddPerson = new frmAddPerson((int)dgvPeopleData.CurrentRow.Cells[0].Value);
            frmAddPerson.ShowDialog();
            LoadPeopleData();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvPeopleData.CurrentRow == null) return;

            int personID = (int)dgvPeopleData.CurrentRow.Cells[0].Value;

            DialogResult result = MessageBox.Show("Are you sure you want to delete this person?",
                                                  "Confirm Delete",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // get image path before deleting from DB
                clsPeople Person = clsPeople.FindPersonByID(personID);

                if (clsPeople.DeletePersonByID(personID))
                {
                    // delete image file after successful DB delete
                    if (Person != null && Person.ImagePath != "" &&
                        System.IO.File.Exists(Person.ImagePath))
                    {
                        System.IO.File.Delete(Person.ImagePath);
                    }

                    MessageBox.Show("Person deleted successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPeopleData();
                }
                else
                {
                    MessageBox.Show("Failed to delete person!", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonDetails frmPersonDetails = new frmPersonDetails((int)dgvPeopleData.CurrentRow.Cells[0].Value);
            frmPersonDetails.ShowDialog();
            LoadPeopleData();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddPerson frmaddperson = new frmAddPerson();
            frmaddperson.ShowDialog();
            LoadPeopleData();
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Future is not Implemeneted Yet!");

        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Future is not Implemeneted Yet!");
        }

        // ----- Filter Events -----
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedItem.ToString() != "None")
            {
                tbFilter.Visible = true;
            }
            else
            {
                tbFilter.Visible = false;
                LoadPeopleData();
            }
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {

            string filterColumn = "";

            switch (cbFilter.SelectedItem.ToString())
            {
                case "PersonID":
                    filterColumn = "PersonID";
                    break;
                case "NationalNo":
                    filterColumn = "NationalNo";
                    break;  
                case "FirstName":
                    filterColumn = "FirstName";
                    break;
                case "SecondName":
                    filterColumn = "SecondName";
                    break;
                case "ThirdName":
                    filterColumn = "ThirdName";
                    break;
                case "LastName":
                    filterColumn = "LastName";
                    break;
                case "Nationality":
                    filterColumn = "NationalityCountryID";
                    break;
                case "Gender":
                    filterColumn = "Gender"; 
                    break;
                case "Phone":
                    filterColumn = "Phone";
                    break;
                case "Email":
                    filterColumn = "Email";
                    break;
            }

            DataTable dt = clsPeople.GetPeopleByFilter(filterColumn, tbFilter.Text);
            dgvPeopleData.DataSource = dt;
            lblRecords.Text = dt.Rows.Count.ToString();
        }

        // Restrict input to digits (Backspace still allowed) when filtering by
        // Person ID only since it columns are numeric.
        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (cbFilter.SelectedItem.ToString())
            {
                case "PersonID":
                    e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back;
                    break;
            }
        }
    }
}
