/* *********************************************************************
 * Copyright (c) 2017 南京华盾电力信息安全测评有限公司 All Rights Reserved.
 * CLR 版本: 4.0.30319.42000
 * 公司名称: 南京华盾电力信息安全测评有限公司
 * 命名空间: IdentityServer3.Dapper.Extensions
 * 文 件 名: IdentityServerServiceFactoryExtensions
 * 版 本 号: V1.0.0.0
 * 创 建 人: 管超
 * 电子邮箱: levid.gc@outlook.com
 * 创建时间: 9/21/2017 14:32:08
 * =====================================================================
 * 修改标记 
 * 修改时间: 9/21/2017 14:32:08
 * 修 改 人: 管超
 * 版 本 号: V1.0.0.0
 * 修改描述:
 * ********************************************************************* */

#region Usings

using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using System;

#endregion

namespace IdentityServer3.Dapper.Configuration
{
    public static class IdentityServerServiceFactoryExtensions
    {
        public static void RegisterOperationalServices(this IdentityServerServiceFactory factory, DapperServiceOptions options)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            if (options == null) throw new ArgumentNullException("options");

            if (options.SynchronousReads)
            {
                factory.Register(new Registration<DapperServiceOptions>(options));
            }

            factory.AuthorizationCodeStore = new Registration<IAuthorizationCodeStore, AuthorizationCodeStore>();
            factory.TokenHandleStore = new Registration<ITokenHandleStore, TokenHandleStore>();
            factory.ConsentStore = new Registration<IConsentStore, ConsentStore>();
            factory.RefreshTokenStore = new Registration<IRefreshTokenStore, RefreshTokenStore>();
        }

        public static void RegisterConfigurationServices(this IdentityServerServiceFactory factory, DapperServiceOptions options)
        {
            factory.RegisterClientStore(options);
            factory.RegisterScopeStore(options);
        }

        public static void RegisterClientStore(this IdentityServerServiceFactory factory, DapperServiceOptions options)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            if (options == null) throw new ArgumentNullException("options");

            if (options.SynchronousReads)
            {
                factory.Register(new Registration<DapperServiceOptions>(options));
            }

            factory.ClientStore = new Registration<IClientStore, ClientStore>();
            factory.CorsPolicyService = new ClientConfigurationCorsPolicyRegistration(options);
        }

        public static void RegisterScopeStore(this IdentityServerServiceFactory factory, DapperServiceOptions options)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            if (options == null) throw new ArgumentNullException("options");

            if (options.SynchronousReads)
            {
                factory.Register(new Registration<DapperServiceOptions>(options));
            }

            factory.ScopeStore = new Registration<IScopeStore, ScopeStore>();
        }
    }
}
