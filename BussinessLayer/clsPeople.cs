using System;
using DataAccessLayer;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsPeople
    {

        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }
        public string ImagePath { get; set; }

        public clsPeople()
        {
            PersonID = -1;
            NationalNo = "";
            FirstName = "";
            SecondName = "";
            ThirdName = "";
            LastName = "";
            DateOfBirth = DateTime.Now;
            Gender = 0;
            Address = "";
            Phone = "";
            Email = "";
            NationalityCountryID = -1;
            ImagePath = "";

            Mode = enMode.AddNew;
        }

        private clsPeople(int personID, string nationalNo, string firstName, string secondName, string thirdName, string lastName, DateTime dateOfBirth, byte gender, string address, string phone, string email, int nationalityCountryID, string imagePath)
        {
            PersonID = personID;
            NationalNo = nationalNo;
            FirstName = firstName;
            SecondName = secondName;
            ThirdName = thirdName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address;
            Phone = phone;
            Email = email;
            NationalityCountryID = nationalityCountryID;
            ImagePath = imagePath;

            Mode = enMode.Update;
        }

        private bool _AddNewPerson()
        {
            this.PersonID = clsPeopleDataAccess.AddPerson(this.NationalNo, this.FirstName, this.SecondName,
                this.ThirdName, this.LastName, this.DateOfBirth, this.Gender, this.Address,
                this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);
            
            return (this.PersonID != -1);
        }

        private bool _UpdatePerson()
        {
            return clsPeopleDataAccess.UpdatePerson(this.PersonID, this.NationalNo, this.FirstName, this.SecondName,
                this.ThirdName, this.LastName, this.DateOfBirth, this.Gender, this.Address, this.Phone, this.Email,
                this.NationalityCountryID, this.ImagePath);
        }

        public static bool DeletePersonByID(int PersonID)
        {
            return clsPeopleDataAccess.DeletePersonByID(PersonID);
        }

        public static clsPeople FindPersonByID(int PersonID)
        {
            string nationalNo = "", firstName= "", secondName = "", thirdName = "", lastName = "", address = "", phone = "", email = "", imagePath = "";
            byte Gender = 0;
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1;

            if (clsPeopleDataAccess.GetPersonByID(PersonID, ref nationalNo, ref firstName, ref secondName, ref thirdName, ref lastName, ref DateOfBirth, ref Gender, ref address, ref phone, ref email, ref NationalityCountryID, ref imagePath))
            {
                return new clsPeople(PersonID, nationalNo, firstName, secondName, thirdName, lastName, DateOfBirth, Gender, address, phone, email, NationalityCountryID, imagePath);
            }

            return null;
        }

        public static DataTable GetPeopleByFilter(string FilterColumn, string FilterValue)
        {
            return clsPeopleDataAccess.GetPeopleByFilter(FilterColumn, FilterValue);
        }

        public static DataTable GetAllContacts()
        {
            return clsPeopleDataAccess.GetAllContacts();
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdatePerson();
            }
            return true;
        }

        public static bool SearchPersonByNationalID(string NationalID)
        {
            return clsPeopleDataAccess.SearchPersonByNationalNo(NationalID);
        }
    }
}
