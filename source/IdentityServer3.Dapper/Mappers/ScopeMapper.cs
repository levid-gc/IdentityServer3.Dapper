using DapperExtensions.Mapper;
using IdentityServer3.Dapper.Entities;

namespace IdentityServer3.Dapper.Mappers
{
    public class ScopeMapper : ClassMapper<Scope>
    {
        public ScopeMapper()
        {
            // use a custom schema
            Schema("dbo");

            // use different table name
            Table("Scopes");

            // Ignore this property entirely
            Map(x => x.ScopeClaims).Ignore();

            // optional, map all other columns
            AutoMap();
        }
    }
}
