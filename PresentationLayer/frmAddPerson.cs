using BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BussinessLayer.clsPeople;

namespace PresentationLayer
{
    public partial class frmAddPerson : Form
    {

        // ----- Private Fields -----
        private int _PersonID;
        private enMode _Mode = enMode.AddNew;
        private bool _ImageRemoved = false;
        private string _OldImagePath = "";

        // --------------------------

        // ----- Public Fields -----

        public int newPersonID { get; private set; } = -1;

        // --------------------------

        // ----- Private Methods -----
        private Image _LoadImage(string path)
        {
            using (var temp = Image.FromFile(path))
            {
                return new Bitmap(temp);
            }
        }

        private void _DeleteOldImage()
        {
            if (_OldImagePath != "" && System.IO.File.Exists(_OldImagePath))
            {
                // release the image first
                pbPictureProfile.Image?.Dispose();
                pbPictureProfile.Image = null;

                System.IO.File.Delete(_OldImagePath);
            }
        }

        // --------------------------

        public frmAddPerson()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddPerson(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
            _Mode = clsPeople.enMode.Update;
        }

        private void frmAddPerson_Load(object sender, EventArgs e)
        {
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);

            cbCountry.DataSource = clsCountry.GetAllCountries();
            cbCountry.DisplayMember = "CountryName";
            cbCountry.ValueMember = "CountryID";
            cbCountry.SelectedIndex = 0;

            rbMale.Checked = true;

            if (_Mode == enMode.AddNew)
            {
                lblPersonID.Text = "";
                lblHeader.Text = "Add New Person";
                lbRemove.Visible = false;
            }
            else if (_Mode == enMode.Update)
            {
                lblHeader.Text = "Update Person";

                clsPeople Person = clsPeople.FindPersonByID(_PersonID);
                _OldImagePath = Person.ImagePath;
                lbRemove.Visible = Person.ImagePath != "";
                if (Person != null)
                {
                    lblPersonID.Text = _PersonID.ToString();
                    tbFirstName.Text = Person.FirstName;
                    tbSecondName.Text = Person.SecondName;
                    tbThirdName.Text = Person.ThirdName;
                    tbLastName.Text = Person.LastName;
                    tbNationalID.Text = Person.NationalNo;
                    dtpDateOfBirth.Value = Person.DateOfBirth;
                    rbMale.Checked = Person.Gender == 0;
                    rbFemale.Checked = Person.Gender == 1;
                    tbPhone.Text = Person.Phone;
                    tbEmail.Text = Person.Email;
                    cbCountry.SelectedValue = Person.NationalityCountryID;
                    tbAddress.Text = Person.Address;
                    pbPictureProfile.Image = Person.ImagePath != ""
                                            ? _LoadImage(Person.ImagePath)
                                            : null;
                }
            }
        }

        // ----- Click Methods -----
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbFirstName.Text == "" || tbSecondName.Text == "" ||
                tbLastName.Text == "" || tbNationalID.Text == "" ||
                tbPhone.Text == "" || tbAddress.Text == "")
            {
                MessageBox.Show("Please fill all required fields!", "Validation",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            clsPeople Person = _Mode == enMode.Update
                             ? clsPeople.FindPersonByID(_PersonID)
                             : new clsPeople();

            Person.FirstName = tbFirstName.Text;
            Person.SecondName = tbSecondName.Text;
            Person.ThirdName = tbThirdName.Text;
            Person.LastName = tbLastName.Text;
            Person.NationalNo = tbNationalID.Text;
            Person.DateOfBirth = dtpDateOfBirth.Value;
            Person.Gender = (byte)(rbMale.Checked ? 0 : 1);
            Person.NationalityCountryID = (int)cbCountry.SelectedValue;
            Person.Email = tbEmail.Text;
            Person.Phone = tbPhone.Text;
            Person.Address = tbAddress.Text;

            if (pbPictureProfile.Image != null && openFileDialog1.FileName != "")
            {
                _DeleteOldImage();

                string imagePath = @"C:\DVLD-People-Images";
                if (!System.IO.Directory.Exists(imagePath))
                    System.IO.Directory.CreateDirectory(imagePath);

                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(openFileDialog1.FileName);
                string fullPath = System.IO.Path.Combine(imagePath, fileName);

                // ✅ Copy file directly instead of saving from PictureBox
                System.IO.File.Copy(openFileDialog1.FileName, fullPath);

                Person.ImagePath = fullPath;
            }
            else if (_ImageRemoved)
            {
                // user removed the image
                _DeleteOldImage();
                Person.ImagePath = "";
            }
            else
            {
                // user didn't change anything — keep old path
                Person.ImagePath = _Mode == enMode.Update
                                 ? _OldImagePath
                                 : "";
            }

            try
            {
                if (Person.Save())
                {
                    MessageBox.Show(_Mode == enMode.AddNew ? "Person added successfully!" : "Person updated successfully!",
                                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    newPersonID = Person.PersonID;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed. Please try again.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lbSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            openFileDialog1.Title = "Select an Image";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbPictureProfile.Image = _LoadImage(openFileDialog1.FileName);
                _ImageRemoved = false;
                lbRemove.Visible = true;
            }
        }

        private void lbRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPictureProfile.Image = null;
            openFileDialog1.FileName = "";
            _ImageRemoved = true;
            lbRemove.Visible = false;
        }

        // -------------------------

        // Validation Of NationalD Field
        private void tbNationalID_Leave(object sender, EventArgs e)
        {
            if (clsPeople.SearchPersonByNationalID(tbNationalID.Text))
            {
                epNationalNo.SetError(tbNationalID, "This National ID is Already used!");
            }
            else
            {
                epNationalNo.SetError(tbNationalID, "");
            }

        }
        // -------------------------

        // ------ Validation Of Email Field -----
        private void tbEmail_Leave(object sender, EventArgs e)
        {
            if (tbEmail.Text != "")
            {
                try
                {
                    System.Net.Mail.MailAddress mail = new System.Net.Mail.MailAddress(tbEmail.Text);
                    epEmail.SetError(tbEmail, "");
                }
                catch
                {
                    epEmail.SetError(tbEmail, "Invalid email format!");
                }
            }
            else
            {
                epEmail.SetError(tbEmail, "");
            }
        }
        
        // -------------------------
    }
}
