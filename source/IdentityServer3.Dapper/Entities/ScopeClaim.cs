#region Usings

using System.ComponentModel.DataAnnotations;

#endregion

namespace IdentityServer3.Dapper.Entities
{
    public class ScopeClaim
    {
        [Key]
        public virtual string Id { get; set; }

        [Required]
        [StringLength(200)]
        public virtual string Name { get; set; }

        [StringLength(1000)]
        public virtual string Description { get; set; }

        public virtual bool AlwaysIncludeInIdToken { get; set; }

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

            var other = (ScopeClaim)obj;

            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
