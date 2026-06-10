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
    public partial class frmMain : Form
    {
        private clsUser _User;
        public frmMain()
        {
            InitializeComponent();
        }

        public frmMain(clsUser User)
        {
            InitializeComponent();
            _User = User;
        }

        private void btnPeople_Click(object sender, EventArgs e)
        {
            frmPeople frmPeople = new frmPeople();
            frmPeople.ShowDialog();
        }
    }
}
