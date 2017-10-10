using DapperExtensions;
using DapperExtensions.Sql;
using System;
using System.Data;

namespace IdentityServer3.Dapper
{
    public enum DbType
    {
        SQLServer,
        Oracle,
        DB2,
        MySQL
    }

    public class TableNames
    {
        private string _client;
        private string _clientClaims;
        private string _clientCorsOrigins;
        private string _clientCustomGrantTypes;
        private string _clientIdPRestrictions;
        private string _clientPostLogoutRedirectUris;
        private string _clientRedirectUris;
        private string _clientScopes;
        private string _clientSecrets;

        private string _scope;
        private string _scopeClaims;
        private string _scopeSecrets;

        private string _token;
        private string _consent;

        public string Client
        {
            get { return String.IsNullOrEmpty(this._client) ? "Clients" : this._client; }
            set { this._client = value.Trim(); }
        }
        public string ClientClaims
        {
            get { return String.IsNullOrEmpty(this._clientClaims) ? "ClientClaims" : this._clientClaims; }
            set { this._clientClaims = value.Trim(); }
        }
        public string ClientCorsOrigins
        {
            get { return String.IsNullOrEmpty(this._clientCorsOrigins) ? "ClientCorsOrigins" : this._client; }
            set { this._clientCorsOrigins = value.Trim(); }
        }
        public string ClientCustomGrantTypes
        {
            get { return String.IsNullOrEmpty(this._clientCustomGrantTypes) ? "ClientCustomGrantTypes" : this._client; }
            set { this._clientCustomGrantTypes = value.Trim(); }
        }
        public string ClientIdPRestrictions
        {
            get { return String.IsNullOrEmpty(this._clientIdPRestrictions) ? "ClientIdPRestrictions" : this._client; }
            set { this._clientIdPRestrictions = value.Trim(); }
        }
        public string ClientPostLogoutRedirectUris
        {
            get { return String.IsNullOrEmpty(this._clientPostLogoutRedirectUris) ? "ClientPostLogoutRedirectUris" : this._client; }
            set { this._clientPostLogoutRedirectUris = value.Trim(); }
        }
        public string ClientRedirectUris
        {
            get { return String.IsNullOrEmpty(this._clientRedirectUris) ? "ClientRedirectUris" : this._client; }
            set { this._clientRedirectUris = value.Trim(); }
        }
        public string ClientScopes
        {
            get { return String.IsNullOrEmpty(this._clientScopes) ? "ClientScopes" : this._client; }
            set { this._clientScopes = value.Trim(); }
        }
        public string ClientSecrets
        {
            get { return String.IsNullOrEmpty(this._clientSecrets) ? "ClientSecrets" : this._client; }
            set { this._clientSecrets = value.Trim(); }
        }

        public string Scope
        {
            get { return String.IsNullOrEmpty(this._scope) ? "Scopes" : this._scope; }
            set { this._scope = value.Trim(); }
        }
        public string ScopeClaims
        {
            get { return String.IsNullOrEmpty(this._scopeClaims) ? "ScopeClaims" : this._scopeClaims; }
            set { this._scopeClaims = value.Trim(); }
        }
        public string ScopeSecrets
        {
            get { return String.IsNullOrEmpty(this._scopeSecrets) ? "ScopeSecrets" : this._scopeSecrets; }
            set { this._scopeSecrets = value.Trim(); }
        }

        public string Token
        {
            get { return String.IsNullOrEmpty(this._token) ? "Tokens" : this._scopeSecrets; }
            set { this._token = value.Trim(); }
        }

        public string Consent
        {
            get { return String.IsNullOrEmpty(this._consent) ? "Consents" : this._consent; }
            set { this._consent = value.Trim(); }
        }
    }

    public class DapperServiceOptions
    {
        private string _schemaName;
        private TableNames _tableNames;

        public DapperServiceOptions(IDbConnection connection, DbType dbType, string schemaName = null, TableNames tableNames = null)
        {
            Connection = connection ?? throw new ArgumentNullException("connection");

            SchemaName = schemaName;
            TableNames = tableNames;

            Init(dbType);
        }

        public IDbConnection Connection { get; private set; }

        public IDapperExtensionsConfiguration Config { get; private set; }

        public ISqlGenerator SqlGenerator { get; private set; }

        public IDatabase Db { get; private set; }

        public string SchemaName
        {
            get { return this._schemaName; }
            private set { this._schemaName = value; }
        }

        public TableNames TableNames
        {
            get { return this._tableNames ?? new TableNames(); }
            private set { this._tableNames = value; }
        }

        public bool SynchronousReads { get; set; }

        private void Init(DbType dbType)
        {
            ISqlDialect dialect = null;

            switch (dbType)
            {
                case DbType.SQLServer:
                    dialect = new SqlServerDialect();
                    break;
                case DbType.Oracle:
                    dialect = new OracleDialect();
                    break;
                case DbType.DB2:
                    dialect = new DB2Dialect();
                    break;
                case DbType.MySQL:
                    dialect = new MySqlDialect();
                    break;
                default:
                    throw new NotSupportedException("dbType");
            }

            Config = new IdentityServerDapperExtensionsConfiguration(this, dialect);
            SqlGenerator = new SqlGeneratorImpl(Config);
            Db = new Database(Connection, SqlGenerator);
        }
    }
}
