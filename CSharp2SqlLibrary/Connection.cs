using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CSharp2SqlLibrary {

    public class Connection {

        public SqlConnection sqlConnection { get; set; } = null;

        public void Open() {
            this.sqlConnection.Open();
            if(this.sqlConnection.State != System.Data.ConnectionState.Open) {
                throw new Exception("Connection did not open!");
            }
        }

        public void Close() {
            if(this.sqlConnection.State != System.Data.ConnectionState.Open) {
                return;
            }
            this.sqlConnection.Close();
        }

        public Connection(string server, string database) {
            var connStr = $"server={server};database={database};trusted_connection=true;";
            this.sqlConnection = new SqlConnection(connStr);
        }
    }
}
