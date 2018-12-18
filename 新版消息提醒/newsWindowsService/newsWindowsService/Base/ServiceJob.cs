using System;

namespace TimerManageRunningService.Base
{
    /// <summary>
    /// 工作项
    /// </summary>
    public abstract class ServiceJob
    {
        //配置对象
        private ServiceConfig mConfigObject;
        //下次运行时间
        private DateTime mNextTime;
        //任务是否在运行中
        protected bool mIsRunning;
        //用来让定时执行的服务刚运行的时候不执行
        private bool fristtrun = false;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ServiceJob()
        {
            //变量初始化
            this.mNextTime = DateTime.Now;
            this.mIsRunning = true;
        }

        /// <summary>
        /// 配置对象
        /// </summary>
        public ServiceConfig ConfigObject
        {
            get { return this.mConfigObject; }
            set { this.mConfigObject = value; }
        }

        /// <summary>
        /// 开始工作
        /// </summary>
        public void StartJob()
        {
            try
            {
                if (this.mIsRunning)
                {
                    if (this.mConfigObject != null && this.mNextTime != null)
                    {
                        if (this.mConfigObject.Enabled.ToLower() == "true" && Convert.ToDateTime(this.mConfigObject.startTime) <= DateTime.Now)
                        {
                            if (DateTime.Now >= this.mNextTime)
                            {
                                if (this.mConfigObject.runningType == 1)
                                {
                                    fristtrun = true;
                                    this.mNextTime = DateTime.Now.AddSeconds((double)this.mConfigObject.intervalTime);
                                }
                                else if (this.mConfigObject.runningType == 0)
                                {
                                    DateTime runningdaytime = DateTime.MinValue;
                                    DateTime tempDatetime = DateTime.MinValue;
                                    if (this.mConfigObject.intervalType == 1)
                                    {
                                        if (this.mConfigObject.runningDate == -1)
                                        {
                                            tempDatetime = Convert.ToDateTime(DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd") + " " + this.mConfigObject.runningTime);
                                            runningdaytime = tempDatetime <= DateTime.Now ? tempDatetime.AddMonths(1) : tempDatetime;
                                        }
                                        else
                                        {
                                            tempDatetime = Convert.ToDateTime(DateTime.Now.AddDays(1 - DateTime.Now.Day).AddDays(this.mConfigObject.runningDate - 1).ToString("yyyy-MM-dd") + " " + this.mConfigObject.runningTime);
                                            runningdaytime = tempDatetime <= DateTime.Now ? tempDatetime.AddMonths(1) : tempDatetime;
                                        }
                                    }
                                    else if (this.mConfigObject.intervalType == 2)
                                    {
                                        tempDatetime = Convert.ToDateTime
                                            (DateTime.Now.AddDays(this.mConfigObject.runningDate - Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d")) < 0
                                            ? 7 - (this.mConfigObject.runningDate - Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d")))
                                            : this.mConfigObject.runningDate - Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"))).ToString("yyyy-MM-dd") + " " + this.mConfigObject.runningTime
                                            );
                                        runningdaytime = tempDatetime <= DateTime.Now ? tempDatetime.AddDays(7) : tempDatetime;
                                    }
                                    else if (this.mConfigObject.intervalType == 3)
                                    {
                                        tempDatetime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " " + this.mConfigObject.runningTime);
                                        runningdaytime = tempDatetime <= DateTime.Now ? tempDatetime.AddDays(1) : tempDatetime;
                                    }
                                    this.mNextTime = runningdaytime;
                                }
                                if (fristtrun)
                                {
                                    this.Start();
                                }
                                fristtrun = true;
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                CommonLog.WriteLog("运行信息", ex.ToString());
                StopJob();
            }
        }

        /// <summary>
        /// 停止工作
        /// </summary>
        public void StopJob()
        {
            this.mConfigObject = null;
            this.mNextTime = DateTime.Now;
            this.mIsRunning = false;
            this.Stop();
        }

        #region 子类必需实现的抽象成员

        /// <summary>
        /// 开始工作
        /// </summary>
        protected abstract void Start();

        /// <summary>
        /// 停止工作
        /// </summary>
        protected abstract void Stop();

        #endregion

        public void WriteLogInfo(string strLog)
        {
            CommonLog.WriteLog(mConfigObject.Description, strLog);
        }
    }
}
