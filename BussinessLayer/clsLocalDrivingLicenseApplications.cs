using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsLocalDrivingLicenseApplications
    {

        public int LocalDrivingLicenseApplicationID { get; set; }
        public int ApplicationID { get; set; }
        public int LicenseClassID { get; set; }

        public clsLocalDrivingLicenseApplications()
        {
            LocalDrivingLicenseApplicationID = -1;
            ApplicationID = -1;
            LicenseClassID = -1;
        }

        public bool Save()
        {
            this.LocalDrivingLicenseApplicationID =
                clsLocalDrivingLicenseApplicationsDataAccess.AddNewLocalDrivingLicenseApplication(
                    this.ApplicationID, this.LicenseClassID);

            return (this.LocalDrivingLicenseApplicationID != -1);
        }

        public static DataTable GetLocalDrivingLicenseApplicationsByFilter(string FilterColumn, string FilterValue)
        {
            return clsLocalDrivingLicenseApplicationsDataAccess.GetLocalDrivingLicenseApplicationsByFilter(FilterColumn, FilterValue);
        }

        public static bool DoesPersonHaveActiveApplicationForClass(int ApplicantPersonID, int LicenseClassID)
        {
            return clsLocalDrivingLicenseApplicationsDataAccess.DoesPersonHaveActiveApplicationForClass(ApplicantPersonID, LicenseClassID);
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseApplicationsDataAccess.GetAllLocalDrivingLicenseApplications();
        }
    }
}
