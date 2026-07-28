using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsApplicationTypesDataAccess
    {

        public static bool GetApplicationTypeByID(int ApplicationTypeID, ref string ApplicationTypeTitle, ref short ApplicationFees)
        {
            bool isFound = false;

            string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;
                        ApplicationTypeTitle = reader["ApplicationTypeTitle"].ToString();
                        ApplicationFees = Convert.ToInt16(reader["ApplicationFees"]);
                    }
                }
            }
            return isFound;
        }

        public static bool UpdateApplicationTypes(int ApplicationTypeID, string ApplicationTypeTitle, short ApplicationFees)
        {
            int rowsAffected = 0;

            string query = "UPDATE ApplicationTypes SET ApplicationTypeTitle = @ApplicationTypeTitle, ApplicationFees = @ApplicationFees WHERE ApplicationTypeID = @ApplicationTypeID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
                command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);

                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            return rowsAffected > 0;
        }

        public static DataTable GetApplicationTypes()
        {
            DataTable dt = new DataTable();

            string query = "SELECT * FROM ApplicationTypes";

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
    }
}
