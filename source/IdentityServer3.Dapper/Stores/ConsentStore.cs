#region Usings

using Dapper;
using IdentityServer3.Core.Services;
using IdentityServer3.Dapper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

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
            Consent found = null;

            var sql = @"SELECT * FROM SACCORE.T_CONSENT WHERE Subject = @Subject AND ClientId = @ClientId";

            if (options != null && options.SynchronousReads)
            {
                found = this.options.Connection
                    .Query<Entities.Consent>(sql, new { Subject = subject, ClientId = client }).FirstOrDefault();
            }
            else
            {
                found = (await this.options.Connection
                    .QueryAsync<Entities.Consent>(sql, new { Subject = subject, ClientId = client })).FirstOrDefault();
            }

            if (found == null)
            {
                return null;
            }

            var result = new IdentityServer3.Core.Models.Consent
            {
                Subject = found.Subject,
                ClientId = found.ClientId,
                Scopes = ParseScopes(found.Scopes)
            };

            return result;
        }

        public async Task UpdateAsync(IdentityServer3.Core.Models.Consent consent)
        {
            Consent item = null;
            bool add = false;
            bool delete = false;

            var sql = @"SELECT * FROM SACCORE.T_CONSENT WHERE Subject = @Subject AND ClientId = @ClientId";

            if (options != null && options.SynchronousReads)
            {
                item = this.options.Connection
                    .Query<Entities.Consent>(sql, consent).FirstOrDefault();
            }
            else
            {
                item = (await this.options.Connection
                    .QueryAsync<Entities.Consent>(sql, consent)).FirstOrDefault();
            }

            if (item == null)
            {
                item = new Entities.Consent
                {
                    Subject = consent.Subject,
                    ClientId = consent.ClientId
                };

                add = true;
            }

            if (consent.Scopes == null || !consent.Scopes.Any())
            {
                delete = true;
            }

            item.Scopes = StringifyScopes(consent.Scopes);

            if (add == false && delete == false)
            {
                sql = @"UPDATE SACCORE.T_CONSENT SET Scopes = @Scopes WHERE Subject = @Subject AND ClientId = @ClientId";
                await this.options.Connection.ExecuteAsync(sql, item);
            }
            else if (add == true && delete == false)
            {
                sql = @"INSERT INTO SACCORE.T_CONSENT VALUES(@Subject, @ClientId, @Scopes)";
                await this.options.Connection.ExecuteAsync(sql, item);
            }
            else if (add == false && delete == true)
            {
                sql = @"DELETE FROM SACCORE.T_CONSENT WHERE Subject = @Subject AND ClientId = @ClientId";
                await this.options.Connection.ExecuteAsync(sql, item);
            }

            return;
        }

        public async Task<IEnumerable<IdentityServer3.Core.Models.Consent>> LoadAllAsync(string subject)
        {
            Consent[] found = null;

            var sql = @"SELECT * FROM SACCORE.T_CONSENT WHERE Subject = @Subject";

            if (options != null && options.SynchronousReads)
            {
                found = this.options.Connection
                    .Query<Entities.Consent>(sql, new { Subject = subject }).ToArray();
            }
            else
            {
                found = (await this.options.Connection
                    .QueryAsync<Entities.Consent>(sql, new { Subject = subject })).ToArray();
            }


            var results = found.Select(x => new IdentityServer3.Core.Models.Consent
            {
                Subject = x.Subject,
                ClientId = x.ClientId,
                Scopes = ParseScopes(x.Scopes)
            });

            return results.ToArray().AsEnumerable();
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

        public async Task RevokeAsync(string subject, string client)
        {
            Consent found = null;

            var sql = @"SELECT * FROM SACCORE.T_CONSENT WHERE Subject = @Subject AND ClientId = @ClientId";

            if (options != null && options.SynchronousReads)
            {
                found = this.options.Connection
                    .Query<Entities.Consent>(sql, new { Subject = subject, ClientId = client }).FirstOrDefault();
            }
            else
            {
                found = (await this.options.Connection
                    .QueryAsync<Entities.Consent>(sql, new { Subject = subject, ClientId = client })).FirstOrDefault();
            }

            if (found != null)
            {
                sql = @"DELETE FROM SACCORE.T_CONSENT WHERE Subject = @Subject AND ClientId = @ClientId";
                await this.options.Connection.ExecuteAsync(sql, found);
            }
        }
    }
}
