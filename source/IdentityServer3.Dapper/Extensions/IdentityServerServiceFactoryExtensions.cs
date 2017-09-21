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
