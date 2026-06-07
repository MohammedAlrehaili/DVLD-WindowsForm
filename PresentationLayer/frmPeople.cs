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
        public frmPeople()
        {
            InitializeComponent();
        }

        private void LoadPeopleData()
        {
            DataTable dt = clsPeople.GetAllContacts();
            dgvPeopleData.DataSource = dt;
            lblRecords.Text = dt.Rows.Count.ToString();
        }

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
            clsPeople.DeletePersonByID((int)dgvPeopleData.CurrentRow.Cells[0].Value);
            LoadPeopleData();
        }

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
    }
}
