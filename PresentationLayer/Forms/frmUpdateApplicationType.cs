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
    public partial class frmUpdateApplicationType : Form
    {

        private int _ApplicationTypeID;

        public frmUpdateApplicationType()
        {
            InitializeComponent();
        }

        public frmUpdateApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();
            _ApplicationTypeID = ApplicationTypeID;
        }

        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            clsApplicationTypes ApplicationType = clsApplicationTypes.GetApplicationTypeByID(_ApplicationTypeID);

            lblID.Text = ApplicationType.ApplicationTypeID.ToString();
            tbTitle.Text = ApplicationType.ApplicationTypeTitle;
            tbFees.Text = ApplicationType.ApplicationFees.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(tbTitle.Text == "" || tbFees.Text == "")
            {
                MessageBox.Show("Please Fill the fields","Required",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            if (!short.TryParse(tbFees.Text, out short fees))
            {
                MessageBox.Show("Fees must be a valid number!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            clsApplicationTypes ApplicationType = clsApplicationTypes.GetApplicationTypeByID(_ApplicationTypeID);

            if (ApplicationType != null)
            {

                ApplicationType.ApplicationTypeTitle = tbTitle.Text;
                ApplicationType.ApplicationFees = fees;

                try
                {
                    if (ApplicationType.Update())
                    {
                        MessageBox.Show("Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Update Failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
    }
}
