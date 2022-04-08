using System;
using System.Collections.Generic;
using System.Text;

namespace DemoDapper.Infarstructure
{
    public static class ConnectionInfo
    {
        public static string ConnectionString { get {
                return "Data Source=localhost;Initial Catalog=DemoDapper;User ID=xxxx;Password=xxx;Integrated Security=SSPI;";
            } }
    }
}
