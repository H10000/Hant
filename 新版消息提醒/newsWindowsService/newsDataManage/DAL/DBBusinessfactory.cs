using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newsDataManage.DAL
{
    public class DBBusinessfactory
    {
        public DBBusiness dbbusiness;
        public DBBusinessfactory()
        {
            string is_formal = System.Configuration.ConfigurationManager.AppSettings["is_formal"] == null ? 0.ToString() : System.Configuration.ConfigurationManager.AppSettings["is_formal"];
            switch (is_formal)
            {
                default://默认接口样式
                    dbbusiness = new DBBusiness();
                    break;
            }
        }
    }
}
