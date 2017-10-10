using DapperExtensions.Mapper;
using IdentityServer3.Dapper.Entities;

namespace IdentityServer3.Dapper.Mappers
{
    public class ConsentMapper : ClassMapper<Consent>, IIdentityServerMapper
    {
        public ConsentMapper(DapperServiceOptions option)
        {
            Schema(option.SchemaName);
            Table(option.TableNames.Consent);

            Map(x => x.Subject).Key(KeyType.Assigned);
            Map(x => x.ClientId).Key(KeyType.Assigned);

            AutoMap();
        }
    }
}
