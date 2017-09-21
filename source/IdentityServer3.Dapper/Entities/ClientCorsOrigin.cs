﻿#region Usings

using System.ComponentModel.DataAnnotations;

#endregion

namespace IdentityServer3.Dapper.Entities
{
    public class ClientCorsOrigin
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        [StringLength(150)]
        public virtual string Origin { get; set; }

        public virtual Client Client { get; set; }
    }
}
