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
        /// <summary>
        /// 用户基础信息表
        /// </summary>
        DbSet<sys_user> sys_user { get; set; }

        /// <summary>
        /// 用户授权信息表
        /// </summary>
        DbSet<sys_user_authorization> sys_user_authorization { get; set; }

        /// <summary>
        /// 用户名生产表
        /// </summary>
        DbSet<sys_code_number> sys_code_number { get; set; }

        /// <summary>
        /// 角色表
        /// </summary>
        DbSet<sys_role> sys_role { get; set; }

        /// <summary>
        /// 组织表
        /// </summary>
        DbSet<sys_group> sys_group { get; set; }

        /// <summary>
        /// 用户角色关系表
        /// </summary>
        DbSet<sys_user_role_relation> sys_user_role_relation { get; set; }

        /// <summary>
        /// 用户分组关系表
        /// </summary>
        DbSet<sys_user_group_relation> sys_user_group_relation { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //指定单数形式的表名
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}