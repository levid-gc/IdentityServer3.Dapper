#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace IdentityServer3.Dapper.Entities
{
    public class Scope
    {
        [Key]
        public virtual string Id { get; set; }

        public virtual bool Enabled { get; set; }

        [Required]
        [StringLength(200)]
        public virtual string Name { get; set; }

        [StringLength(200)]
        public virtual string DisplayName { get; set; }

        [StringLength(1000)]
        public virtual string Description { get; set; }

        public virtual bool Required { get; set; }

        public virtual bool Emphasize { get; set; }

        public virtual int Type { get; set; }

        public virtual bool IncludeAllClaimsForUser { get; set; }

        [StringLength(200)]
        public virtual string ClaimsRule { get; set; }

        public virtual bool ShowInDiscoveryDocument { get; set; }

        public virtual bool AllowUnrestrictedIntrospection { get; set; }

        public virtual ICollection<ScopeClaim> ScopeClaims { get; set; }

        public virtual ICollection<ScopeSecret> ScopeSecrets { get; set; }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Scope)obj;

            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
