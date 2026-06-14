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
    public partial class frmUserInfo : Form
    {

        private int _UserID;
        public frmUserInfo()
        {
            InitializeComponent();
        }

        public frmUserInfo(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }

        private void frmUserInfo_Load(object sender, EventArgs e)
        {

            clsUser user = clsUser.FindByUserID(_UserID);

            if (user != null)
            {
                ucPersonDetails1.Person = clsPeople.FindPersonByID(user.PersonID);

                lblUserID.Text = user.UserID.ToString();
                lblUserName.Text = user.UserName;
                lblIsActive.Text = user.isActive ? "Yes" : "No";
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
