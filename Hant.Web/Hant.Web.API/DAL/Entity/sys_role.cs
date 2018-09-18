using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hant.Web.API.DAL.Entity
{
    public class sys_role
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [StringLength(50)]
        public string RoleID { get; set; }

        [StringLength(50)]
        public string RoleName { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public int Status { get; set; }
    }
}