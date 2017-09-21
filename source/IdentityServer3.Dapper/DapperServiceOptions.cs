#region Usings

using System;
using System.Data;

#endregion

namespace IdentityServer3.Dapper
{
    public class DapperServiceOptions
    {
        public DapperServiceOptions(IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            this.Connection = connection;
        }

        public IDbConnection Connection { get; set; }

        public string Schema { get; set; }

        public bool SynchronousReads { get; set; }
    }
}
