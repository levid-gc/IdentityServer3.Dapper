using System.ComponentModel.DataAnnotations;

namespace IdentityServer3.Dapper.Entities
{
    public class ClientCorsOrigin : ClientCorsOrigin<int>
    { }

    public class ClientCorsOrigin<TKey>
    {
        [Key]
        public virtual TKey Id { get; set; }

        [Required]
        [StringLength(150)]
        public virtual string Origin { get; set; }

        public virtual Client Client { get; set; }
    }
}
