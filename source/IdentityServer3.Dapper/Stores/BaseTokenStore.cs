#region Usings

using Dapper;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Dapper.Entities;
using IdentityServer3.Dapper.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

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
            Entities.Token token = null;

            var sql = @"SELECT * FROM SACCORE.T_TOKEN WHERE Key = @Key AND TokenType = @TokenType";

            if (options != null && options.SynchronousReads)
            {
                token = this.options.Connection
                    .Query<Entities.Token>(sql, new { Key = key, TokenType = (short)tokenType }).FirstOrDefault();
            }
            else
            {
                token = (await this.options.Connection
                    .QueryAsync<Entities.Token>(sql, new { Key = key, TokenType = (short)tokenType })).FirstOrDefault();
            }

            if (token == null || token.Expiry < DateTimeOffset.UtcNow)
            {
                return null;
            }

            return ConvertFromJson(token.JsonCode);
        }

        public async Task RemoveAsync(string key)
        {
            Entities.Token token = null;
            var sql = @"SELECT * FROM SACCORE.T_TOKEN WHERE Key = @Key AND TokenType = @TokenType";

            if (options != null && options.SynchronousReads)
            {
                token = this.options.Connection
                    .Query<Entities.Token>(sql, new { Key = key, TokenType = (short)tokenType }).FirstOrDefault();
            }
            else
            {
                token = (await this.options.Connection
                    .QueryAsync<Entities.Token>(sql, new { Key = key, TokenType = (short)tokenType })).FirstOrDefault();
            }

            if (token != null)
            {
                sql = @"DELETE FROM SACCORE.T_TOKEN WHERE Key = @Key AND TokenType = @TokenType";
                await this.options.Connection
                    .ExecuteAsync(sql, new { Key = token.Key, TokenType = (short)token.TokenType });
            }
        }

        public async Task<IEnumerable<ITokenMetadata>> GetAllAsync(string subject)
        {
            Entities.Token[] tokens = null;
            var sql = @"SELECT * FROM SACCORE.T_TOKEN WHERE SubjectId = @SubjectId AND TokenType = @TokenType";

            if (options != null && options.SynchronousReads)
            {
                tokens = this.options.Connection
                    .Query<Entities.Token>(sql, new { SubjectId = subject, TokenType = (short)tokenType }).ToArray();
            }
            else
            {
                tokens = (await this.options.Connection
                    .QueryAsync<Entities.Token>(sql, new { SubjectId = subject, TokenType = (short)tokenType })).ToArray();
            }

            var results = tokens.Select(x => ConvertFromJson(x.JsonCode)).ToArray();
            return results.Cast<Core.Models.ITokenMetadata>();
        }

        public async Task RevokeAsync(string subject, string client)
        {
            Entities.Token[] tokens = null;
            var sql = @"SELECT * FROM SACCORE.T_TOKEN WHERE SubjectId = @SubjectId AND ClientId = @ClientId AND TokenType = @TokenType";

            if (options != null && options.SynchronousReads)
            {
                tokens = this.options.Connection
                    .Query<Entities.Token>(sql, new { SubjectId = subject, ClientId = client, TokenType = (short)tokenType }).ToArray();
            }
            else
            {
                tokens = (await this.options.Connection
                    .QueryAsync<Entities.Token>(sql, new { SubjectId = subject, ClientId = client, TokenType = (short)tokenType })).ToArray();
            }

            if (tokens != null && tokens.Any())
            {
                sql = @"DELETE FROM SACCORE.T_TOKEN WHERE SubjectId IN @SubjectIds AND ClientId IN @ClientIds AND TokenType = @TokenType";
                await this.options.Connection
                    .ExecuteAsync(sql, new { SubjectIds = tokens.Select(t => t.SubjectId).ToArray(), ClientIds = tokens.Select(t => t.ClientId).ToArray(), TokenType = (short)tokenType });
            }
        }

        public abstract Task StoreAsync(string key, T value);
    }
}
