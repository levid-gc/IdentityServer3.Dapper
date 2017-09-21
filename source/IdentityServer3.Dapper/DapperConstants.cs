/* *********************************************************************
 * Copyright (c) 2017 南京华盾电力信息安全测评有限公司 All Rights Reserved.
 * CLR 版本: 4.0.30319.42000
 * 公司名称: 南京华盾电力信息安全测评有限公司
 * 命名空间: IdentityServer3.Dapper
 * 文 件 名: DapperConstants
 * 版 本 号: V1.0.0.0
 * 创 建 人: 管超
 * 电子邮箱: levid.gc@outlook.com
 * 创建时间: 9/19/2017 11:33:08
 * =====================================================================
 * 修改标记 
 * 修改时间: 9/19/2017 11:33:08
 * 修 改 人: 管超
 * 版 本 号: V1.0.0.0
 * 修改描述:
 * ********************************************************************* */

namespace IdentityServer3.Dapper
{
    class DapperConstants
    {
        public class TableNames
        {
            public const string Client = "Clients";
            public const string ClientClaim = "ClientClaims";
            public const string ClientCustomGrantType = "ClientCustomGrantTypes";
            public const string ClientIdPRestriction = "ClientIdPRestrictions";
            public const string ClientPostLogoutRedirectUri = "ClientPostLogoutRedirectUris";
            public const string ClientRedirectUri = "ClientRedirectUris";
            public const string ClientScopes = "ClientScopes";
            public const string ClientSecret = "ClientSecrets";
            public const string ClientCorsOrigin = "ClientCorsOrigins";

            public const string Scope = "Scopes";
            public const string ScopeClaim = "ScopeClaims";
            public const string ScopeSecrets = "ScopeSecrets";

            public const string Consent = "Consents";
            public const string Token = "Tokens";
        }
    }
}
