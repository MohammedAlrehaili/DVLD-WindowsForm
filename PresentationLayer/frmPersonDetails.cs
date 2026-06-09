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
    public partial class frmPersonDetails : Form
    {

        // ----- Private Fields -----
        private int _PersonID;

        public frmPersonDetails(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }

        private void frmPersonDetails_Load(object sender, EventArgs e)
        {
            clsPeople person = clsPeople.FindPersonByID(_PersonID);
            if (person != null)
            {
                ucPersonDetails1.Person = person;
            }
        }

        // ----- Click Methods -----

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // -------------------------

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddPerson frmaddperson = new frmAddPerson(_PersonID);
            frmaddperson.ShowDialog();
        }
    }
}
