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
    public class ConsentStore : IConsentStore
    {
        private readonly DapperServiceOptions options;

        public ConsentStore(DapperServiceOptions options)
        {
            this.options = options ?? throw new ArgumentNullException("options");
        }

        public async Task<IdentityServer3.Core.Models.Consent> LoadAsync(string subject, string client)
        {
            var parameters = new Dictionary<string, object>();
            var dynamicParameters = new DynamicParameters();

            var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pg.Predicates.Add(Predicates.Field<Consent>(t => t.Subject, Operator.Eq, subject));
            pg.Predicates.Add(Predicates.Field<Consent>(t => t.ClientId, Operator.Eq, client));

            var sql = options.SqlGenerator.Select(new ConsentMapper(options), pg, null, parameters);

            dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            var consent = await options.Connection.QueryFirstOrDefaultAsync<Consent>(sql, dynamicParameters);

            if (consent == null)
            {
                return null;
            }

            var result = new IdentityServer3.Core.Models.Consent
            {
                Subject = consent.Subject,
                ClientId = consent.ClientId,
                Scopes = ParseScopes(consent.Scopes)
            };

            return result;
        }

        public async Task UpdateAsync(IdentityServer3.Core.Models.Consent consent)
        {
            var parameters = new Dictionary<string, object>();
            var dynamicParameters = new DynamicParameters();

            var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pg.Predicates.Add(Predicates.Field<Consent>(t => t.Subject, Operator.Eq, consent.Subject));
            pg.Predicates.Add(Predicates.Field<Consent>(t => t.ClientId, Operator.Eq, consent.ClientId));

            var sql = options.SqlGenerator.Select(new ConsentMapper(options), pg, null, parameters);

            dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            var item = await options.Connection.QueryFirstOrDefaultAsync<Consent>(sql, dynamicParameters);

            if (item == null)
            {
                if (consent.Scopes == null || !consent.Scopes.Any())
                {
                    return;
                }

                item = new Entities.Consent
                {
                    Subject = consent.Subject,
                    ClientId = consent.ClientId,
                    Scopes = StringifyScopes(consent.Scopes)
                };

                sql = options.SqlGenerator.Insert(new ConsentMapper(options));
                await options.Connection.ExecuteAsync(sql, item);
            }
            else
            {
                if (consent.Scopes == null || !consent.Scopes.Any())
                {
                    parameters = new Dictionary<string, object>();
                    sql = options.SqlGenerator.Delete(new ConsentMapper(options), pg, parameters);

                    dynamicParameters = new DynamicParameters();
                    foreach (var parameter in parameters)
                    {
                        dynamicParameters.Add(parameter.Key, parameter.Value);
                    }

                    await options.Connection.ExecuteAsync(sql, dynamicParameters);
                }
                else
                {
                    item.Scopes = StringifyScopes(consent.Scopes);

                    parameters = new Dictionary<string, object>();
                    sql = options.SqlGenerator.Update(new ConsentMapper(options), pg, parameters, true);

                    dynamicParameters = new DynamicParameters();
                    foreach (var parameter in parameters)
                    {
                        dynamicParameters.Add(parameter.Key, parameter.Value);
                    }
                    dynamicParameters.Add("Scopes", item.Scopes);

                    await options.Connection.ExecuteAsync(sql, dynamicParameters);
                }
            }
        }

        public async Task<IEnumerable<IdentityServer3.Core.Models.Consent>> LoadAllAsync(string subject)
        {
            var parameters = new Dictionary<string, object>();
            var dynamicParameters = new DynamicParameters();

            var sql = options.SqlGenerator.Select(new TokenMapper(options), Predicates.Field<Consent>(c => c.Subject, Operator.Eq, subject), null, parameters);

            dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            var consents = await options.Connection.QueryAsync<Consent>(sql, dynamicParameters);

            var results = consents.Select(x => new IdentityServer3.Core.Models.Consent
            {
                Subject = x.Subject,
                ClientId = x.ClientId,
                Scopes = ParseScopes(x.Scopes)
            });

            return results.ToArray().AsEnumerable();
        }

        public async Task RevokeAsync(string subject, string client)
        {
            var parameters = new Dictionary<string, object>();
            var dynamicParameters = new DynamicParameters();

            var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pg.Predicates.Add(Predicates.Field<Consent>(t => t.Subject, Operator.Eq, subject));
            pg.Predicates.Add(Predicates.Field<Consent>(t => t.ClientId, Operator.Eq, client));

            var sql = options.SqlGenerator.Delete(new ConsentMapper(options), pg, parameters);

            dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            await options.Connection.ExecuteAsync(sql, dynamicParameters);
        }

        private IEnumerable<string> ParseScopes(string scopes)
        {
            if (scopes == null || String.IsNullOrWhiteSpace(scopes))
            {
                return Enumerable.Empty<string>();
            }

            return scopes.Split(',');
        }

        private string StringifyScopes(IEnumerable<string> scopes)
        {
            if (scopes == null || !scopes.Any())
            {
                return null;
            }

            return scopes.Aggregate((s1, s2) => s1 + "," + s2);
        }
    }
}
