using Dapper;
using DapperExtensions;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Dapper.Entities;
using IdentityServer3.Dapper.Mappers;
using IdentityServer3.Dapper.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer3.Dapper
{
    public abstract class BaseTokenStore<T> where T : class
    {
        protected readonly TokenType tokenType;
        protected readonly IScopeStore scopeStore;
        protected readonly IClientStore clientStore;
        protected readonly DapperServiceOptions options;

        protected BaseTokenStore(DapperServiceOptions options, TokenType tokenType, IScopeStore scopeStore, IClientStore clientStore)
        {
            this.options = options ?? throw new ArgumentNullException("options");
            this.tokenType = tokenType;
            this.scopeStore = scopeStore ?? throw new ArgumentNullException("scopeStore");
            this.clientStore = clientStore ?? throw new ArgumentNullException("clientStore");
        }

        JsonSerializerSettings GetJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ClaimConverter());
            settings.Converters.Add(new ClaimsPrincipalConverter());
            settings.Converters.Add(new ClientConverter(clientStore));
            settings.Converters.Add(new ScopeConverter(scopeStore));
            return settings;
        }

        protected string ConvertToJson(T value)
        {
            return JsonConvert.SerializeObject(value, GetJsonSerializerSettings());
        }

        protected T ConvertFromJson(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, GetJsonSerializerSettings());
        }

        public async Task<T> GetAsync(string key)
        {
            var parameters = new Dictionary<string, object>();
            var dynamicParameters = new DynamicParameters();

            var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pg.Predicates.Add(Predicates.Field<Entities.Token>(t => t.Key, Operator.Eq, key));
            pg.Predicates.Add(Predicates.Field<Entities.Token>(t => t.TokenType, Operator.Eq, tokenType));

            var sql = options.SqlGenerator.Select(new TokenMapper(options), pg, null, parameters);

            dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            var token = await options.Connection.QueryFirstOrDefaultAsync<Entities.Token>(sql, dynamicParameters);

            if (token == null || token.Expiry < DateTimeOffset.UtcNow)
            {
                return null;
            }

            return ConvertFromJson(token.JsonCode);
        }

        public async Task RemoveAsync(string key)
        {
            var parameters = new Dictionary<string, object>();
            var dynamicParameters = new DynamicParameters();

            var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pg.Predicates.Add(Predicates.Field<Entities.Token>(t => t.Key, Operator.Eq, key));
            pg.Predicates.Add(Predicates.Field<Entities.Token>(t => t.TokenType, Operator.Eq, tokenType));

            var sql = options.SqlGenerator.Delete(new TokenMapper(options), pg, parameters);

            dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            await options.Connection.ExecuteAsync(sql, dynamicParameters);
        }

        public async Task<IEnumerable<ITokenMetadata>> GetAllAsync(string subject)
        {
            var parameters = new Dictionary<string, object>();
            var dynamicParameters = new DynamicParameters();

            var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pg.Predicates.Add(Predicates.Field<Entities.Token>(t => t.SubjectId, Operator.Eq, subject));
            pg.Predicates.Add(Predicates.Field<Entities.Token>(t => t.TokenType, Operator.Eq, tokenType));

            var sql = options.SqlGenerator.Select(new TokenMapper(options), pg, null, parameters);

            dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            var tokens = await options.Connection.QueryAsync<Entities.Token>(sql, dynamicParameters);

            var results = tokens.Select(x => ConvertFromJson(x.JsonCode)).ToArray();
            return results.Cast<Core.Models.ITokenMetadata>();
        }

        public async Task RevokeAsync(string subject, string client)
        {
            var parameters = new Dictionary<string, object>();
            var dynamicParameters = new DynamicParameters();

            var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pg.Predicates.Add(Predicates.Field<Entities.Token>(t => t.SubjectId, Operator.Eq, subject));
            pg.Predicates.Add(Predicates.Field<Entities.Token>(t => t.ClientId, Operator.Eq, client));
            pg.Predicates.Add(Predicates.Field<Entities.Token>(t => t.TokenType, Operator.Eq, tokenType));

            var sql = options.SqlGenerator.Delete(new TokenMapper(options), pg, parameters);

            dynamicParameters = new DynamicParameters();
            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            await options.Connection.ExecuteAsync(sql, dynamicParameters);
        }

        public abstract Task StoreAsync(string key, T value);
    }
}
