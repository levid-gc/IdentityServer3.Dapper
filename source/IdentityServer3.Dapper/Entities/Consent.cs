#region Usings

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace IdentityServer3.Dapper.Entities
{
    public class Consent
    {
        [Key, Column(Order = 0)]
        [StringLength(200)]
        public virtual string Subject { get; set; }

        [Key, Column(Order = 1)]
        [StringLength(200)]
        public virtual string ClientId { get; set; }

        [Required]
        [StringLength(2000)]
        public virtual string Scopes { get; set; }
    }
}
