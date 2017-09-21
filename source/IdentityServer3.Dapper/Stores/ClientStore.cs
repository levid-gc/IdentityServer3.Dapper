/* *********************************************************************
 * Copyright (c) 2017 南京华盾电力信息安全测评有限公司 All Rights Reserved.
 * CLR 版本: 4.0.30319.42000
 * 公司名称: 南京华盾电力信息安全测评有限公司
 * 命名空间: IdentityServer3.Dapper
 * 文 件 名: ClientStore
 * 版 本 号: V1.0.0.0
 * 创 建 人: 管超
 * 电子邮箱: levid.gc@outlook.com
 * 创建时间: 9/21/2017 08:45:07
 * =====================================================================
 * 修改标记 
 * 修改时间: 9/21/2017 08:45:07
 * 修 改 人: 管超
 * 版 本 号: V1.0.0.0
 * 修改描述:
 * ********************************************************************* */

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
