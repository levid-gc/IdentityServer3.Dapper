/* *********************************************************************
 * Copyright (c) 2017 南京华盾电力信息安全测评有限公司 All Rights Reserved.
 * CLR 版本: 4.0.30319.42000
 * 公司名称: 南京华盾电力信息安全测评有限公司
 * 命名空间: IdentityServer3.Dapper.Registrations
 * 文 件 名: ClientConfigurationCorsPolicyRegistration
 * 版 本 号: V1.0.0.0
 * 创 建 人: 管超
 * 电子邮箱: levid.gc@outlook.com
 * 创建时间: 9/21/2017 14:16:25
 * =====================================================================
 * 修改标记 
 * 修改时间: 9/21/2017 14:16:25
 * 修 改 人: 管超
 * 版 本 号: V1.0.0.0
 * 修改描述:
 * ********************************************************************* */

#region Usings

using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using System;

#endregion

namespace IdentityServer3.Dapper
{
    public class ClientConfigurationCorsPolicyRegistration : Registration<ICorsPolicyService, ClientConfigurationCorsPolicyService>
    {
        public ClientConfigurationCorsPolicyRegistration(DapperServiceOptions options)
        {
            if (options == null) throw new ArgumentNullException("options");

            this.AdditionalRegistrations.Add(new Registration<DapperServiceOptions>(resolver => options));
        }
    }
}
