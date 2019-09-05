using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CSharp2SqlLibrary {

    public class Users {

        public static Connection Connection { get; set; }

        public static Users Login(string username, string password) {
            var sql = "SELECT * from Users Where Username = @Username AND Password = @Password";
            var sqlcmd = new SqlCommand(sql, Connection._Connection);
            sqlcmd.Parameters.AddWithValue("@Username", username);
            sqlcmd.Parameters.AddWithValue("@Password", password);
            var reader = sqlcmd.ExecuteReader();
            if(!reader.HasRows) {
                reader.Close();
                return null;
            }
            reader.Read();
            var user = new Users();
            LoadUserFromSql(user, reader);

            reader.Close();
            return user;
        }

        public static Users GetByPk(int id) {
            var sql = "SELECT * from Users Where Id = @Id";
            var sqlcmd = new SqlCommand(sql, Connection._Connection);
            sqlcmd.Parameters.AddWithValue("@Id", id);
            var reader = sqlcmd.ExecuteReader();
            if(!reader.HasRows) {
                reader.Close();
                return null;
            }
            reader.Read();
            var user = new Users();
            LoadUserFromSql(user, reader);

            reader.Close();
            return user;
        }

        public static List<Users> GetAll() {
            var sql = "SELECT * from Users;";
            var sqlcmd = new SqlCommand(sql, Connection._Connection);
            var reader = sqlcmd.ExecuteReader();
            var users = new List<Users>();
            while(reader.Read()) {
                var user = new Users();
                users.Add(user);

                LoadUserFromSql(user, reader);
            }
            reader.Close();
            return users;
        }

        private static void LoadUserFromSql(Users user, SqlDataReader reader) {
            user.Id = (int)reader["Id"];
            user.Username = reader["Username"].ToString();
            user.Password = (string)reader["Password"];
            user.Firstname = (string)reader["Firstname"];
            user.Lastname = (string)reader["Lastname"];
            user.Phone = reader["Phone"]?.ToString();
            user.Email = reader["Email"]?.ToString();
            user.IsAdmin = (bool)reader["IsAdmin"];
            user.IsReviewer = (bool)reader["IsReviewer"];
        }

        public int Id { get; private set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsReviewer { get; set; }

        public Users() {
        }

        public override string ToString() {
            return $"Id={Id}, Username={Username}, Password={Password}, " +
                    $"Name={Firstname} {Lastname}, Admin?={IsAdmin}, Reviewer?={IsReviewer}";
        }
    }
}
