using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsUser
    {

        public static clsUser CurrentUser = null;

        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        bool isActive { get; set; }

        clsUser() { 
            
        }

        clsUser(int UserID, int PersonID, string UserName, string Password, bool isActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.Password = Password;
            this.isActive = isActive;
        }

        public static clsUser Login(string UserName, string Password)
        {
            int UserID = -1, PersonID = -1;
            bool isActive = false;

            if(clsUserDataAccess.GetUserByUsernameAndPassword(UserName,Password, ref UserID, ref PersonID,ref isActive))
            {
                if (!isActive) return null;


                CurrentUser = new clsUser(UserID, PersonID, UserName, Password, isActive);
                return CurrentUser;
            }
            return null;
        }
    }
}
