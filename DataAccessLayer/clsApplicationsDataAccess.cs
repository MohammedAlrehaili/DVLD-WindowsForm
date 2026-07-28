using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsApplicationsDataAccess
    {

        // DataAccessLayer/clsApplicationsDataAccess.cs

        public static bool GetApplicationByID(int ApplicationID, ref int ApplicantPersonID, ref DateTime ApplicationDate,
            ref int ApplicationTypeID, ref byte ApplicationStatus, ref DateTime LastStatusDate,
            ref decimal PaidFees, ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = "SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;
                        ApplicantPersonID = Convert.ToInt32(reader["ApplicantPersonID"]);
                        ApplicationDate = Convert.ToDateTime(reader["ApplicationDate"]);
                        ApplicationTypeID = Convert.ToInt32(reader["ApplicationTypeID"]);
                        ApplicationStatus = Convert.ToByte(reader["ApplicationStatus"]);
                        LastStatusDate = Convert.ToDateTime(reader["LastStatusDate"]);
                        PaidFees = Convert.ToDecimal(reader["PaidFees"]);
                        CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    }
                }
            }
            return isFound;
        }

        public static bool UpdateApplicationStatus(int ApplicationID, byte NewStatus)
        {
            int rowsAffected = 0;

            string query = @"UPDATE Applications 
                    SET ApplicationStatus = @ApplicationStatus, LastStatusDate = @LastStatusDate
                    WHERE ApplicationID = @ApplicationID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@ApplicationStatus", NewStatus);
                command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            return (rowsAffected > 0);
        }

        public static int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
            byte ApplicationStatus, DateTime LastStatusDate, decimal PaidFees, int CreatedByUserID)
        {
            int ApplicationID = -1;

            string query = @"INSERT INTO Applications (ApplicantPersonID, ApplicationDate, ApplicationTypeID,
                    ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID)
                    VALUES (@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID,
                    @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID);
                    SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    ApplicationID = insertedID;
                }
            }
            return ApplicationID;
        }
    }
}