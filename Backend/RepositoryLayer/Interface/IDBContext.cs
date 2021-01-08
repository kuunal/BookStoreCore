using RepositoryLayer.Implementation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IDBContext
    {
        SqlConnection GetConnection();
    }
}
