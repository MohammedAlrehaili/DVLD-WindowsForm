using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsLicenseClasses
    {

        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public short MinimumAllowedAge { get; set; }
        public short DefaultValidityLength { get; set; }
        public short ClassFees { get; set; }

        public clsLicenseClasses()
        {
            LicenseClassID = -1;
            ClassName = "";
            ClassDescription = "";
            MinimumAllowedAge = 0;
            DefaultValidityLength = 0;
            ClassFees = 0;
        }

        private clsLicenseClasses(int LicenseClassID, string ClassName, string ClassDescription, short MinimumAllowedAge, short DefaultValidityLength, short ClassFees)
        {
            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees = ClassFees;
        }

        public static clsLicenseClasses GetLicenseClassesByID(int LicenseClassID)
        {
            string ClassName = "";
            string ClassDesciption = "";
            short MinimumAllowedAge = 0;
            short DefaultValidityLength = 0;
            short ClassFees = 0;

            if(clsLicenseClassesDataAccess.GetLicenseClassesByID(LicenseClassID, ref ClassName, ref ClassDesciption, ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))
            {
                return new clsLicenseClasses(LicenseClassID, ClassName, ClassDesciption, MinimumAllowedAge, DefaultValidityLength, ClassFees);
            }
            return null;
        }

        public static DataTable GetLicenseClasses()
        {
            return clsLicenseClassesDataAccess.GetLicenseClasses();
        }
    }
}
