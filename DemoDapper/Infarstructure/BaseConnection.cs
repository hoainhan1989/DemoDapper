using System.Data;
using System.Data.SqlClient;

namespace DemoDapper.Infarstructure
{
    public static class BaseConnection
    {
        public static IDbConnection CreateConnection()
        {
            return new SqlConnection(ConnectionInfo.ConnectionString);
        }
    }
}
