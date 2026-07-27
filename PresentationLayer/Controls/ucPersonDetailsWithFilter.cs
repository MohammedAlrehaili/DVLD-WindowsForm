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
    public partial class ucPersonDetailsWithFilter : UserControl
    {
        
        public clsPeople Person
        {
            get { return ucPersonDetails1.Person; }
            set { ucPersonDetails1.Person = value; }
        }

        public ucPersonDetailsWithFilter()
        {
            InitializeComponent();

            cbFilter.Items.Add("NationalNo");
            cbFilter.Items.Add("PersonID");
            cbFilter.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(tbFilter.Text == "")
            {
                MessageBox.Show("Please fill the filter field", "Required",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            clsPeople person = null;

            switch(cbFilter.SelectedItem.ToString())
            {
                case "NationalNo":
                    person = clsPeople.FindPersonByNationalNo(tbFilter.Text);
                    break;
                case "PersonID":
                    person = clsPeople.FindPersonByID(Convert.ToInt32(tbFilter.Text));
                    break;
            }

            if(person != null)
            {
                ucPersonDetails1.Person = person;
            }
            else
            {
                MessageBox.Show("Person not found!", "Not Found",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Restrict input to digits (Backspace still allowed) when filtering by
            // PersonID, since that column is numeric.
            if (cbFilter.SelectedItem.ToString() == "PersonID")
                e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddPerson frmAddPerson = new frmAddPerson();
            frmAddPerson.ShowDialog();

            if(frmAddPerson.newPersonID != -1)
            {
                clsPeople person = clsPeople.FindPersonByID(frmAddPerson.newPersonID);
                if(person != null)
                {
                    ucPersonDetails1.Person = person;
                }
            }
        }
    }
}
