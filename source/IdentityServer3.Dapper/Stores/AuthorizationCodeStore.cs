/* *********************************************************************
 * Copyright (c) 2017 南京华盾电力信息安全测评有限公司 All Rights Reserved.
 * CLR 版本: 4.0.30319.42000
 * 公司名称: 南京华盾电力信息安全测评有限公司
 * 命名空间: IdentityServer3.Dapper
 * 文 件 名: AuthorizationCodeStore
 * 版 本 号: V1.0.0.0
 * 创 建 人: 管超
 * 电子邮箱: levid.gc@outlook.com
 * 创建时间: 9/20/2017 13:54:28
 * =====================================================================
 * 修改标记 
 * 修改时间: 9/20/2017 13:54:28
 * 修 改 人: 管超
 * 版 本 号: V1.0.0.0
 * 修改描述:
 * ********************************************************************* */

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
