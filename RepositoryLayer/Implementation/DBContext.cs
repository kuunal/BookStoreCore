using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryLayer.Implementation
{
    public class DBContext: IDBContext
    {

        public SqlConnection GetConnection(DatabaseConfigurations connectionString)
        {
            return new SqlConnection(connectionString.ConnectionString);
        }
    }
}
