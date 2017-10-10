using Dapper;
using IdentityServer3.Core.Services;
using IdentityServer3.Dapper.Entities;
using IdentityServer3.Dapper.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var sql = options.SqlGenerator.Select(new ClientCorsOriginMapper(options), null, null, new Dictionary<string, object>());

            var urls = (await this.options.Connection.QueryAsync<ClientCorsOrigin>(sql)).Select(x => x.Origin).ToArray();

            var origins = urls.Select(x => x.GetOrigin()).Where(x => x != null).Distinct();

            var result = origins.Contains(origin, StringComparer.OrdinalIgnoreCase);

            return result;
        }
    }
}
