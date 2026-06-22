using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsLocalDrivingLicenseApplicationsDataAccess
    {

        public static bool DoesPersonHaveActiveApplicationForClass(int ApplicantPersonID, int LicenseClassID)
        {
            bool found = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT COUNT(*)
                    FROM LocalDrivingLicenseApplications LDLA
                    JOIN Applications A ON LDLA.ApplicationID = A.ApplicationID
                    WHERE A.ApplicantPersonID = @ApplicantPersonID
                      AND LDLA.LicenseClassID = @LicenseClassID
                      AND A.ApplicationStatus <> 2";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();
                int count = (int)command.ExecuteScalar();
                found = (count > 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return found;
        }

        public static int AddNewLocalDrivingLicenseApplication(int ApplicationID, int LicenseClassID)
        {
            int LocalDrivingLicenseApplicationID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO LocalDrivingLicenseApplications (ApplicationID, LicenseClassID)
                    VALUES (@ApplicationID, @LicenseClassID);
                    SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LocalDrivingLicenseApplicationID = insertedID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

            return LocalDrivingLicenseApplicationID;
        }

        public static DataTable GetLocalDrivingLicenseApplicationsByFilter(string FilterColumn, string FilterValue)
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = $@"SELECT * FROM 
            (   
            SELECT 
            LDLA.LocalDrivingLicenseApplicationID AS [L.D.L.AppID],
            LDLA.ApplicationID AS [ApplicationID],
            LC.ClassName AS [Driving Class],
            P.NationalNo AS [National No.],
            (P.FirstName + ' ' + P.SecondName + ' ' + ISNULL(P.ThirdName + ' ', '') + P.LastName) AS [Full Name],
            A.ApplicationDate AS [Application Date],
            (
            SELECT COUNT(*) 
            FROM TestAppointments TA
            JOIN Tests T ON T.TestAppointmentID = TA.TestAppointmentID
            WHERE TA.LocalDrivingLicenseApplicationID = LDLA.LocalDrivingLicenseApplicationID
                AND T.TestResult = 1
            ) AS [Passed Tests],
            CASE A.ApplicationStatus
                WHEN 1 THEN 'New'
                WHEN 2 THEN 'Cancelled'
                WHEN 3 THEN 'Completed'
                ELSE 'Unknown'
            END AS [Status]                        
            FROM LocalDrivingLicenseApplications LDLA
            JOIN Applications A ON LDLA.ApplicationID = A.ApplicationID
            JOIN People P ON A.ApplicantPersonID = P.PersonID
            JOIN LicenseClasses LC ON LDLA.LicenseClassID = LC.LicenseClassID
            ) AS LDLApplications
            WHERE [{FilterColumn}] LIKE @FilterValue";

            SqlCommand command = new SqlCommand(query, connection);

            switch (FilterColumn)
            {
                case "L.D.L.AppID":
                case "National No.":
                    command.Parameters.AddWithValue("@FilterValue", FilterValue);
                    break;
                default:
                    command.Parameters.AddWithValue("@FilterValue", "%" + FilterValue + "%");
                    break;
            }

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT 
                        LDLA.LocalDrivingLicenseApplicationID AS [L.D.L.AppID],
                        LDLA.ApplicationID AS [ApplicationID],
                        LC.ClassName AS [Driving Class],
                        P.NationalNo AS [National No.],
                        (P.FirstName + ' ' + P.SecondName + ' ' + ISNULL(P.ThirdName + ' ', '') + P.LastName) AS [Full Name],
                        A.ApplicationDate AS [Application Date],
                        (
                        SELECT COUNT(*) 
                        FROM TestAppointments TA
                        JOIN Tests T ON T.TestAppointmentID = TA.TestAppointmentID
                        WHERE TA.LocalDrivingLicenseApplicationID = LDLA.LocalDrivingLicenseApplicationID
                            AND T.TestResult = 1
                        ) AS [Passed Tests],
                        CASE A.ApplicationStatus
                            WHEN 1 THEN 'New'
                            WHEN 2 THEN 'Cancelled'
                            WHEN 3 THEN 'Completed'
                            ELSE 'Unknown'
                        END AS [Status]                        
                        FROM LocalDrivingLicenseApplications LDLA
                        JOIN Applications A ON LDLA.ApplicationID = A.ApplicationID
                        JOIN People P ON A.ApplicantPersonID = P.PersonID
                        JOIN LicenseClasses LC ON LDLA.LicenseClassID = LC.LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }
    }
}
