using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebClient.Helper
{
    public class DBHelper
    {
        protected string connUrl = "";
        protected string providerName = "";
        protected SqlConnection myConn;
        protected SqlDataAdapter myAdapter;
        protected OracleConnection myOrcConn;
        protected OracleDataAdapter myOrcAdapter;
        //protected MySqlConnection myMyConn;
        //protected MySqlDataAdapter myMyAdapter;
        protected OleDbConnection myDB2Conn;
        protected OleDbDataAdapter myBD2Adapter;

        #region 构造函数
        public DBHelper()
        {
            string Conn = "DBContext";
            connUrl = ConfigurationManager.ConnectionStrings[Conn].ToString();
            providerName = ConfigurationManager.ConnectionStrings[Conn].ProviderName;
        }
        public DBHelper(string Conn)
        {
            try
            {
                connUrl = ConfigurationManager.ConnectionStrings[Conn].ToString();
                providerName = ConfigurationManager.ConnectionStrings[Conn].ProviderName;
            }
            catch
            { }
        }
        #endregion

        #region 开关sql server连接
        private void OpenConnection()
        {
            if (myConn == null)
                myConn = new SqlConnection(connUrl);
            if (myConn.State == ConnectionState.Closed)
                myConn.Open();
        }

        private void CloseConnection()
        {
            if (myConn != null && myConn.State == ConnectionState.Open)
                myConn.Close();
        }
        #endregion

        #region 开关oracle连接
        private void OpenOracleConnection()
        {
            if (myOrcConn == null)
            {
                myOrcConn = new OracleConnection(connUrl);
            }
            if (myOrcConn.State == ConnectionState.Closed)
            {
                myOrcConn.Open();
            }
        }

        private void CloseOracleConnection()
        {
            if (myOrcConn != null && myOrcConn.State == ConnectionState.Open)
            {
                myOrcConn.Close();
            }
        }
        #endregion

        #region 开关MySql连接
        //private void OpenMyConnection()
        //{
        //    if (myMyConn == null)
        //    {
        //        myMyConn = new MySqlConnection(connUrl);
        //    }
        //    if (myMyConn.State == ConnectionState.Closed)
        //    {
        //        myMyConn.Open();
        //    }
        //}

        //private void CloseMyConnection()
        //{
        //    if (myMyConn != null && myMyConn.State == ConnectionState.Open)
        //    {
        //        myMyConn.Close();
        //    }
        //}
        #endregion

        #region 开关DB2连接
        private void OpenDB2Connection()
        {
            if (myDB2Conn == null)
            {
                myDB2Conn = new OleDbConnection(connUrl);
            }
            if (myDB2Conn.State == ConnectionState.Closed)
            {
                myDB2Conn.Open();
            }
        }

        private void CloseBD2Connection()
        {
            if (myDB2Conn != null && myDB2Conn.State == ConnectionState.Open)
            {
                myDB2Conn.Close();
            }
        }
        #endregion

        #region 通用操作数据库方法
        public int MyExecuteInt(string sql)
        {
            if (providerName == "System.Data.SqlClient")
            {
                return mySqlExecuteNonQuery(sql);
            }
            else if (providerName == "Oracle.ManagedDataAccess.Client")
            {
                return myOrcExecuteNonQuery(sql);
            }
            //else if (providerName == "MySql.Data.MySqlClient")
            //{

            //    return myMyExecuteNonQuery(sql);
            //}
            else
            {
                return myBD2ExecuteNonQuery(sql);
            }
        }

        public DataTable MyExecuteTable(string sql)
        {
            if (providerName == "System.Data.SqlClient")
            {
                return mySqlExecuteQuery(sql);
            }
            else if (providerName == "Oracle.ManagedDataAccess.Client")
            {
                return myOrcExecuteQuery(sql);
            }
            //else if (providerName == "MySql.Data.MySqlClient")
            //{
            //    return myMyExecuteQuery(sql);
            //}
            else
            {
                return myDB2ExecuteQuery(sql);
            }
        }

        #endregion

        #region sqlserver数据库相关操作
        /// <summary>
        /// 返回操作的数据数
        /// </summary>
        /// <param name="sql"></param>
        private int mySqlExecuteNonQuery(string sql)
        {
            OpenConnection();
            myAdapter = new SqlDataAdapter(sql, myConn);
            int i = myAdapter.SelectCommand.ExecuteNonQuery();
            CloseConnection();
            return i;
        }
        /// <summary>
        /// 返回查询得到的DataTable数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private DataTable mySqlExecuteQuery(string sql)
        {
            OpenConnection();
            myAdapter = new SqlDataAdapter(sql, myConn);
            DataTable dt = new DataTable();
            myAdapter.Fill(dt);
            CloseConnection();
            return dt;
        }
        #endregion

        #region oracle数据库相关操作
        /// <summary>
        /// 返回操作的数据数
        /// </summary>
        /// <param name="sql"></param>
        private int myOrcExecuteNonQuery(string sql)
        {
            OpenOracleConnection();
            myOrcAdapter = new OracleDataAdapter(sql, myOrcConn);
            CloseOracleConnection();
            return myOrcAdapter.SelectCommand.ExecuteNonQuery();
        }
        /// <summary>
        /// 返回查询得到的DataTable数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private DataTable myOrcExecuteQuery(string sql)
        {
            OpenOracleConnection();
            myOrcAdapter = new OracleDataAdapter(sql, myOrcConn);
            DataTable dt = new DataTable();
            myOrcAdapter.Fill(dt);
            CloseOracleConnection();
            return dt;
        }
        #endregion

        #region MySql数据库相关操作
        /// <summary>
        /// 返回操作的数据数
        /// </summary>
        /// <param name="sql"></param>
        //private int myMyExecuteNonQuery(string sql)
        //{
        //    OpenMyConnection();
        //    myMyAdapter = new MySqlDataAdapter(sql, myMyConn);
        //    CloseMyConnection();
        //    return myMyAdapter.SelectCommand.ExecuteNonQuery();
        //}
        ///// <summary>
        ///// 返回查询得到的DataTable数据
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <returns></returns>
        //private DataTable myMyExecuteQuery(string sql)
        //{
        //    OpenMyConnection();
        //    myMyAdapter = new MySqlDataAdapter(sql, myMyConn);
        //    DataTable dt = new DataTable();
        //    myMyAdapter.Fill(dt);
        //    CloseMyConnection();
        //    return dt;
        //}
        #endregion

        #region DB2数据库相关操作
        /// <summary>
        /// 返回操作的数据数
        /// </summary>
        /// <param name="sql"></param>
        private int myBD2ExecuteNonQuery(string sql)
        {
            OpenDB2Connection();
            myBD2Adapter = new OleDbDataAdapter(sql, myDB2Conn);
            CloseBD2Connection();
            return myBD2Adapter.SelectCommand.ExecuteNonQuery();
        }
        /// <summary>
        /// 返回查询得到的DataTable数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private DataTable myDB2ExecuteQuery(string sql)
        {
            OpenDB2Connection();
            myBD2Adapter = new OleDbDataAdapter(sql, myDB2Conn);
            DataTable dt = new DataTable();
            myBD2Adapter.Fill(dt);
            CloseBD2Connection();
            return dt;
        }
        #endregion
    }
}