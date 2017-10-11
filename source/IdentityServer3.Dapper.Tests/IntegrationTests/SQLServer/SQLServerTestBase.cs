using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace IdentityServer3.Dapper.Tests.IntegrationTests.SQLServer
{
    public class SQLServerTestBase
    {
        protected DapperServiceOptions options;
        protected DbType dbType = DbType.SQLServer;
        
        protected SQLServerTestBase()
        {
            Setup();
        }

        public virtual void Setup()
        {
            
            options = new DapperServiceOptions(
                new SqlConnection("Data Source=.;Initial Catalog=IdentityServer3;Integrated security=True;"), dbType);

            var files = new List<string>
                                {
                                    ReadScriptFile("clients"),
                                    ReadScriptFile("scopes"),
                                    ReadScriptFile("operational")
                                };

            foreach (var setupFile in files)
            {
                options.Connection.Execute(setupFile);
            }
        }

        public string ReadScriptFile(string name)
        {
            string fileName = GetType().Namespace + ".Sql." + name + ".sql";
            using (Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName))
            using (StreamReader sr = new StreamReader(s))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
