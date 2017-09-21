﻿/* *********************************************************************
 * Copyright (c) 2017 南京华盾电力信息安全测评有限公司 All Rights Reserved.
 * CLR 版本: 4.0.30319.42000
 * 公司名称: 南京华盾电力信息安全测评有限公司
 * 命名空间: IdentityServer3.Dapper
 * 文 件 名: TokenHandleStore
 * 版 本 号: V1.0.0.0
 * 创 建 人: 管超
 * 电子邮箱: levid.gc@outlook.com
 * 创建时间: 9/20/2017 14:58:42
 * =====================================================================
 * 修改标记 
 * 修改时间: 9/20/2017 14:58:42
 * 修 改 人: 管超
 * 版 本 号: V1.0.0.0
 * 修改描述:
 * ********************************************************************* */

#region Usings

using Dapper;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using System;
using System.Threading.Tasks;

#endregion

namespace IdentityServer3.Dapper
{
    public class TokenHandleStore : BaseTokenStore<Token>, ITokenHandleStore
    {
        public TokenHandleStore(DapperServiceOptions options, IScopeStore scopeStore, IClientStore clientStore)
            : base(options, Entities.TokenType.TokenHandle, scopeStore, clientStore)
        {
        }

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

            var sql = @"INSERT INTO SACCORE.T_TOKEN VALUES(@Key, @TokenType, @SubjectId, @ClientId, @JsonCode, @Expiry)";

            await this.options.Connection.ExecuteAsync(sql, efToken);
        }
    }
}
