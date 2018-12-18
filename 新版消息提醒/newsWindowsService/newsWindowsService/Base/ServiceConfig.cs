using System;
using System.Collections.Specialized;

namespace TimerManageRunningService.Base
{
    /// <summary>
    /// 工作项配置
    /// </summary>
    public class ServiceConfig
    {
        #region 属性

        /// <summary>
        /// 工作项说明
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 工作项是否开启
        /// </summary>
        public string Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// 工作项程序集
        /// </summary>
        public string Assembly
        {
            get;
            set;
        }
       
        /// <summary>
        /// 执行模式
        /// </summary>
        public int runningType
        {
            get;
            set;
        }
        /// <summary>
        /// 执行开始时间（大于这个时间才会执行）
        /// </summary>
        public string startTime
        {
            get;
            set;
        }
        /// <summary>
        /// 固定时间执行间隔（1为月，2为星期，3为天）
        /// </summary>
        public int intervalType
        {
            get;
            set;
        }

        /// <summary>
        /// 执行日期
        /// </summary>
        public int runningDate
        {
            get;
            set;
        }

        /// <summary>
        /// 执行时间
        /// </summary>
        public string runningTime
        {
            get;
            set;
        }
        /// <summary>
        /// 工作项执行间隔时间
        /// </summary>
        public int intervalTime
        {
            get;
            set;
        }
        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数，将配置项加载进对象
        /// </summary>
        public ServiceConfig(string job)
        {
            NameValueCollection nvc = Base.ServiceTools.GetSection(job);

            foreach (string s in nvc.Keys)
            {
                switch (s.ToLower())
                {
                    //基本
                    case "description":
                        this.Description = nvc[s].ToString();
                        break;
                    case "enabled":
                        this.Enabled = nvc[s].ToString();
                        break;
                    case "assembly":
                        this.Assembly = nvc[s].ToString();
                        break;
                    case "runningtype":
                        this.runningType = int.Parse(nvc[s].ToString());
                        break;
                    case "starttime":
                        this.startTime = nvc[s].ToString();
                        break;
                    case "intervaltype":
                        this.intervalType = int.Parse(nvc[s].ToString());
                        break;
                    case "runningdate":
                        this.runningDate = int.Parse(nvc[s].ToString());
                        break;
                    case "runningtime":
                        this.runningTime = nvc[s].ToString();
                        break;
                    case "intervaltime":
                        this.intervalTime = int.Parse(nvc[s].ToString());
                        break;
                }
            }
        }

        #endregion
    }
}
