using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using System;

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
