using Dapper;
using IdentityServer3.Dapper.Entities;
using IdentityServer3.Dapper.Mappers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer3.Dapper.Tests.IntegrationTests.SQLServer
{
    public class ClientTests : SQLServerTestBase
    {
        public override void Setup()
        {
            base.Setup();

            var sql = options.SqlGenerator.Insert(new ClientMapper(options));

            IEnumerable<Client> entities = new List<Client>()
            {
                new Client() { ClientId = "client1", ClientName = "Client1", Flow = Core.Models.Flows.AuthorizationCode, AccessTokenType = Core.Models.AccessTokenType.Jwt },
            };

            options.Connection.Execute(sql, entities);
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
