using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayNexus.NewsServer.Models
{
    public class User
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 用户名字
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 上线时间
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 当前用户所在部门id
        /// </summary>
        public string DepartmentID { get; set; }

        /// <summary>
        /// signalr 链接id
        /// </summary>
        public string ConnectionId { get; set; }

        public string DeviceType
        {
            get
           ;
            set
            ;
        }

    }
}
