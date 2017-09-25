using System.ComponentModel.DataAnnotations;

namespace IdentityServer3.Dapper.Entities
{
    public class ScopeClaim : ScopeClaim<int>
    { }

    public class ScopeClaim<TKey>
    {
        [Key]
        public virtual TKey Id { get; set; }

        [Required]
        [StringLength(200)]
        public virtual string Name { get; set; }

        [StringLength(1000)]
        public virtual string Description { get; set; }

        public virtual bool AlwaysIncludeInIdToken { get; set; }

        public virtual Scope Scope { get; set; }
    }
}
