using Dapper;
using IdentityServer3.Core.Models;
using IdentityServer3.Dapper.Mappers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IdentityServer3.Dapper.Tests.IntegrationTests.DB2
{
    public class ScopeTests : DB2TestBase
    {
        public override void Setup()
        {
            base.Setup();

            var sql = options.SqlGenerator.Insert(new ScopeMapper(options));

            IEnumerable<Entities.Scope> entities = new List<Entities.Scope>()
            {
                new Entities.Scope() { Name = "scope1", Enabled = true, Required = false, Emphasize = false, Type = (int)ScopeType.Identity, IncludeAllClaimsForUser = true, ShowInDiscoveryDocument = false },
                new Entities.Scope() { Name = "scope2", Enabled = true, Required = true, Emphasize = false, Type = (int)ScopeType.Identity, IncludeAllClaimsForUser = true, ShowInDiscoveryDocument = true },
                new Entities.Scope() { Name = "scope3", Enabled = false, Required = true, Emphasize = false, Type = (int)ScopeType.Identity, IncludeAllClaimsForUser = true, ShowInDiscoveryDocument = true },
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
        public async Task FindScopesAsync_Test()
        {
            var store = new ScopeStore(options);

            var scopes = await store.FindScopesAsync(null);
            Assert.Equal<int>(3, scopes.Count());
            scopes = await store.FindScopesAsync(new List<string> { "scope1" });
            Assert.Equal<int>(1, scopes.Count());
        }

        [Fact]
        public async Task GetScopesAsync_Test()
        {
            var store = new ScopeStore(options);

            var scopes = await store.GetScopesAsync();
            Assert.Equal<int>(2, scopes.Count());
            scopes = await store.GetScopesAsync(false);
            Assert.Equal<int>(3, scopes.Count());
        }
    }
}
