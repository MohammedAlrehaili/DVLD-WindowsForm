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

        public static DataTable GetTestAppointmentsByLDLAppID(int LocalDrivingLicenseApplicationID)
        {
            DataTable dt = new DataTable();

            string query = "SELECT * FROM TestAppointments WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using(SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

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