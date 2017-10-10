using DapperExtensions.Mapper;
using IdentityServer3.Dapper.Entities;

namespace IdentityServer3.Dapper.Mappers
{
    public class TokenMapper : ClassMapper<Token>, IIdentityServerMapper
    {
        public TokenMapper(DapperServiceOptions option)
        {
            Schema(option.SchemaName);
            Table(option.TableNames.Token);

            Map(x => x.Key).Key(KeyType.Assigned);
            Map(x => x.TokenType).Key(KeyType.Assigned);

            AutoMap();
        }
    }
}
