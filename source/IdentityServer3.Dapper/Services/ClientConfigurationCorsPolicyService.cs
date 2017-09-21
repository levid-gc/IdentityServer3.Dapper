/* *********************************************************************
 * Copyright (c) 2017 南京华盾电力信息安全测评有限公司 All Rights Reserved.
 * CLR 版本: 4.0.30319.42000
 * 公司名称: 南京华盾电力信息安全测评有限公司
 * 命名空间: IdentityServer3.Dapper
 * 文 件 名: ClientConfigurationCorsPolicyService
 * 版 本 号: V1.0.0.0
 * 创 建 人: 管超
 * 电子邮箱: levid.gc@outlook.com
 * 创建时间: 9/21/2017 14:05:14
 * =====================================================================
 * 修改标记 
 * 修改时间: 9/21/2017 14:05:14
 * 修 改 人: 管超
 * 版 本 号: V1.0.0.0
 * 修改描述:
 * ********************************************************************* */

#region Usings

using Dapper;
using IdentityServer3.Core.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace IdentityServer3.Dapper
{
    public class ClientConfigurationCorsPolicyService : ICorsPolicyService
    {
        private readonly DapperServiceOptions options;

        public ClientConfigurationCorsPolicyService(DapperServiceOptions options)
        {
            this.options = options ?? throw new ArgumentNullException("options");
        }

        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            var sql = @"SELECT Origin FROM SACCORE.T_CLIENT_CORS_ORIGIN";

            var urls = (await this.options.Connection.QueryAsync<string>(sql)).ToArray();

            var origins = urls.Select(x => x.GetOrigin()).Where(x => x != null).Distinct();

            var result = origins.Contains(origin, StringComparer.OrdinalIgnoreCase);

            return result;
        }
    }
}
