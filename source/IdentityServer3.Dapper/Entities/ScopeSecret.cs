#region Usings

using System;
using System.ComponentModel.DataAnnotations;

#endregion

namespace IdentityServer3.Dapper.Entities
{
    public class ScopeSecret
    {
        [Key]
        public virtual string Id { get; set; }

        [StringLength(1000)]
        public virtual string Description { get; set; }

        public virtual DateTimeOffset? Expiration { get; set; }

        [StringLength(250)]
        public virtual string Type { get; set; }

        [Required]
        [StringLength(250)]
        public virtual string Value { get; set; }

        public virtual Scope Scope { get; set; }

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

            var other = (ScopeSecret)obj;

            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
