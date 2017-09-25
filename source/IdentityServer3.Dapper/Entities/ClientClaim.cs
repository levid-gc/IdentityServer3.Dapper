using System.ComponentModel.DataAnnotations;

namespace IdentityServer3.Dapper.Entities
{
    public class ClientClaim : ClientClaim<int>
    { }

    public class ClientClaim<TKey>
    {
        [Key]
        public virtual TKey Id { get; set; }

        [Required]
        [StringLength(250)]
        public virtual string Type { get; set; }

        [Required]
        [StringLength(250)]
        public virtual string Value { get; set; }

        public virtual Client Client { get; set; }
    }
}
