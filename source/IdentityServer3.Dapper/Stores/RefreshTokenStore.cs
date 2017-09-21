/* *********************************************************************
 * Copyright (c) 2017 南京华盾电力信息安全测评有限公司 All Rights Reserved.
 * CLR 版本: 4.0.30319.42000
 * 公司名称: 南京华盾电力信息安全测评有限公司
 * 命名空间: IdentityServer3.Dapper.Stores
 * 文 件 名: RefreshTokenStore
 * 版 本 号: V1.0.0.0
 * 创 建 人: 管超
 * 电子邮箱: levid.gc@outlook.com
 * 创建时间: 9/20/2017 14:42:40
 * =====================================================================
 * 修改标记 
 * 修改时间: 9/20/2017 14:42:40
 * 修 改 人: 管超
 * 版 本 号: V1.0.0.0
 * 修改描述:
 * ********************************************************************* */

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
