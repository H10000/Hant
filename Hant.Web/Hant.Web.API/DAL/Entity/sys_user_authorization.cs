using System;
using System.ComponentModel.DataAnnotations;

namespace Hant.Web.API.DAL.Entity
{
    public class sys_user_authorization
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        [StringLength(50)]
        public string LoginName { get; set; }

        [StringLength(256)]
        public string LoginPwd { get; set; }

        [StringLength(20)]
        public string Salt { get; set; }

        public int LoginType { get; set; }

        public int Status { get; set; }

        public DateTime? EffectiveTime { get; set; }

        public DateTime? InvalidTime { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }
    }
}