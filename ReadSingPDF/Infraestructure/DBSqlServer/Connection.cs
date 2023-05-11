using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadSingPDF.Infraestructure.DBSqlServer
{
    public class Connection : IDisposable
    {
        private SqlConnection sqlConnection;
        private const string connectionString = "Data Source=LAPTOP-RT5CO4QQ\\SQL2014;Initial Catalog=SignPDF;Persist Security Info=True;User ID=sa;Password=sa";

        public void Dispose()
        {
            sqlConnection = null;
        }

        public SqlConnection GetConnection()
        {
            if (sqlConnection == null)
            {
                sqlConnection = new SqlConnection(GetConnectionString());
            }

            return sqlConnection;
        }

        string GetConnectionString()
        {
            return connectionString;
        }

        public void Open()
        {
            if (sqlConnection == null) throw new Exception("Fault open connection.");
            sqlConnection.Open();
        }

        public void Close()
        {
            if (sqlConnection == null) throw new Exception("Fault close connection.");
            sqlConnection.Close();
            Dispose();
        }
    }
}
