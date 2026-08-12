using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsApplications
    {
        public enum enApplicationStatus : byte
        {
            New = 1,
            Cancelled = 2,
            Completed = 3
        }

        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }
        public byte ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }

        public clsApplications()
        {
            ApplicationID = -1;
            ApplicantPersonID = -1;
            ApplicationDate = DateTime.Now;
            ApplicationTypeID = 0;
            ApplicationStatus = (byte)enApplicationStatus.New;
            LastStatusDate = DateTime.Now;
            PaidFees = 0;
            CreatedByUserID = -1;
        }

        // BussinessLayer/clsApplications.cs

        private clsApplications(int applicationID, int applicantPersonID, DateTime applicationDate,
            int applicationTypeID, byte applicationStatus, DateTime lastStatusDate,
            decimal paidFees, int createdByUserID)
        {
            ApplicationID = applicationID;
            ApplicantPersonID = applicantPersonID;
            ApplicationDate = applicationDate;
            ApplicationTypeID = applicationTypeID;
            ApplicationStatus = applicationStatus;
            LastStatusDate = lastStatusDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;
        }

        public static clsApplications FindByApplicationID(int ApplicationID)
        {
            int applicantPersonID = -1, applicationTypeID = -1, createdByUserID = -1;
            DateTime applicationDate = DateTime.Now, lastStatusDate = DateTime.Now;
            byte applicationStatus = 0;
            decimal paidFees = 0;

            if (clsApplicationsDataAccess.GetApplicationByID(ApplicationID, ref applicantPersonID, ref applicationDate,
                ref applicationTypeID, ref applicationStatus, ref lastStatusDate, ref paidFees, ref createdByUserID))
            {
                return new clsApplications(ApplicationID, applicantPersonID, applicationDate,
                    applicationTypeID, applicationStatus, lastStatusDate, paidFees, createdByUserID);
            }
            return null;
        }

        public bool Save()
        {
            this.ApplicationID = clsApplicationsDataAccess.AddNewApplication(
                this.ApplicantPersonID, this.ApplicationDate, this.ApplicationTypeID,
                this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);

            return (this.ApplicationID != -1);
        }

        public static bool CancelApplication(int ApplicationID)
        {
            clsApplications app = FindByApplicationID(ApplicationID);

            if (app == null)
                return false;

            if (app.ApplicationStatus == (byte)enApplicationStatus.Completed)
                return false;

            return clsApplicationsDataAccess.UpdateApplicationStatus(ApplicationID, (byte)enApplicationStatus.Cancelled);
        }

        public static bool CompleteApplication(int ApplicationID)
        {
            clsApplications app = FindByApplicationID(ApplicationID);
            if (app == null)
                return false;
            if (app.ApplicationStatus == (byte)enApplicationStatus.Cancelled)
                return false;
            return clsApplicationsDataAccess.UpdateApplicationStatus(ApplicationID, (byte)enApplicationStatus.Completed);
        }
    }
}