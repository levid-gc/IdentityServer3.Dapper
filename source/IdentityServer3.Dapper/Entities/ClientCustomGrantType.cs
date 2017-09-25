using System.ComponentModel.DataAnnotations;

namespace IdentityServer3.Dapper.Entities
{
    public class ClientCustomGrantType : ClientCustomGrantType<int>
    { }

    public class ClientCustomGrantType<TKey>
    {
        [Key]
        public virtual TKey Id { get; set; }

        [Required]
        [StringLength(250)]
        public virtual string GrantType { get; set; }

        public virtual Client Client { get; set; }
    }
}
