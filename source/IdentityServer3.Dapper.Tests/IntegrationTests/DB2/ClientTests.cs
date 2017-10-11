using Dapper;
using IdentityServer3.Dapper.Entities;
using IdentityServer3.Dapper.Mappers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer3.Dapper.Tests.IntegrationTests.DB2
{
    public class ClientTests : DB2TestBase
    {
        public override void Setup()
        {
            base.Setup();

            var sql = options.SqlGenerator.Insert(new ClientMapper(options));

            IEnumerable<Client> entities = new List<Client>()
            {
                new Client() { ClientId = "client1", ClientName = "Client1", Flow = Core.Models.Flows.AuthorizationCode, AccessTokenType = Core.Models.AccessTokenType.Jwt },
            };

            IList<DynamicParameters> parameters = new List<DynamicParameters>();

            foreach (var entity in entities)
            {
                var parameter = new DynamicParameters();

                foreach (var property in entity.GetType().GetProperties())
                {
                    var value = property.GetValue(entity);

                    if (value?.GetType() == typeof(bool))
                    {
                        parameter.Add(property.Name, (bool)value ? 1 : 0);
                    }
                    else
                    {
                        parameter.Add(property.Name, value);
                    }
                }

                parameters.Add(parameter);
            }

            options.Connection.Execute(sql, parameters);
        }

        [Fact]
        public async Task FindClientByIdAsync_TestAsync()
        {
            var store = new ClientStore(options);

            var client = await store.FindClientByIdAsync("");
            Assert.Null(client);
            client = await store.FindClientByIdAsync("client1");
            Assert.NotNull(client);
        }
    }
}
