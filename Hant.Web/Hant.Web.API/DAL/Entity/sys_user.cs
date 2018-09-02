using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hant.Web.API.DAL.Entity
{
    public class sys_user
    {
        [Key]
        [Required]
        public int ID { get; set; }

    }
}