using System.ComponentModel.DataAnnotations;

namespace IdentityServer3.Dapper.Entities
{
    public class ClientIdPRestriction : ClientIdPRestriction<int>
    { }

    public class ClientIdPRestriction<TKey>
    {
        [Key]
        public virtual TKey Id { get; set; }

        [Required]
        [StringLength(200)]
        public virtual string Provider { get; set; }

        public virtual Client Client { get; set; }
    }
}
