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

        private clsLocalDrivingLicenseApplications(int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID)
        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.ApplicationID = ApplicationID;
            this.LicenseClassID = LicenseClassID;
        }

        public static int GetPassedTestsCount(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenseApplicationsDataAccess.GetPassedTestsCount(LocalDrivingLicenseApplicationID);
        }

        public bool Save()
        {
            this.LocalDrivingLicenseApplicationID =
                clsLocalDrivingLicenseApplicationsDataAccess.AddNewLocalDrivingLicenseApplication(
                    this.ApplicationID, this.LicenseClassID);

            return (this.LocalDrivingLicenseApplicationID != -1);
        }

        public static clsLocalDrivingLicenseApplications GetLocalDrivingLicenseApplicationByID(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID = -1;
            int LicenseClassID = -1;

            if(clsLocalDrivingLicenseApplicationsDataAccess.GetLocalDrivingLicenseApplicationByID(LocalDrivingLicenseApplicationID,ref ApplicationID,ref LicenseClassID))
            {
                return new clsLocalDrivingLicenseApplications(LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID);
            }
            return null;
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

        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            clsLocalDrivingLicenseApplications app = GetLocalDrivingLicenseApplicationByID(LocalDrivingLicenseApplicationID);

            if (app == null)
                return false;

            clsApplications baseApp = clsApplications.FindByApplicationID(app.ApplicationID);

            if (baseApp == null)
                return false;

            if (baseApp.ApplicationStatus != (byte)clsApplications.enApplicationStatus.New)
                return false;

            return clsLocalDrivingLicenseApplicationsDataAccess.DeleteLocalDrivingLicenseApplication(
                LocalDrivingLicenseApplicationID, app.ApplicationID);
        }
    }
}
