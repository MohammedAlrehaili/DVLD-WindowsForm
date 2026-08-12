using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsTestAppointmentsDataAccess
    {

        public static bool IsAppointmentExists(int TestTypeID, int LocalDrivingLicenseApplicationID)
        {
            bool exists = false;

            string query = "SELECT COUNT(*) FROM TestAppointments WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID AND TestTypeID = @TestTypeID";
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                connection.Open();
                int count = (int)command.ExecuteScalar();
                exists = count > 0;
            }
            return exists;
        }

        public static DataTable GetTestAppointmentsByLDLAppID(int LocalDrivingLicenseApplicationID)
        {
            DataTable dt = new DataTable();

            string query = "SELECT * FROM TestAppointments WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using(SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                connection.Open();

                using(SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }

        public static bool GetTestAppointmentByID(int TestAppointmentID, ref int testTypeID,
        ref int localDrivingLicenseApplicationID, ref DateTime appointmentDate, ref short paidFees, ref bool isLocked, ref int createdByUserID)
        {
            bool isFound = false;

            string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;
                        localDrivingLicenseApplicationID = Convert.ToInt32(reader["LocalDrivingLicenseApplicationID"]);
                        testTypeID = Convert.ToInt32(reader["TestTypeID"]);
                        appointmentDate = Convert.ToDateTime(reader["AppointmentDate"]);
                        paidFees = Convert.ToInt16(reader["PaidFees"]);
                        isLocked = Convert.ToBoolean(reader["IsLocked"]);
                        createdByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    }
                }
            }
            return isFound;
        }

        public static bool UpdateTestAppointments(int TestAppointmentID, DateTime AppointmentDate)
        {
            int rowsAffected = 0;

            string query = "UPDATE TestAppointments SET AppointmentDate = @AppointmentDate WHERE TestAppointmentID = @TestAppointmentID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);

                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            return rowsAffected > 0;

        }

        public static int AddTestAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, short PaidFees, int CreatedByUserID, bool isLocked)
        {
            int TestAppointmentID = -1;

            string query = @"INSERT INTO TestAppointments (TestTypeID,LocalDrivingLicenseApplicationID,
                AppointmentDate, PaidFees, CreatedByUserID, IsLocked)
                VALUES (@TestTypeID,@LocalDrivingLicenseApplicationID,
                @AppointmentDate, @PaidFees, @CreatedByUserID, @IsLocked);
                SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                command.Parameters.AddWithValue("@IsLocked", isLocked);

                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestAppointmentID = insertedID;
                }
            }
            return TestAppointmentID;
        }
    }
}