using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static BussinessLayer.clsPeople;

namespace BussinessLayer
{

    public class clsUser
    {

        public static clsUser CurrentUser = null;
        public enum enMode { AddNew, Update }

        public enMode Mode = enMode.AddNew;
        
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool isActive { get; set; }

        public clsUser() {
            UserID = -1;
            PersonID = -1;
            UserName = "";
            Password = "";
            isActive = false;
            Mode = enMode.AddNew;
        }

        private clsUser(int UserID, int PersonID, string UserName, string Password, bool isActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.Password = Password;
            this.isActive = isActive;
            Mode = enMode.Update;
        }

        private bool _AddNewUser()
        {
            this.UserID = clsUserDataAccess.AddUser(this.PersonID, this.UserName, this.Password, this.isActive);

            return (this.UserID != -1);
        }

        private bool _UpdateUser()
        {
            return clsUserDataAccess.UpdateUser(this.UserID, this.PersonID, this.UserName, this.Password, this.isActive);
        }

        public static bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return clsHelper.HashPassword(plainPassword) == hashedPassword;
        }

        public class InactiveAccountException : Exception
        {
            public InactiveAccountException() : base("Account is inactive.") { }
        }

        public static clsUser Login(string UserName, string Password)
        {
            int UserID = -1, PersonID = -1;
            bool isActive = false;

            if(clsUserDataAccess.GetUserByUsernameAndPassword(UserName,Password, ref UserID, ref PersonID,ref isActive))
            {
                if (!isActive) throw new InactiveAccountException();


                CurrentUser = new clsUser(UserID, PersonID, UserName, Password, isActive);
                return CurrentUser;
            }
            return null;
        }

        public static DataTable GetUsers()
        {
            return clsUserDataAccess.GetUsers();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateUser();
            }
            return false;
        }

        public static bool IsPersonIDExisit(int PersonID)
        {
            return clsUserDataAccess.IsPersonIDExisit(PersonID);
        }

        public static DataTable GetUsersByFilter(string FilterColumn, string FilterValue)
        {
            return clsUserDataAccess.GetUsersByFilter(FilterColumn, FilterValue);
        }

        // BusinessLayer
        public static clsUser FindByUserID(int UserID)
        {
            int personID = -1;
            string userName = "", password = "";
            bool isActive = false;

            if (clsUserDataAccess.FindByUserID(UserID, ref personID, ref userName, ref password, ref isActive))
            {
                return new clsUser(UserID, personID, userName, password, isActive);
            }
            return null;
        }

        public static bool UpdatePassword(int UserID, string Password)
        {
            return clsUserDataAccess.UpdatePassword(UserID, Password);
        }

        public static bool DeleteUser(int UserID)
        {
            return clsUserDataAccess.DeleteUser(UserID);
        }

    
    }
}
