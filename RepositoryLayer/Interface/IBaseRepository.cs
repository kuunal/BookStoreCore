using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryLayer.Interface
{
    interface IBaseRepository
    {
        public SqlConnection GetConnection();
    }
}
