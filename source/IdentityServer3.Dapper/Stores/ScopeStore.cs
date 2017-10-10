using Dapper;
using DapperExtensions;
using IdentityServer3.Core.Services;
using IdentityServer3.Dapper.Entities;
using IdentityServer3.Dapper.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer3.Dapper
{
    public class ScopeStore : IScopeStore
    {
        private readonly DapperServiceOptions options;

        public ScopeStore(DapperServiceOptions options)
        {
            this.options = options ?? throw new ArgumentNullException("options");
        }

        public async Task<IEnumerable<IdentityServer3.Core.Models.Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
        {
            IPredicate predicate = null;
            var parameters = new Dictionary<string, object>();
            var dynamicParameters = new DynamicParameters();

            if (scopeNames != null && scopeNames.Any())
            {
                predicate = Predicates.Field<Scope>(s => s.Name, Operator.Eq, scopeNames.ToArray());
            }

            var sql = options.SqlGenerator.Select(new ScopeMapper(options), predicate, null, parameters);

            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            var scopes = await options.Connection.QueryAsync<Scope>(sql, dynamicParameters);

            foreach (var scope in scopes)
            {
                parameters = new Dictionary<string, object>();

                var predicate1 = Predicates.Field<ScopeClaim>(s => s.ScopeId, Operator.Eq, scope.Id);
                var sql1 = options.SqlGenerator.Select(new ScopeClaimMapper(options), predicate1, null, parameters);

                var predicate2 = Predicates.Field<ScopeSecret>(s => s.ScopeId, Operator.Eq, scope.Id);
                var sql2 = options.SqlGenerator.Select(new ScopeSecretMapper(options), predicate2, null, parameters);

                sql = sql1 + " " + sql2;

                dynamicParameters = new DynamicParameters();
                foreach (var parameter in parameters)
                {
                    dynamicParameters.Add(parameter.Key, parameter.Value);
                }

                using (var multi = await options.Connection.QueryMultipleAsync(sql, dynamicParameters))
                {
                    scope.ScopeClaims = multi.Read<ScopeClaim>().ToList();
                    scope.ScopeSecrets = multi.Read<ScopeSecret>().ToList();
                }
            }

            return scopes.Select(x => x.ToModel());
        }

        public async Task<IEnumerable<IdentityServer3.Core.Models.Scope>> GetScopesAsync(bool publicOnly = true)
        {
            IPredicate predicate = null;
            var parameters = new Dictionary<string, object>();
            var dynamicParameters = new DynamicParameters();

            if (publicOnly)
            {
                predicate = Predicates.Field<Scope>(s => s.ShowInDiscoveryDocument, Operator.Eq, true);
            }

            var sql = options.SqlGenerator.Select(new ScopeMapper(options), predicate, null, parameters);

            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            var scopes = await options.Connection.QueryAsync<Scope>(sql, dynamicParameters);

            foreach (var scope in scopes)
            {
                parameters = new Dictionary<string, object>();

                var predicate1 = Predicates.Field<ScopeClaim>(s => s.ScopeId, Operator.Eq, scope.Id);
                var sql1 = options.SqlGenerator.Select(new ScopeClaimMapper(options), predicate1, null, parameters);

                var predicate2 = Predicates.Field<ScopeSecret>(s => s.ScopeId, Operator.Eq, scope.Id);
                var sql2 = options.SqlGenerator.Select(new ScopeSecretMapper(options), predicate2, null, parameters);

                sql = sql1 + " " + sql2;

                dynamicParameters = new DynamicParameters();
                foreach (var parameter in parameters)
                {
                    dynamicParameters.Add(parameter.Key, parameter.Value);
                }

                using (var multi = await options.Connection.QueryMultipleAsync(sql, dynamicParameters))
                {
                    scope.ScopeClaims = multi.Read<ScopeClaim>().ToList();
                    scope.ScopeSecrets = multi.Read<ScopeSecret>().ToList();
                }
            }

            return scopes.Select(x => x.ToModel());
        }
    }
}
