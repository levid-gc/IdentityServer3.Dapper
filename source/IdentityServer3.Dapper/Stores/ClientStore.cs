using Dapper;
using DapperExtensions;
using IdentityServer3.Core.Services;
using IdentityServer3.Dapper.Entities;
using IdentityServer3.Dapper.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer3.Dapper
{
    public class ClientStore : IClientStore
    {
        private readonly DapperServiceOptions options;

        public ClientStore(DapperServiceOptions options)
        {
            this.options = options ?? throw new ArgumentNullException("options");
        }

        public async Task<IdentityServer3.Core.Models.Client> FindClientByIdAsync(string clientId)
        {
            var parameters = new Dictionary<string, object>();
            var dynamicParameters = new DynamicParameters();

            var sql = options.SqlGenerator.Select(new ClientMapper(options), Predicates.Field<Client>(s => s.ClientId, Operator.Eq, clientId), null, parameters);

            dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            var client = await options.Connection.QueryFirstOrDefaultAsync<Client>(sql, dynamicParameters);

            if (client != null)
            {
                parameters = new Dictionary<string, object>();

                sql =
                    options.SqlGenerator.Select(new ClientSecretMapper(options), Predicates.Field<ClientSecret>(s => s.ClientId, Operator.Eq, client.Id), null, parameters) + " " +
                    options.SqlGenerator.Select(new ClientRedirectUriMapper(options), Predicates.Field<ClientRedirectUri>(s => s.ClientId, Operator.Eq, client.Id), null, parameters) + " " +
                    options.SqlGenerator.Select(new ClientPostLogoutRedirectUriMapper(options), Predicates.Field<ClientPostLogoutRedirectUri>(s => s.ClientId, Operator.Eq, client.Id), null, parameters) + " " +
                    options.SqlGenerator.Select(new ClientScopeMapper(options), Predicates.Field<ClientScope>(s => s.ClientId, Operator.Eq, client.Id), null, parameters) + " " +
                    options.SqlGenerator.Select(new ClientIdPRestrictionMapper(options), Predicates.Field<ClientIdPRestriction>(s => s.ClientId, Operator.Eq, client.Id), null, parameters) + " " +
                    options.SqlGenerator.Select(new ClientClaimMapper(options), Predicates.Field<ClientClaim>(s => s.ClientId, Operator.Eq, client.Id), null, parameters) + " " +
                    options.SqlGenerator.Select(new ClientCustomGrantTypeMapper(options), Predicates.Field<ClientCustomGrantType>(s => s.ClientId, Operator.Eq, client.Id), null, parameters) + " " +
                    options.SqlGenerator.Select(new ClientCorsOriginMapper(options), Predicates.Field<ClientCorsOrigin>(s => s.ClientId, Operator.Eq, client.Id), null, parameters);

                dynamicParameters = new DynamicParameters();
                foreach (var parameter in parameters)
                {
                    dynamicParameters.Add(parameter.Key, parameter.Value);
                }

                using (var multi = await options.Connection.QueryMultipleAsync(sql, dynamicParameters))
                {
                    client.ClientSecrets = multi.Read<ClientSecret>().ToList();
                    client.RedirectUris = multi.Read<ClientRedirectUri>().ToList();
                    client.PostLogoutRedirectUris = multi.Read<ClientPostLogoutRedirectUri>().ToList();
                    client.AllowedScopes = multi.Read<ClientScope>().ToList();
                    client.IdentityProviderRestrictions = multi.Read<ClientIdPRestriction>().ToList();
                    client.AllowedCustomGrantTypes = multi.Read<ClientCustomGrantType>().ToList();
                    client.AllowedCorsOrigins = multi.Read<ClientCorsOrigin>().ToList();
                }
            }

            IdentityServer3.Core.Models.Client model = client.ToModel();
            return model;
        }
    }
}
