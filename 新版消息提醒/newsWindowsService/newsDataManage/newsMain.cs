using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimerManageRunningService.Base;

namespace newsDataManage
{
    #region 未执行医嘱
    public class UnExtOrders : ServiceJob
    {
        /// <summary>
        /// 任务开始
        /// </summary>
        protected override void Start()
        {
            try
            {
                //执行工作项
                new BLL.Businessfactory().Business.UnExtOrdersMethod();
            }
            catch (Exception error)
            {
                WriteLogInfo(error.ToString());
            }
            finally
            {
                WriteLogInfo("结束");
                //空闲
            }
        }

        /// <summary>
        /// 任务停止
        /// </summary>
        protected override void Stop()
        {
            this.mIsRunning = false;
        }
    }
    #endregion

    #region 新医嘱
    public class newOrders : ServiceJob
    {
        /// <summary>
        /// 任务开始
        /// </summary>
        protected override void Start()
        {
            try
            {
                //执行工作项
                new BLL.Businessfactory().Business.newOrdersMethod();
            }
            catch (Exception error)
            {
                WriteLogInfo(error.ToString());
            }
            finally
            {
                WriteLogInfo("结束");
                //空闲
            }
        }

        /// <summary>
        /// 任务停止
        /// </summary>
        protected override void Stop()
        {
            this.mIsRunning = false;
        }
    }
    #endregion

    #region 三天未大便
    public class unThreeStool : ServiceJob
    {
        /// <summary>
        /// 任务开始
        /// </summary>
        protected override void Start()
        {
            try
            {
                //执行工作项
                new BLL.Businessfactory().Business.unThreeStoolMethod();
            }
            catch (Exception error)
            {
                WriteLogInfo(error.ToString());
            }
            finally
            {
                WriteLogInfo("结束");
                //空闲
            }
        }

        /// <summary>
        /// 任务停止
        /// </summary>
        protected override void Stop()
        {
            this.mIsRunning = false;
        }
    }
    #endregion

    #region 停止医嘱
    public class stopOrders : ServiceJob
    {
        /// <summary>
        /// 任务开始
        /// </summary>
        protected override void Start()
        {
            try
            {
                //执行工作项
                new BLL.Businessfactory().Business.stopOrdersMethod();
            }
            catch (Exception error)
            {
                WriteLogInfo(error.ToString());
            }
            finally
            {
                WriteLogInfo("结束");
                //空闲
            }
        }

        /// <summary>
        /// 任务停止
        /// </summary>
        protected override void Stop()
        {
            this.mIsRunning = false;
        }
    }
    #endregion

    #region 检查检验报告
    public class inspectionReport : ServiceJob
    {
        /// <summary>
        /// 任务开始
        /// </summary>
        protected override void Start()
        {
            try
            {
                //执行工作项
                new BLL.Businessfactory().Business.inspectionReportMethod();
            }
            catch (Exception error)
            {
                WriteLogInfo(error.ToString());
            }
            finally
            {
                WriteLogInfo("结束");
                //空闲
            }
        }

        /// <summary>
        /// 任务停止
        /// </summary>
        protected override void Stop()
        {
            this.mIsRunning = false;
        }
    }
    #endregion

    #region 当日手术提醒
    public class opetationInfo : ServiceJob
    {
        /// <summary>
        /// 任务开始
        /// </summary>
        protected override void Start()
        {
            try
            {
                //执行工作项
                new BLL.Businessfactory().Business.opetationInfoMethod();
            }
            catch (Exception error)
            {
                WriteLogInfo(error.ToString());
            }
            finally
            {
                WriteLogInfo("结束");
                //空闲
            }
        }

        /// <summary>
        /// 任务停止
        /// </summary>
        protected override void Stop()
        {
            this.mIsRunning = false;
        }
    }
    #endregion

    #region 转科提醒
    public class zhuanruNews : ServiceJob
    {
        /// <summary>
        /// 任务开始
        /// </summary>
        protected override void Start()
        {
            try
            {
                //执行工作项
                new BLL.Businessfactory().Business.zhuanruNewsMethod();
            }
            catch (Exception error)
            {
                WriteLogInfo(error.ToString());
            }
            finally
            {
                WriteLogInfo("结束");
                //空闲
            }
        }

        /// <summary>
        /// 任务停止
        /// </summary>
        protected override void Stop()
        {
            this.mIsRunning = false;
        }
    }
    #endregion

    #region 同步his医嘱信息
    public class TongBuHisOrders : ServiceJob
    {
        /// <summary>
        /// 任务开始
        /// </summary>
        protected override void Start()
        {
            try
            {
                //执行工作项
                new BLL.Businessfactory().Business.TongBuHisOrder();
            }
            catch (Exception error)
            {
                WriteLogInfo(error.ToString());
            }
            finally
            {
                WriteLogInfo("结束");
                //空闲
            }
        }

        /// <summary>
        /// 任务停止
        /// </summary>
        protected override void Stop()
        {
            this.mIsRunning = false;
        }
    }
    #endregion

    #region 摆 配 执行 医嘱
    public class unPutDispensedCheck : ServiceJob
    {
        /// <summary>
        /// 任务开始
        /// </summary>
        protected override void Start()
        {
            try
            {
                //执行工作项
                new BLL.Businessfactory().Business.UnPutMethod();
                new BLL.Businessfactory().Business.UnDispensedMethod();
                new BLL.Businessfactory().Business.UnCheckMethod();
            }
            catch (Exception error)
            {
                WriteLogInfo(error.ToString());
            }
            finally
            {
                WriteLogInfo("结束");
                //空闲
            }
        }

        /// <summary>
        /// 任务停止
        /// </summary>
        protected override void Stop()
        {
            this.mIsRunning = false;
        }
    }
    #endregion
}
