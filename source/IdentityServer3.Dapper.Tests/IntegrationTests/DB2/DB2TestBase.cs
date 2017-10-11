using Dapper;
using IBM.Data.DB2;
using System.Collections.Generic;
using System.IO;

namespace IdentityServer3.Dapper.Tests.IntegrationTests.DB2
{
    public class DB2TestBase
    {
        protected DapperServiceOptions options;
        protected DbType dbType = DbType.DB2;

        protected DB2TestBase()
        {
            Setup();
        }

        public virtual void Setup()
        {

            options = new DapperServiceOptions(
                new DB2Connection("Server=localhost;Database=test;UID=db2user;PWD=db2pwd;CurrentSchema=db2admin;"), dbType);

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
