using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsApplicationTypes
    {

        public int ApplicationTypeID { get; set; }
        public string ApplicationTypeTitle { get; set; }
        public short ApplicationFees { get; set; }

        public clsApplicationTypes()
        {
            ApplicationTypeID = -1;
            ApplicationTypeTitle = "";
            ApplicationFees = 0;
        }

        private clsApplicationTypes(int ID, string Title, short Fees)
        {
            this.ApplicationTypeID = ID;
            this.ApplicationTypeTitle = Title;
            this.ApplicationFees = Fees;
        }

        public static DataTable GetApplicationTypes()
        {
            return clsApplicationTypesDataAccess.GetApplicationTypes();
        }

        public static clsApplicationTypes GetApplicationTypeByID(int ID)
        {
            string Title = "";
            short Fees = 0;

            if(clsApplicationTypesDataAccess.GetApplicationTypeByID(ID,ref Title,ref Fees))
            {
                return new clsApplicationTypes(ID, Title, Fees);
            }
            return null;
        }

        public bool Update()
        {
            return clsApplicationTypesDataAccess.UpdateApplicationTypes(this.ApplicationTypeID, this.ApplicationTypeTitle, this.ApplicationFees);
        }
    }
}
