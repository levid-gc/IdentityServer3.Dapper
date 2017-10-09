using DapperExtensions.Mapper;
using IdentityServer3.Dapper.Entities;

namespace IdentityServer3.Dapper.Mappers
{
    public class ScopeMapper : ClassMapper<Scope>, IIdentityServerMapper
    {
        public ScopeMapper(DapperServiceOptions option)
        {
            Schema(option.SchemaName);
            Table(option.TableNames.Scope);

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.ScopeClaims).Ignore();
            Map(x => x.ScopeSecrets).Ignore();

            AutoMap();
        }
    }

    public class ScopeClaimMapper : ClassMapper<ScopeClaim>, IIdentityServerMapper
    {
        public ScopeClaimMapper(DapperServiceOptions option)
        {
            Schema(option.SchemaName);
            Table(option.TableNames.ScopeClaims);

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.ScopeId).Column("Scope_Id");
            Map(x => x.Scope).Ignore();

            AutoMap();
        }
    }

    public class ScopeSecretMapper : ClassMapper<ScopeSecret>, IIdentityServerMapper
    {
        public ScopeSecretMapper(DapperServiceOptions option)
        {
            Schema(option.SchemaName);
            Table(option.TableNames.ScopeSecrets);

            Map(x => x.Id).Key(KeyType.Identity);
            Map(x => x.ScopeId).Column("Scope_Id");
            Map(x => x.Scope).Ignore();

            AutoMap();
        }
    }
}
