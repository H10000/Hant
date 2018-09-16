using System;
using System.ComponentModel.DataAnnotations;

namespace Hant.Web.API.DAL.Entity
{
    public class sys_user
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(64)]
        public string Email { get; set; }

        public string Photo { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        public int Status { get; set; }
    }
}