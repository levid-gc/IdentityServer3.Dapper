using Dapper;
using DapperExtensions;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Dapper.Entities;
using IdentityServer3.Dapper.Mappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer3.Dapper
{
    public class RefreshTokenStore : BaseTokenStore<RefreshToken>, IRefreshTokenStore
    { 
        public RefreshTokenStore(DapperServiceOptions options, IScopeStore scopeStore, IClientStore clientStore)
            : base(options, TokenType.RefreshToken, scopeStore, clientStore)
        { }

        public override async Task StoreAsync(string key, RefreshToken value)
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

            if (token == null)
            {
                token = new Entities.Token
                {
                    Key = key,
                    SubjectId = value.SubjectId,
                    ClientId = value.ClientId,
                    TokenType = tokenType,
                    JsonCode = ConvertToJson(value),
                    Expiry = value.CreationTime.AddSeconds(value.LifeTime)
                };

                sql = options.SqlGenerator.Insert(new TokenMapper(options));
                await options.Connection.ExecuteAsync(sql, token);
            }
            else
            {
                token.JsonCode = ConvertToJson(value);
                token.Expiry = value.CreationTime.AddSeconds(value.LifeTime);

                parameters = new Dictionary<string, object>();
                sql = options.SqlGenerator.Update(new TokenMapper(options), pg, parameters, true);

                dynamicParameters = new DynamicParameters();
                foreach (var parameter in parameters)
                {
                    dynamicParameters.Add(parameter.Key, parameter.Value);
                }
                dynamicParameters.Add("SubjectId", token.SubjectId);
                dynamicParameters.Add("ClientId", token.ClientId);
                dynamicParameters.Add("JsonCode", token.JsonCode);
                dynamicParameters.Add("Expiry", token.Expiry);

                await options.Connection.ExecuteAsync(sql, dynamicParameters);
            }
        }
    }
}
