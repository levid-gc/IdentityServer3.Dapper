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
