#region Usings

using Dapper;
using IdentityServer3.Core.Services;
using IdentityServer3.Dapper.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace IdentityServer3.Dapper
{
    public class ScopeStore : IScopeStore
    {
        private readonly DapperServiceOptions options;

        private readonly Func<Scope, ScopeClaim, ScopeSecret, Scope> func = (scope, claim, secret) =>
        {
            scope.ScopeClaims = claim != null ? new Collection<ScopeClaim>() { claim } : null;
            scope.ScopeSecrets = secret != null ? new Collection<ScopeSecret>() { secret } : null;
            return scope;
        };

        public ScopeStore(DapperServiceOptions options)
        {
            this.options = options ?? throw new ArgumentNullException("options");
        }

        public async Task<IEnumerable<IdentityServer3.Core.Models.Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
        {
            Scope[] list = null;
            List<Scope> scopes = null;

            var sql = @"SELECT 
                                * 
                        FROM 
                                SACCORE.T_SCOPE AS A 
                        LEFT JOIN
                                SACCORE.T_SCOPE_CLAIM AS B ON B.SCOPE_ID = A.ID 
                        LEFT JOIN
                                SACCORE.T_SCOPE_SECRET AS C ON C.SCOPE_ID = A.ID ";

            if (scopeNames != null && scopeNames.Any())
            {
                sql += @"WHERE A.Name IN @Names";

                if (options != null && options.SynchronousReads)
                {
                    list = this.options.Connection
                        .Query<Scope, ScopeClaim, ScopeSecret, Scope>(sql, func, new { Names = scopeNames.ToArray() }).ToArray();
                } 
                else
                {
                    list = (await this.options.Connection
                        .QueryAsync<Scope, ScopeClaim, ScopeSecret, Scope>(sql, func, new { Names = scopeNames.ToArray() })).ToArray();
                }
            }
            else
            {
                if (options != null && options.SynchronousReads)
                {
                    list = this.options.Connection
                        .Query<Scope, ScopeClaim, ScopeSecret, Scope>(sql, func).ToArray();
                }
                else
                {
                    list = (await this.options.Connection
                        .QueryAsync<Scope, ScopeClaim, ScopeSecret, Scope>(sql, func)).ToArray();
                }
            }

            scopes = Parse(list);
            return scopes.Select(x => x.ToModel());
        }

        public async Task<IEnumerable<IdentityServer3.Core.Models.Scope>> GetScopesAsync(bool publicOnly = true)
        {
            Scope[] list = null;
            List<Scope> scopes = null;

            var sql = @"SELECT 
                                * 
                        FROM 
                                SACCORE.T_SCOPE AS A 
                        LEFT JOIN
                                SACCORE.T_SCOPE_CLAIM AS B ON B.SCOPE_ID = A.ID 
                        LEFT JOIN
                                SACCORE.T_SCOPE_SECRET AS C ON C.SCOPE_ID = A.ID ";

            if (publicOnly)
            {
                sql += @"WHERE A.ShowInDiscoveryDocument = @ShowInDiscoveryDocument";

                if (options != null && options.SynchronousReads)
                {
                    list = this.options.Connection
                        .Query<Scope, ScopeClaim, ScopeSecret, Scope>(sql, func, new { ShowInDiscoveryDocument = true }).ToArray();
                }
                else
                {
                    list = (await this.options.Connection
                        .QueryAsync<Scope, ScopeClaim, ScopeSecret, Scope>(sql, func, new { ShowInDiscoveryDocument = true })).ToArray();
                }
            }
            else
            {
                if (options != null && options.SynchronousReads)
                {
                    list = this.options.Connection
                        .Query<Scope, ScopeClaim, ScopeSecret, Scope>(sql, func).ToArray();
                }
                else
                {
                    list = (await this.options.Connection
                        .QueryAsync<Scope, ScopeClaim, ScopeSecret, Scope>(sql, func)).ToArray();
                }
            }

            scopes = Parse(list);

            return scopes.Select(x => x.ToModel());
        }

        private List<Scope> Parse(Scope[] list)
        {
            List<Scope> scopes = new List<Scope>();

            var groups = list.GroupBy(s => s.Id);

            foreach (var group in groups)
            {
                foreach (var scope in group)
                {
                    if (!scopes.Contains(scope))
                    {
                        if (scope.ScopeClaims != null)
                        {
                            scope.ScopeClaims.FirstOrDefault().Scope = scope;
                        }
                        else
                        {
                            scope.ScopeClaims = new Collection<ScopeClaim>();
                        }

                        if (scope.ScopeSecrets != null)
                        {
                            scope.ScopeSecrets.FirstOrDefault().Scope = scope;
                        }
                        else
                        {
                            scope.ScopeSecrets = new Collection<ScopeSecret>();
                        }

                        scopes.Add(scope);
                    }
                    else
                    {
                        var archievedScope = scopes.Where(s => s.Id == scope.Id).FirstOrDefault();

                        if (scope.ScopeClaims != null && archievedScope.ScopeClaims.Contains(scope.ScopeClaims.FirstOrDefault()))
                        {
                            var scopeClaim = scope.ScopeClaims.FirstOrDefault();
                            scopeClaim.Scope = archievedScope;
                            archievedScope.ScopeClaims.Add(scopeClaim);
                        }

                        if (scope.ScopeSecrets != null && archievedScope.ScopeSecrets.Contains(scope.ScopeSecrets.FirstOrDefault()))
                        {
                            var scopeSecret = scope.ScopeSecrets.FirstOrDefault();
                            scopeSecret.Scope = archievedScope;
                            archievedScope.ScopeSecrets.Add(scopeSecret);
                        }
                    }
                }
            }

            return scopes;
        }
    }
}
