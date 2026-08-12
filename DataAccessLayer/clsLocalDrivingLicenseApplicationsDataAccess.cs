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

        public static int GetPassedTestsCount(int LocalDrivingLicenseApplicationID)
        {
            int count = 0;

            string query = @"SELECT COUNT(*) 
                    FROM TestAppointments TA
                    JOIN Tests T ON T.TestAppointmentID = TA.TestAppointmentID
                    WHERE TA.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                      AND T.TestResult = 1";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                connection.Open();
                count = (int)command.ExecuteScalar();

            }
            return count;
        }

        public static bool GetLocalDrivingLicenseApplicationByID(int LocalDrivingLicenseApplicationID, ref int ApplicationID, ref int LicenseClassID)
        {
            bool isFound = false;

            string query = "SELECT * FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;
                        ApplicationID = Convert.ToInt32(reader["ApplicationID"]);
                        LicenseClassID = Convert.ToInt32(reader["LicenseClassID"]);
                    }
                }
            }
            return isFound;
        }

        public static bool DoesPersonHaveActiveApplicationForClass(int ApplicantPersonID, int LicenseClassID)
        {
            bool found = false;

            string query = @"SELECT COUNT(*)
                    FROM LocalDrivingLicenseApplications LDLA
                    JOIN Applications A ON LDLA.ApplicationID = A.ApplicationID
                    WHERE A.ApplicantPersonID = @ApplicantPersonID
                      AND LDLA.LicenseClassID = @LicenseClassID
                      AND A.ApplicationStatus <> 2";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                found = (count > 0);

            }
            return found;
        }

        public static int AddNewLocalDrivingLicenseApplication(int ApplicationID, int LicenseClassID)
        {
            int LocalDrivingLicenseApplicationID = -1;

            string query = @"INSERT INTO LocalDrivingLicenseApplications (ApplicationID, LicenseClassID)
                    VALUES (@ApplicationID, @LicenseClassID);
                    SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LocalDrivingLicenseApplicationID = insertedID;
                }

            }
            return LocalDrivingLicenseApplicationID;
        }

        public static DataTable GetLocalDrivingLicenseApplicationsByFilter(string FilterColumn, string FilterValue)
        {
            DataTable dt = new DataTable();

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

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
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

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if(reader.HasRows)
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable dt = new DataTable();

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

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }

        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID)
        {
            string query1 = "DELETE FROM TestAppointments WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            string query2 = "DELETE FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            string query3 = "DELETE FROM Applications WHERE ApplicationID = @ApplicationID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    using (SqlCommand cmd1 = new SqlCommand(query1, connection, transaction))
                    {
                        cmd1.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        cmd1.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd2 = new SqlCommand(query2, connection, transaction))
                    {
                        cmd2.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        cmd2.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd3 = new SqlCommand(query3, connection, transaction))
                    {
                        cmd3.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        cmd3.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}
