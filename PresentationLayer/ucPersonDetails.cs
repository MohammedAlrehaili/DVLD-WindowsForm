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

        private clsPeople _Person;

        private Image _LoadImage(string path)
        {
            using (var temp = Image.FromFile(path))
            {
                return new Bitmap(temp);
            }
        }

        public ucPersonDetails()
        {
            InitializeComponent();
        }

        public clsPeople Person
        {
            get { return _Person; }
            set
            {
                _Person = value;
                _FillData();
            }
        }

        private void _FillData()
        {
            if (_Person == null) return;

            lblPersonID.Text = _Person.PersonID.ToString();
            lblName.Text = _Person.FirstName + " " + _Person.SecondName + " " +
                                 _Person.ThirdName + " " + _Person.LastName;
            lblNationalNo.Text = _Person.NationalNo;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblGender.Text = _Person.Gender == 0 ? "Male" : "Female";
            lblPhone.Text = _Person.Phone;
            lblEmail.Text = _Person.Email;
            lblAddress.Text = _Person.Address;
            lblCountry.Text = clsCountry.GetCountryName(_Person.NationalityCountryID);

            pbProfilePicture.Image = _Person.ImagePath != ""
                                ? _LoadImage(_Person.ImagePath)
                                : null;
        }
    }
}
