using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hant.Web.API.DAL.Entity
{
    public class sys_group
    {
        [Key]
        [Required]
        public int ID { get; set; }

        public string OuID { get; set; }

        public string OuName { get; set; }

        public string ParentOuID { get; set; }

        public int Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}