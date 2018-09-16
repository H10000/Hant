using Hant.Web.API.DAL.Entity;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Hant.Web.API.DAL
{
    public class AccountContext : DbContext
    {
        public AccountContext()
           : base("AccountContext")
        {

        }
        DbSet<sys_user> sys_user { get; set; }

        DbSet<sys_user_authorization> sys_user_authorization { get; set; }

        DbSet<sys_code_number> sys_code_number { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //指定单数形式的表名
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}