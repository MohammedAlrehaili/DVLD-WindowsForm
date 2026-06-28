using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsTestAppointments
    {

        public enum enMode {AddNew = 0, Update = 1};
        public enMode Mode = enMode.AddNew;

        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public short PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }

        public clsTestAppointments()
        {
            TestAppointmentID = -1;
            TestTypeID = -1;
            LocalDrivingLicenseApplicationID = -1;
            AppointmentDate = DateTime.Today;
            PaidFees = 0;
            CreatedByUserID = -1;
            IsLocked = false;
            Mode = enMode.AddNew;
        }

        private clsTestAppointments(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, short PaidFees, int CreatedByUserID, bool isLocked)
        {
            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsLocked = isLocked;
            Mode = enMode.Update;
        }

        private bool _AddTestAppointment()
        {
            this.TestAppointmentID = clsTestAppointmentsDataAccess.AddTestAppointment(this.TestTypeID, this.LocalDrivingLicenseApplicationID, this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked);

            return (this.TestAppointmentID != -1);
        }



        public static DataTable GetTestAppointmentsByLDLAppID(int LocalDrivingLicenseApplicationID)
        {
            return clsTestAppointmentsDataAccess.GetTestAppointmentsByLDLAppID(LocalDrivingLicenseApplicationID);
        }

        public bool Save()
        {

            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddTestAppointment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    break;
            }
            return false;
        }
    }
}
