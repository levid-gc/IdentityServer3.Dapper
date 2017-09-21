#region Usings

using Dapper;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Dapper.Entities;
using System;
using System.Threading.Tasks;

#endregion

namespace IdentityServer3.Dapper
{
    public class AuthorizationCodeStore : BaseTokenStore<AuthorizationCode>, IAuthorizationCodeStore
    {
        public AuthorizationCodeStore(DapperServiceOptions options, IScopeStore scopeStore, IClientStore clientStore)
            : base(options, TokenType.AuthorizationCode, scopeStore, clientStore)
        { }

        public override async Task StoreAsync(string key, AuthorizationCode code)
        {
            var efCode = new Entities.Token
            {
                Key = key,
                SubjectId = code.SubjectId,
                ClientId = code.ClientId,
                JsonCode = ConvertToJson(code),
                Expiry = DateTimeOffset.UtcNow.AddSeconds(code.Client.AuthorizationCodeLifetime),
                TokenType = this.tokenType
            };

            var sql = @"INSERT INTO SACCORE.T_TOKEN VALUES(@Key, @TokenType, @SubjectId, @ClientId, @JsonCode, @Expiry)";

            await this.options.Connection.ExecuteAsync(sql, efCode);
        }
    }
}
