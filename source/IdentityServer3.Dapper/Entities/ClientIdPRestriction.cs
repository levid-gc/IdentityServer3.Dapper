using System.ComponentModel.DataAnnotations;

namespace IdentityServer3.Dapper.Entities
{
    public class ClientIdPRestriction
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        [StringLength(200)]
        public virtual string Provider { get; set; }

        public virtual Client Client { get; set; }

        public virtual int ClientId { get; set; }
    }
}
