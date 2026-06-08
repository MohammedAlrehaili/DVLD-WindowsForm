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
    public partial class ucPersonDetails : UserControl
    {
        public ucPersonDetails()
        {
            InitializeComponent();
        }

        private clsPeople _person;

        public clsPeople Person
        {
            get { return _person; }
            set
            {
                _person = value;
                _FillData();
            }
        }

        private void _FillData()
        {
            if (_person == null) return;

            lblPersonID.Text = _person.PersonID.ToString();
            lblName.Text = _person.FirstName + " " + _person.SecondName + " " +
                                 _person.ThirdName + " " + _person.LastName;
            lblNationalNo.Text = _person.NationalNo;
            lblDateOfBirth.Text = _person.DateOfBirth.ToShortDateString();
            lblGender.Text = _person.Gender == 0 ? "Male" : "Female";
            lblPhone.Text = _person.Phone;
            lblEmail.Text = _person.Email;
            lblAddress.Text = _person.Address;
            lblCountry.Text = clsCountry.GetCountryName(_person.NationalityCountryID);

            pbProfilePicture.Image = _person.ImagePath != ""
                                ? Image.FromFile(_person.ImagePath)
                                : null;


        }
    }
}
