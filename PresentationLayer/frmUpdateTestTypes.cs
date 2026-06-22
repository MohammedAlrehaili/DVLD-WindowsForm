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
    public partial class frmUpdateTestTypes : Form
    {

        private int _TestTypeID;

        public frmUpdateTestTypes()
        {
            InitializeComponent();
        }

        public frmUpdateTestTypes(int TestTypeID)
        {
            InitializeComponent();
            _TestTypeID = TestTypeID;
        }

        private void frmUpdateTestTypes_Load(object sender, EventArgs e)
        {
            clsTestTypes TestType = clsTestTypes.GetTestTypeByID(_TestTypeID);

            lblID.Text = TestType.TestTypeID.ToString();
            tbTitle.Text = TestType.TestTypeTitle;
            tbDescription.Text = TestType.TestTypeDescription;
            tbFees.Text = TestType.TestTypeFees.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbTitle.Text == "" || tbDescription.Text == "" || tbFees.Text == "")
            {
                MessageBox.Show("Please Fill the fields", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!short.TryParse(tbFees.Text, out short fees))
            {
                MessageBox.Show("Fees must be a valid number!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            clsTestTypes TestType = clsTestTypes.GetTestTypeByID(_TestTypeID);

            if (TestType != null)
            {
                TestType.TestTypeTitle = tbTitle.Text;
                TestType.TestTypeDescription = tbDescription.Text;
                TestType.TestTypeFees = fees;

                try
                {
                    if (TestType.Update())
                    {
                        MessageBox.Show("Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Update Failed", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
