﻿using System.ComponentModel.DataAnnotations;

namespace IdentityServer3.Dapper.Entities
{
    public class ClientPostLogoutRedirectUri
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        [StringLength(2000)]
        public virtual string Uri { get; set; }

        public virtual Client Client { get; set; }
    }
}
