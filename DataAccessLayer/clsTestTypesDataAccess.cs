using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataAccessLayer
{
    public class clsTestTypesDataAccess
    {

        public static bool UpdateTestType(int TestTypeID, string TestTypeTitle, string TestTypeDescription, short TestTypeFees)
        {
            int rowsAffected = 0;

            string query = "UPDATE TestTypes SET TestTypeTitle = @TestTypeTitle, TestTypeDescription = @TestTypeDescription, TestTypeFees = @TestTypeFees WHERE TestTypeID = @TestTypeID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
                command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
                command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);

                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }

            return rowsAffected > 0;
        }

        public static bool GetTestTypeByID(int TestTypeID, ref string TestTypeTitle, ref string TestTypeDescription, ref short TestTypeFees)
        {
            bool isFound = false;

            string query = "SELECT * FROM TestTypes WHERE TestTypeID = @TestTypeID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;
                        TestTypeTitle = reader["TestTypeTitle"].ToString();
                        TestTypeDescription = reader["TestTypeDescription"].ToString();
                        TestTypeFees = Convert.ToInt16(reader["TestTypeFees"]);
                    }
                }
            }
            return isFound;
        }

        public static DataTable GetTestTypes()
        {
            DataTable table = new DataTable();

            string query = "SELECT * FROM TestTypes";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        table.Load(reader);
                    }
                }
                return table;
            }
        }
    }
}