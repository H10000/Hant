using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newsDataManage.BLL
{
    public class Businessfactory
    {
        public Business Business;
        public Businessfactory()
        {
            string is_formal = System.Configuration.ConfigurationManager.AppSettings["is_formal"] == null ? 0.ToString() : System.Configuration.ConfigurationManager.AppSettings["is_formal"];
            switch (is_formal)
            { 
                default://默认接口样式
                    Business = new Business();
                    break;
            }
        }
    }
}
