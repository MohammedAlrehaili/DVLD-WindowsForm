using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsCountryDataAccess
    {

        public static string GetCountryNameByID(int CountryID)
        {
            string countryName = "";

            string query = "SELECT CountryName FROM Countries WHERE CountryID = @CountryID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CountryID", CountryID);

                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    countryName = result.ToString();
                }

            }
            return countryName;
        }

        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();

            string query = "SELECT CountryID, CountryName FROM Countries";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
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
    }
}
