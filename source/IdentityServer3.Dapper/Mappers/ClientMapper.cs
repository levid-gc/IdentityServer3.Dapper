using DapperExtensions.Mapper;
using IdentityServer3.Dapper.Entities;

namespace IdentityServer3.Dapper.Mappers
{
    public class ClientMapper : ClassMapper<Client>, IIdentityServerMapper
    {
        public ClientMapper(DapperServiceOptions option)
        {
            Schema(option.SchemaName);
            Table(option.TableNames.Client);

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.ClientSecrets).Ignore();
            Map(x => x.RedirectUris).Ignore();
            Map(x => x.PostLogoutRedirectUris).Ignore();
            Map(x => x.AllowedScopes).Ignore();
            Map(x => x.IdentityProviderRestrictions).Ignore();
            Map(x => x.Claims).Ignore();
            Map(x => x.AllowedCustomGrantTypes).Ignore();
            Map(x => x.AllowedCorsOrigins).Ignore();

            AutoMap();
        }
    }

    public class ClientClaimMapper : ClassMapper<ClientClaim>, IIdentityServerMapper
    {
        public ClientClaimMapper(DapperServiceOptions option)
        {
            Schema(option.SchemaName);
            Table(option.TableNames.ClientClaims);

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.ClientId).Column("Client_Id");
            Map(x => x.Client).Ignore();

            AutoMap();
        }
    }

    public class ClientCorsOriginMapper : ClassMapper<ClientCorsOrigin>, IIdentityServerMapper
    {
        public ClientCorsOriginMapper(DapperServiceOptions option)
        {
            Schema(option.SchemaName);
            Table(option.TableNames.ClientCorsOrigins);

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.ClientId).Column("Client_Id");
            Map(x => x.Client).Ignore();

            AutoMap();
        }
    }

    public class ClientCustomGrantTypeMapper : ClassMapper<ClientCustomGrantType>, IIdentityServerMapper
    {
        public ClientCustomGrantTypeMapper(DapperServiceOptions option)
        {
            Schema(option.SchemaName);
            Table(option.TableNames.ClientCustomGrantTypes);

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.ClientId).Column("Client_Id");
            Map(x => x.Client).Ignore();

            AutoMap();
        }
    }

    public class ClientIdPRestrictionMapper : ClassMapper<ClientIdPRestriction>, IIdentityServerMapper
    {
        public ClientIdPRestrictionMapper(DapperServiceOptions option)
        {
            Schema(option.SchemaName);
            Table(option.TableNames.ClientIdPRestrictions);

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.ClientId).Column("Client_Id");
            Map(x => x.Client).Ignore();

            AutoMap();
        }
    }

    public class ClientPostLogoutRedirectUriMapper : ClassMapper<ClientPostLogoutRedirectUri>, IIdentityServerMapper
    {
        public ClientPostLogoutRedirectUriMapper(DapperServiceOptions option)
        {
            Schema(option.SchemaName);
            Table(option.TableNames.ClientPostLogoutRedirectUris);

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.ClientId).Column("Client_Id");
            Map(x => x.Client).Ignore();

            AutoMap();
        }
    }

    public class ClientRedirectUriMapper : ClassMapper<ClientRedirectUri>, IIdentityServerMapper
    {
        public ClientRedirectUriMapper(DapperServiceOptions option)
        {
            Schema(option.SchemaName);
            Table(option.TableNames.ClientRedirectUris);

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.ClientId).Column("Client_Id");
            Map(x => x.Client).Ignore();

            AutoMap();
        }
    }

    public class ClientScopeMapper : ClassMapper<ClientScope>, IIdentityServerMapper
    {
        public ClientScopeMapper(DapperServiceOptions option)
        {
            Schema(option.SchemaName);
            Table(option.TableNames.ClientScopes);

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.ClientId).Column("Client_Id");
            Map(x => x.Client).Ignore();

            AutoMap();
        }
    }

    public class ClientSecretMapper : ClassMapper<ClientSecret>, IIdentityServerMapper
    {
        public ClientSecretMapper(DapperServiceOptions option)
        {
            Schema(option.SchemaName);
            Table(option.TableNames.ClientSecrets);

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.ClientId).Column("Client_Id");
            Map(x => x.Client).Ignore();

            AutoMap();
        }
    }
}
