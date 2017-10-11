using DapperExtensions.Sql;

namespace IdentityServer3.Dapper.Sql
{
    public class DB2DialectCustom : DB2Dialect
    {
        // if we use the default '"' character for OpenQuote and CloseQuote, it can results in errors,
        // because properties of our entities and column names of tables does not match exactly,
        // therefore in our project, we left the OpenQuote and CloseQuote blank.

        public override char OpenQuote
        {
            get { return ' '; }
        }

        public override char CloseQuote
        {
            get { return ' '; }
        }
    }
}
