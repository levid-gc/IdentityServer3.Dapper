using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer3.Dapper.Entities
{
    public class ScopeSecret : ScopeSecret<int>
    { }

    public class ScopeSecret<TKey>
    {
        [Key]
        public virtual TKey Id { get; set; }

        [StringLength(1000)]
        public virtual string Description { get; set; }

        public virtual DateTimeOffset? Expiration { get; set; }

        [StringLength(250)]
        public virtual string Type { get; set; }

        [Required]
        [StringLength(250)]
        public virtual string Value { get; set; }

        public virtual Scope Scope { get; set; }
    }
}
