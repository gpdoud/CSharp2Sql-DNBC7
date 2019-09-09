using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CSharp2SqlLibrary {

    public class Products {

        public static Connection Connection { get; set; }
        #region SQL Statements
        private const string SqlGetAll = "SELECT * from Products ";
        private const string SqlGetByPk = "SELECT * from Products Where Id = @Id ";
        private const string SqlGetByPartNbr = SqlGetAll + " Where PartNbr = @PartNbr ";
        private const string SqlDelete = "DELETE from Products Where Id = @Id ";
        private const string SqlInsert = "INSERT Products " +
            " (PartNbr, Name, Price, Unit, PhotoPath, VendorId) " +
            " VALUES (@PartNbr, @Name, @Price, @Unit, @PhotoPath, @VendorId) ";
        private const string SqlUpdate = "UPDATE Products Set " +
            " PartNbr = @PartNbr, Name = @Name, Price = @Price, Unit = @Unit, " +
            " PhotoPath = @PhotoPath, VendorId = @VendorId " +
            " Where Id = @Id ";
        #endregion

        public static Products GetByPartNbr(string partNbr) {
            var sqlcmd = new SqlCommand(SqlGetByPartNbr, Connection.sqlConnection);
            sqlcmd.Parameters.AddWithValue("@PartNbr", partNbr);
            var reader = sqlcmd.ExecuteReader();
            if(!reader.HasRows) {
                reader.Close();
                return null;
            }
            reader.Read();
            var product = new Products();
            LoadProductFromSql(product, reader);
            reader.Close();
            return product;
        }

        public static bool Insert(Products product) {
            var sqlcmd = new SqlCommand(SqlInsert, Connection.sqlConnection);
            SetParameterValues(product, sqlcmd);
            var rowsAffected = sqlcmd.ExecuteNonQuery();
            return rowsAffected == 1;
        }

        public static bool Update(Products product) {
            var sqlcmd = new SqlCommand(SqlUpdate, Connection.sqlConnection);
            SetParameterValues(product, sqlcmd);
            sqlcmd.Parameters.AddWithValue("@Id", product.Id);
            var rowsAffected = sqlcmd.ExecuteNonQuery();
            return rowsAffected == 1;
        }

        public static bool Delete(int id) {
            var sqlcmd = new SqlCommand(SqlDelete, Connection.sqlConnection);
            sqlcmd.Parameters.AddWithValue("@Id", id);
            var rowsAffected = sqlcmd.ExecuteNonQuery();
            return rowsAffected == 1;
        }

        public static bool Delete(Products product) {
            return Delete(product.Id);
        }
        public static Products GetByPk(int id) {
            var sqlcmd = new SqlCommand(SqlGetByPk, Connection.sqlConnection);
            sqlcmd.Parameters.AddWithValue("@Id", id);
            var reader = sqlcmd.ExecuteReader();
            if(!reader.HasRows) {
                reader.Close();
                return null;
            }
            reader.Read();
            var product = new Products();
            LoadProductFromSql(product, reader);

            reader.Close();

            Vendors.Connection = Connection;
            var vendor = Vendors.GetByPk(product.VendorId);
            product.Vendor = vendor;

            return product;
        }
        public static List<Products> GetAll() {
            var sqlcmd = new SqlCommand(SqlGetAll, Connection.sqlConnection);
            var reader = sqlcmd.ExecuteReader();
            var products = new List<Products>();
            while(reader.Read()) {
                var product = new Products();
                products.Add(product);
                LoadProductFromSql(product, reader);
            }
            reader.Close();

            Vendors.Connection = Connection;
            foreach(var prod in products) {
                var vendor = Vendors.GetByPk(prod.VendorId);
                prod.Vendor = vendor;
            }

            return products;
        }
        private static void SetParameterValues(Products product, SqlCommand sqlcmd) {
            sqlcmd.Parameters.AddWithValue("@PartNbr", product.PartNbr);
            sqlcmd.Parameters.AddWithValue("@Name", product.Name);
            sqlcmd.Parameters.AddWithValue("@Price", product.Price);
            sqlcmd.Parameters.AddWithValue("@Unit", product.Unit);
            sqlcmd.Parameters.AddWithValue("@PhotoPath", (object)product.PhotoPath ?? DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@VendorId", product.VendorId);
        }
        public static void LoadProductFromSql(Products product, SqlDataReader reader) {
            product.Id = reader.GetInt32(reader.GetOrdinal("Id"));
            product.PartNbr = reader["PartNbr"].ToString();
            product.Name = reader["Name"].ToString();
            product.Price = (decimal)reader["Price"];
            product.Unit = reader["Unit"].ToString();
            product.PhotoPath = reader["PhotoPath"]?.ToString();
            product.VendorId = (int)reader["VendorId"];
        }

        #region Instance Properties
        public int Id { get; private set; }
        public string PartNbr { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public string PhotoPath { get; set; }
        public int VendorId { get; set; }
        public Vendors Vendor { get; private set; }
        #endregion
        public override string ToString() {
            return $"Id={Id}, PartNbr={PartNbr}, Name={Name}, Price={Price}, " +
                $"Unit={Unit}, VendorId={VendorId}, Vendor={Vendor}";
        }
    }
}
