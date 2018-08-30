using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Hant.Frame.Helper
{
    //每次数据模型改变的时候都会drop and re-create数据库
    class AccountInitilizer:DropCreateDatabaseIfModelChanges<AccountContext>
    {
        protected override void Seed(AccountContext context)
        {
            base.Seed(context);
        }
    }
}
