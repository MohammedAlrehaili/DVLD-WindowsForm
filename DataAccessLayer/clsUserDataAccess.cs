using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataAccessLayer
{
    public class clsUserDataAccess
    {

        public static bool DeleteUser(int UserID)
        {
            int rowsAffected = 0;

            string query = "DELETE FROM Users WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", UserID);

                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            return rowsAffected > 0;
        }

        public static bool FindByUserID(int UserID, ref int PersonID, ref string UserName, ref string Password, ref bool IsActive)
        {
            bool isFound = false;

            string query = "SELECT * FROM Users WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))


            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", UserID);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;
                        PersonID = Convert.ToInt32(reader["PersonID"]);
                        UserName = reader["UserName"].ToString();
                        Password = reader["Password"].ToString();
                        IsActive = Convert.ToBoolean(reader["IsActive"]);
                    }
                }
            }
            return isFound;
        }

        public static bool UpdatePassword(int UserID, string NewPassword)
        {
            int rowsAffected = 0;

            string query = @"UPDATE Users SET Password = @Password WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", UserID);
                command.Parameters.AddWithValue("@Password", clsDataAccessSettings.HashPassword(NewPassword));

                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }

            return rowsAffected > 0;
        }

        public static bool UpdateUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            int rowsAffected = 0;


            string query = @"UPDATE Users SET PersonID = @PersonID, UserName = @UserName, 
                     Password = @Password, IsActive = @IsActive
                     WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", UserID);
                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters.AddWithValue("@Password", Password);
                command.Parameters.AddWithValue("@IsActive", IsActive);

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            return (rowsAffected > 0);
        }

        public static DataTable GetUsersByFilter(string filterColumn, string filterValue)
        {
            DataTable dt = new DataTable();

            string query = $@"SELECT * FROM (
                            SELECT u.UserID, u.PersonID,
                                   (p.FirstName + ' ' + p.SecondName + ' ' + 
                                    ISNULL(p.ThirdName + ' ', '') + p.LastName) AS FullName,
                                   u.UserName, u.Password, u.IsActive
                            FROM Users u
                            JOIN People p ON u.PersonID = p.PersonID
                       ) AS UsersWithNames
                       WHERE {filterColumn} LIKE @FilterValue";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                switch (filterColumn)
                {
                    case "UserID":
                    case "PersonID":
                    case "IsActive":
                        command.Parameters.AddWithValue("@FilterValue", filterValue);
                        break;
                    default:
                        command.Parameters.AddWithValue("@FilterValue", "%" + filterValue + "%");
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

        public static bool IsPersonIDExisit(int PersonID)
        {
            bool isFound = false;

            string query = "SELECT COUNT(*) FROM Users WHERE PersonID = @PersonID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))


            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", PersonID);

                connection.Open();

                int count = (int)command.ExecuteScalar();
                isFound = count > 0;
            }
            return isFound;
        }

        public static int AddUser(int PersonID, string UserName, string Password, bool isActive)
        {
            int UserID = -1;

            string query = @"INSERT INTO Users (PersonID, UserName, Password, IsActive) VALUES
                            (@PersonID, @UserName, @Password, @isActive);
                            SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))


            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters.AddWithValue("@Password", clsDataAccessSettings.HashPassword(Password));
                command.Parameters.AddWithValue("@isActive", isActive);

                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    UserID = insertedID;
                }
            }
            return UserID;

        }

        public static DataTable GetUsers()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT u.UserID, u.PersonID,
                            (p.FirstName + ' ' + p.SecondName + ' ' + 
                             ISNULL(p.ThirdName + ' ', '') + p.LastName) AS FullName,
                            u.UserName, u.Password, u.IsActive
                     FROM Users u
                     JOIN People p ON u.PersonID = p.PersonID";

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
                    reader.Close();
                }
                return dt;
            }
        }


        public static bool GetUserByUsernameAndPassword(string username, string password, ref int UserID, ref int PersonID, ref bool isActive)
        {
            bool isFound = false;

            string query = "SELECT * FROM Users WHERE UserName = @Username AND Password = @Password";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {

                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", clsDataAccessSettings.HashPassword(password));

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;
                        UserID = Convert.ToInt32(reader["UserID"]);
                        PersonID = Convert.ToInt32(reader["PersonID"]);
                        isActive = Convert.ToBoolean(reader["IsActive"]);
                    }
                }
            }
            return isFound;
        }
    }
}
