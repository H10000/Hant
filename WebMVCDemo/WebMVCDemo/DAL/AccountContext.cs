using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebMVCDemo.Models;

namespace WebMVCDemo.DAL
{
    public class AccountContext : DbContext
    {
        public AccountContext()
            : base("AccountContext")
        {

        }
        public DbSet<Student> Student { get; set; }
    }
}