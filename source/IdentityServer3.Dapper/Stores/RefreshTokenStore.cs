#region Usings

using Dapper;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Dapper.Entities;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace IdentityServer3.Dapper
{
    public class RefreshTokenStore : BaseTokenStore<RefreshToken>, IRefreshTokenStore
    { 
        public RefreshTokenStore(DapperServiceOptions options, IScopeStore scopeStore, IClientStore clientStore)
            : base(options, TokenType.RefreshToken, scopeStore, clientStore)
        { }

        public override async Task StoreAsync(string key, RefreshToken value)
        {
            Entities.Token token = null;
            bool add = false;

            var sql = @"SELECT * FROM SACCORE.T_TOKEN WHERE Key = @Key AND TokenType = @TokenType";

            if (options != null && options.SynchronousReads)
            {
                token = this.options.Connection
                    .Query<Entities.Token>(sql, new { Key = key, TokenType = (short)tokenType }).FirstOrDefault();
            }
            else
            {
                token = (await this.options.Connection
                    .QueryAsync<Entities.Token>(sql, new { Key = key, TokenType = (short)tokenType })).FirstOrDefault();
            }

            if (token == null)
            {
                add = true;

                token = new Entities.Token
                {
                    Key = key,
                    SubjectId = value.SubjectId,
                    ClientId = value.ClientId,
                    TokenType = tokenType
                };
            }

            token.JsonCode = ConvertToJson(value);
            token.Expiry = value.CreationTime.AddSeconds(value.LifeTime);

            if (add)
            {
                sql = @"INSERT INTO SACCORE.T_TOKEN VALUES(@Key, @TokenType, @SubjectId, @ClientId, @JsonCode, @Expiry)";
            }
            else
            {
                sql = @"UPDATE SACCORE.T_TOKEN SET JsonCode = @JsonCode, Expiry = @Expiry WHERE Key = @Key AND TokenType = @TokenType";
            }
            

            await this.options.Connection.ExecuteAsync(sql, token);
        }
    }
}
