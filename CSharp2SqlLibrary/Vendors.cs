using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CSharp2SqlLibrary {

    public class Vendors {

        public static Connection Connection { get; set; }

        #region SQL Statements
        private const string SqlGetAll = "SELECT * From Vendors ";
        private const string SqlGetByPk = SqlGetAll + " Where Id = @Id";
        private const string SqlDelete = "DELETE From Vendors Where Id = @Id";
        private const string SqlUpdate = "UPDATE Vendors Set " +
            " Code = @Code, Name = @Name, Address = @Address, City = @City, State = @State, Zip = @Zip, " +
            " Phone = @Phone, Email = @Email " +
            " Where Id = @Id ";
        private const string SqlInsert = "INSERT into Vendors " +
            " (Code, Name, Address, City, State, Zip, Phone, Email) " +
            " Output Inserted.Id " +
            " VALUES (@Code, @Name, @Address, @City, @State, @Zip, @Phone, @Email) ";
        #endregion

        public static bool Update(Vendors vendor) {
            var sqlcmd = new SqlCommand(SqlUpdate, Connection.sqlConnection);
            SetParameterValues(vendor, sqlcmd);
            sqlcmd.Parameters.AddWithValue("@Id", vendor.Id);
            var rowsAffected = sqlcmd.ExecuteNonQuery();
            return rowsAffected == 1;
        }

        public static bool Insert(Vendors vendor) {
            var sqlcmd = new SqlCommand(SqlInsert, Connection.sqlConnection);
            SetParameterValues(vendor, sqlcmd);
            var rowsAffected = sqlcmd.ExecuteNonQuery();
            return rowsAffected == 1;
        }

        private static void SetParameterValues(Vendors vendor, SqlCommand sqlcmd) {
            sqlcmd.Parameters.AddWithValue("@Code", vendor.Code);
            sqlcmd.Parameters.AddWithValue("@Name", vendor.Name);
            sqlcmd.Parameters.AddWithValue("@Address", vendor.Address);
            sqlcmd.Parameters.AddWithValue("@City", vendor.City);
            sqlcmd.Parameters.AddWithValue("@State", vendor.State);
            sqlcmd.Parameters.AddWithValue("@Zip", vendor.Zip);
            sqlcmd.Parameters.AddWithValue("@Phone", (object)vendor.Phone ?? DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@Email", (object)vendor.Email ?? DBNull.Value);
        }

        public static bool Delete(int id) {
            var sql = SqlDelete;
            var sqlcmd = new SqlCommand(sql, Connection.sqlConnection);
            sqlcmd.Parameters.AddWithValue("@Id", id);
            var rowsAffected = sqlcmd.ExecuteNonQuery();
            return rowsAffected == 1;
        }

        public static Vendors GetByPk(int id) {
            var sqlcmd = new SqlCommand(SqlGetByPk, Connection.sqlConnection);
            sqlcmd.Parameters.AddWithValue("@Id", id);
            var reader = sqlcmd.ExecuteReader();
            if(!reader.HasRows) {
                reader.Close();
                return null;
            }
            reader.Read();
            var vendor = new Vendors();
            LoadVendorFromSql(vendor, reader);

            reader.Close();
            return vendor;
        }

        public static List<Vendors> GetAll() {
            var sqlcmd = new SqlCommand(SqlGetAll, Connection.sqlConnection);
            var reader = sqlcmd.ExecuteReader();
            var vendors = new List<Vendors>();
            while(reader.Read()) {
                var vendor = new Vendors();
                vendors.Add(vendor);
                LoadVendorFromSql(vendor, reader);
            }
            reader.Close();
            return vendors;
        }

        private static void LoadVendorFromSql(Vendors vendor, SqlDataReader reader) {
            vendor.Id = (int)reader["Id"];
            vendor.Code = reader["Code"].ToString();
            vendor.Name = reader["Name"].ToString();
            vendor.Address = reader["Address"].ToString();
            vendor.City = reader["City"].ToString();
            vendor.State = reader["State"].ToString();
            vendor.Zip = reader["Zip"].ToString();
            vendor.Phone = reader["Phone"]?.ToString();
            vendor.Email = reader["Email"]?.ToString();
        }

        #region Instance Properties
        public int Id { get; private set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        #endregion

        public override string ToString() {
            return $"Id={Id}, Code={Code}, Name={Name}";
        }
    }
}
