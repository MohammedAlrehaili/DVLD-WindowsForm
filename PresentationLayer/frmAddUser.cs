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
    public partial class frmAddUser : Form
    {
        public frmAddUser()
        {
            InitializeComponent();
        }

        private void frmAddUser_Load(object sender, EventArgs e)
        {
            cbFilter.Items.Add("NationalNo");

            cbFilter.SelectedIndex = 0;
        }

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
                    }
                    else
                    {
                        MessageBox.Show("Person not found!", "Not Found",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;
            }
        }
    }
}
