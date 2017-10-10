using Dapper;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Dapper.Mappers;
using System;
using System.Threading.Tasks;

namespace IdentityServer3.Dapper
{
    public class TokenHandleStore : BaseTokenStore<Token>, ITokenHandleStore
    {
        public TokenHandleStore(DapperServiceOptions options, IScopeStore scopeStore, IClientStore clientStore)
            : base(options, Entities.TokenType.TokenHandle, scopeStore, clientStore)
        { }

        public override async Task StoreAsync(string key, Token value)
        {
            var efToken = new Entities.Token
            {
                Key = key,
                SubjectId = value.SubjectId,
                ClientId = value.ClientId,
                JsonCode = ConvertToJson(value),
                Expiry = DateTimeOffset.UtcNow.AddSeconds(value.Lifetime),
                TokenType = this.tokenType
            };

            var sql = options.SqlGenerator.Insert(new TokenMapper(options));
            await options.Connection.ExecuteAsync(sql, efToken);
        }
    }
}
