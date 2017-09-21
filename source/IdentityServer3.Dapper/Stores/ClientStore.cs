#region Usings

using Dapper;
using IdentityServer3.Core.Services;
using IdentityServer3.Dapper.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

#endregion

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
            Client client = null;

            var sql = @"SELECT * FROM SACCORE.T_CLIENT WHERE ID = @ClientId
                        SELECT * FROM SACCORE.T_CLIENT_SECRET WHERE CLIENT_ID = @ClientId 
                        SELECT * FROM SACCORE.T_CLIENT_REDIRECT_URI WHERE CLIENT_ID = @ClientId
                        SELECT * FROM SACCORE.T_CLIENT_POST_LOGOUT_REDIRECT_URI WHERE CLIENT_ID = @ClientId
                        SELECT * FROM SACCORE.T_CLIENT_SCOPE_RESTRICTION WHERE CLIENT_ID = @ClientId
                        SELECT * FROM SACCORE.T_CLIENT_IDP_RESTRICTION WHERE CLIENT_ID = @ClientId
                        SELECT * FROM SACCORE.T_CLIENT_CLAIM WHERE CLIENT_ID = @ClientId
                        SELECT * FROM SACCORE.T_CLIENT_GRANT_TYPE_RESTRICTION WHERE CLIENT_ID = @ClientId
                        SELECT * FROM SACCORE.T_CLIENT_CORS_ORIGIN WHERE CLIENT_ID = @ClientId ";


            if (this.options.SynchronousReads)
            {
                using (var multi = this.options.Connection.QueryMultiple(sql, new { ClientId = clientId }))
                {
                    client = multi.Read<Client>().Single();
                    client.ClientSecrets = multi.Read<ClientSecret>().ToList();
                    client.RedirectUris = multi.Read<ClientRedirectUri>().ToList();
                    client.PostLogoutRedirectUris = multi.Read<ClientPostLogoutRedirectUri>().ToList();
                    client.AllowedScopes = multi.Read<ClientScope>().ToList();
                    client.IdentityProviderRestrictions = multi.Read<ClientIdPRestriction>().ToList();
                    client.Claims = multi.Read<ClientClaim>().ToList();
                    client.AllowedCustomGrantTypes = multi.Read<ClientCustomGrantType>().ToList();
                    client.AllowedCorsOrigins = multi.Read<ClientCorsOrigin>().ToList();
                }
            }
            else
            {
                using (var multi =  await this.options.Connection.QueryMultipleAsync(sql, new { ClientId = clientId }))
                {
                    client = multi.Read<Client>().Single();
                    client.ClientSecrets = multi.Read<ClientSecret>().ToList();
                    client.RedirectUris = multi.Read<ClientRedirectUri>().ToList();
                    client.PostLogoutRedirectUris = multi.Read<ClientPostLogoutRedirectUri>().ToList();
                    client.AllowedScopes = multi.Read<ClientScope>().ToList();
                    client.IdentityProviderRestrictions = multi.Read<ClientIdPRestriction>().ToList();
                    client.Claims = multi.Read<ClientClaim>().ToList();
                    client.AllowedCustomGrantTypes = multi.Read<ClientCustomGrantType>().ToList();
                    client.AllowedCorsOrigins = multi.Read<ClientCorsOrigin>().ToList();
                }
            }

            IdentityServer3.Core.Models.Client model = client.ToModel();
            return model;
        }
    }
}
