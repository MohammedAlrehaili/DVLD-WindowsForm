using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsPeopleDataAccess
    {

        public static DataTable GetPeopleByFilter(string filterColumn, string filterValue)
        {
            DataTable dt = new DataTable();

            string query = $"SELECT * FROM People WHERE {filterColumn} LIKE @FilterValue";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                switch (filterColumn)
                {
                    case "PersonID":
                    case "NationalNo":
                    case "Phone":
                        command.Parameters.AddWithValue("@FilterValue", filterValue);
                        break;
                    default:
                        command.Parameters.AddWithValue("@FilterValue", "%" + filterValue + "%");
                        break;
                }

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

        public static bool GetPersonByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName,
            ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref byte Gender, ref string Address, ref string Phone, ref string Email,
            ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            string query = "SELECT * FROM People WHERE NationalNo = @NationalNo;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@NationalNo", NationalNo);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;
                        PersonID = Convert.ToInt32(reader["PersonID"]);
                        FirstName = reader["FirstName"].ToString();
                        SecondName = reader["SecondName"].ToString();
                        ThirdName = reader["ThirdName"] != DBNull.Value ? reader["ThirdName"].ToString() : "";
                        LastName = reader["LastName"].ToString();
                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                        Gender = Convert.ToByte(reader["Gender"]);
                        Address = reader["Address"].ToString();
                        Phone = reader["Phone"].ToString();
                        Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "";
                        NationalityCountryID = Convert.ToInt32(reader["NationalityCountryID"]);
                        ImagePath = reader["ImagePath"] != DBNull.Value ? reader["ImagePath"].ToString() : "";
                    }
                }
            }
            return isFound;
        }

        public static bool GetPersonByID(int PersonID, ref string NationalNo, ref string FirstName, ref string SecondName,
            ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref byte Gender, ref string Address, ref string Phone, ref string Email,
            ref int NationalityCountryID, ref string ImagePath)
        {

            bool isFound = false;

            string query = "SELECT * FROM People WHERE PersonID = @PersonID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", PersonID);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if(reader.Read()) {
                        isFound = true;
                        NationalNo = reader["NationalNo"].ToString();
                        FirstName = reader["FirstName"].ToString();
                        SecondName = reader["SecondName"].ToString();
                        ThirdName = reader["ThirdName"] != DBNull.Value ? reader["ThirdName"].ToString() : "";
                        LastName = reader["LastName"].ToString();
                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                        Gender = Convert.ToByte(reader["Gender"]);
                        Address = reader["Address"].ToString();
                        Phone = reader["Phone"].ToString();
                        Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "";
                        NationalityCountryID = Convert.ToInt32(reader["NationalityCountryID"]);
                        ImagePath = reader["ImagePath"] != DBNull.Value ? reader["ImagePath"].ToString() : "";
                    }
                }
            }
            return isFound;
        }

        public static DataTable GetPeopleData()
        {
            DataTable dt = new DataTable();

            string query = "SELECT * FROM People";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
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

        public static bool DeletePersonByID(int PersonID)
        {
            int rowsAffected = 0;

            string query = "DELETE FROM People WHERE PersonID = @PersonID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", PersonID);

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            return (rowsAffected > 0);
        }

        public static bool UpdatePerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName,
            string LastName, DateTime DateOfBirth, byte Gender, string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)
        {
            int rowsAffected = 0;

            string query = @"UPDATE People SET FirstName = @FirstName, SecondName = @SecondName, ThirdName = @ThirdName,
                        LastName = @LastName, DateOfBirth = @DateOfBirth, Gender = @Gender, Address = @Address,
                        Phone = @Phone, Email = @Email, NationalityCountryID = @NationalityCountryID, ImagePath = @ImagePath
                        WHERE PersonID = @PersonID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@NationalNo", NationalNo);
                command.Parameters.AddWithValue("@FirstName", FirstName);
                command.Parameters.AddWithValue("@SecondName", SecondName);
                command.Parameters.AddWithValue("@ThirdName", ThirdName == "" ? (object)DBNull.Value : ThirdName);
                command.Parameters.AddWithValue("@LastName", LastName);
                command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                command.Parameters.AddWithValue("@Gender", Gender);
                command.Parameters.AddWithValue("@Address", Address);
                command.Parameters.AddWithValue("@Phone", Phone);
                command.Parameters.AddWithValue("@Email", Email == "" ? (object)DBNull.Value : Email);
                command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
                command.Parameters.AddWithValue("@ImagePath", ImagePath == "" ? (object)DBNull.Value : ImagePath);

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            return (rowsAffected > 0);
        }

        public static int AddPerson(string NationalNo, string FirstName, string SecondName, string ThirdName,
            string LastName, DateTime DateOfBirth, byte Gender, string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)
        {

            int PersonID = -1;

            string query = @"INSERT INTO People (NationalNo, FirstName, SecondName, ThirdName, LastName,
                        DateOfBirth, Gender, Address, Phone, Email,
                        NationalityCountryID, ImagePath) VALUES (@NationalNo,
                        @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth,
                        @Gender, @Address, @Phone, @Email, @NationalityCountryID, @ImagePath);
                        SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@NationalNo", NationalNo);
                command.Parameters.AddWithValue("@FirstName", FirstName);
                command.Parameters.AddWithValue("@SecondName", SecondName);
                command.Parameters.AddWithValue("@ThirdName", ThirdName == "" ? (object)DBNull.Value : ThirdName);
                command.Parameters.AddWithValue("@LastName", LastName);
                command.Parameters.AddWithValue("@Email", Email == "" ? (object)DBNull.Value : Email);
                command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                command.Parameters.AddWithValue("@Gender", Gender);
                command.Parameters.AddWithValue("@Address", Address);
                command.Parameters.AddWithValue("@Phone", Phone);
                command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
                command.Parameters.AddWithValue("@ImagePath", ImagePath == "" ? (object)DBNull.Value : ImagePath);

                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    PersonID = insertedID;
                }
            }
            return PersonID;
        }

        public static bool SearchPersonByNationalNo(string NationalNo)
        {
            bool found = false;

            string query = "SELECT COUNT(*) FROM People WHERE NationalNo = @NationalNo";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("NationalNo", NationalNo);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                found = (count > 0);

            }
            return found;
        }
    }
}