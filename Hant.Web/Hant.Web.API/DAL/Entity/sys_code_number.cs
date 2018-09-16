using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hant.Web.API.DAL.Entity
{
    public class sys_code_number
    {
        [Key]
        [Required]
        public int ID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public int Number { get; set; }

        public int Mark { get; set; }
    }
}