using System.ComponentModel.DataAnnotations;

namespace IdentityServer3.Dapper.Entities
{
    public class ClientScope : ClientScope<int>
    { }

    public class ClientScope<TKey>
    {
        [Key]
        public virtual TKey Id { get; set; }

        [Required]
        [StringLength(200)]
        public virtual string Scope { get; set; }

        public virtual Client Client { get; set; }
    }
}
