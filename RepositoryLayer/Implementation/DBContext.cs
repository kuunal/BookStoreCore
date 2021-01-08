using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryLayer.Implementation
{
    public class DBContext: IDBContext
    {
        readonly string connectionString;
        public DBContext(DatabaseConfigurations connectionString)
        {
            this.connectionString = connectionString.ConnectionString;
        }

        /// <summary>
        /// Gets the connection to database.
        /// </summary>
        /// <returns> </returns>
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
