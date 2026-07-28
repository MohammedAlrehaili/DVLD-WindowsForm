using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsLicenseClassesDataAccess
    {

        public static bool GetLicenseClassesByID(int LicenseClassID, ref string ClassName, ref string ClassDescription, ref short MinimumAllowedAge, ref short DefaultValidityLength, ref short ClassFees)
        {
            bool isFound = false;

            string query = "SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        isFound = true;
                        ClassName = reader["ClassName"].ToString();
                        ClassDescription = reader["ClassDescription"].ToString();
                        MinimumAllowedAge = Convert.ToInt16(reader["MinimumAllowedAge"]);
                        DefaultValidityLength = Convert.ToInt16(reader["DefaultValidityLength"]);
                        ClassFees = Convert.ToInt16(reader["ClassFees"]);
                    }
                }
            }
            return isFound;
        }

        public static DataTable GetLicenseClasses()
        {
            DataTable dt = new DataTable();

            string query = "SELECT LicenseClassID, ClassName FROM LicenseClasses";

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
