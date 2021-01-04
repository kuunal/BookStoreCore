using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryLayer.Implementation
{
    public class DBContext: IDBContext
    {
        string connectionString;
        public DBContext(DatabaseConfigurations connectionString)
        {
            this.connectionString = connectionString.ConnectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
