using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace IdentityServer3.Dapper.Tests.IntegrationTests.SQLServer
{
    public class SQLServerTestBase
    {
        protected IDatabase Db;

        protected SQLServerTestBase()
        {
            Setup();
        }

        public virtual void Setup()
        {
            var connection = new SqlConnection("Data Source=.;Initial Catalog=dapperTest;Integrated security=True;");
            var config = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new SqlServerDialect());
            var sqlGenerator = new SqlGeneratorImpl(config);
            Db = new Database(connection, sqlGenerator);
            var files = new List<string>
                                {
                                    ReadScriptFile("clients"),
                                    ReadScriptFile("scopes"),
                                    ReadScriptFile("operational")
                                };

            foreach (var setupFile in files)
            {
                connection.Execute(setupFile);
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
