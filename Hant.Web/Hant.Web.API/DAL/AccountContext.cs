using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Hant.Web.API.DAL
{
    public class AccountContext:DbContext
    {
        public AccountContext()
           : base("AccountContext")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //指定单数形式的表名
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}