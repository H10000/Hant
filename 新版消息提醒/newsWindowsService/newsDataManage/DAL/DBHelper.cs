using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Reflection;
using Oracle.ManagedDataAccess.Client;
using MySql.Data.MySqlClient;
using System.Data.OleDb;

namespace newsDataManage.DAL
{
    public class DBHelper
    {
        protected string connUrl = "";
        protected string providerName = "";
        protected SqlConnection myConn;
        protected SqlDataAdapter myAdapter;
        protected OracleConnection myOrcConn;
        protected OracleDataAdapter myOrcAdapter;
        protected MySqlConnection myMyConn;
        protected MySqlDataAdapter myMyAdapter;
        protected OleDbConnection myDB2Conn;
        protected OleDbDataAdapter myBD2Adapter; 

        #region 构造函数
        public DBHelper()
        {
            string Conn = "MNSContext";
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
        private void OpenMyConnection()
        {
            if (myMyConn == null)
            {
                myMyConn = new MySqlConnection(connUrl);
            }
            if (myMyConn.State == ConnectionState.Closed)
            {
                myMyConn.Open();
            }
        }

        private void CloseMyConnection()
        {
            if (myMyConn != null && myMyConn.State == ConnectionState.Open)
            {
                myMyConn.Close();
            }
        }
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
        public int MyExecute(string sql)
        {
            if (providerName == "System.Data.SqlClient")
            {
                return mySqlExecuteNonQuery(sql);
            }
            else if (providerName == "Oracle.ManagedDataAccess.Client")
            {
                return myOrcExecuteNonQuery(sql);
            }
            else if (providerName == "MySql.Data.MySqlClient")
            {

                return myMyExecuteNonQuery(sql); 
            }
            else 
            {
                return myBD2ExecuteNonQuery(sql);
            }
        }

        public DataTable MyExecuteQuery(string sql)
        {
            if (providerName == "System.Data.SqlClient")
            {
                return mySqlExecuteQuery(sql);
            }
            else if (providerName == "Oracle.ManagedDataAccess.Client")
            {
                return myOrcExecuteQuery(sql);
            }
            else if (providerName == "MySql.Data.MySqlClient")
            {
                return myMyExecuteQuery(sql);
            }
            else
            {
                return myDB2ExecuteQuery(sql);
            }
        }
        #endregion

        #region sql server数据库相关操作

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public object GetSingle(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        #region 准备执行sql语句
        public void PrepareSqlString(string sql)
        {
            OpenConnection();
            myAdapter = new SqlDataAdapter(sql, myConn);

        }
        #endregion

        #region 传递sql语句的参数
        public void SetParameter(string parameterName, object parameterValue)
        {
            parameterName = "@" + parameterName.Trim();
            if (parameterValue == null)
                parameterValue = DBNull.Value;
            myAdapter.SelectCommand.Parameters.Add(new SqlParameter(parameterName, parameterValue));
        }
        #endregion

        #region 执行一个非查询的sql语句,返回影响的条数
        public int Execute()
        {
            int rowCount = myAdapter.SelectCommand.ExecuteNonQuery();
            //OperationLog();
            CloseConnection();
            return rowCount;
        }
        #endregion

        #region 执行一个查询的sql语句,返回DataTable
        public DataTable ExecuteQuery()
        {
            DataTable dt = new DataTable();
            myAdapter.Fill(dt);
            CloseConnection();
            return dt;
        }
        #endregion

        #region 执行一个查询的sql语句，返回第一行第一列的值
        public object ExecuteScalar()
        {
            Object obj = myAdapter.SelectCommand.ExecuteScalar();
            CloseConnection();
            return obj;
        }
        #endregion

        #region 复杂的非查询语句直接传递sql语句进来
        public int mySqlExecuteNonQuery(string sql)
        {
            this.PrepareSqlString(sql);
            return this.Execute();
        }
        #endregion

        #region 复杂的查询语句直接传递sql语句进来
        public DataTable mySqlExecuteQuery(string sql)
        {
            this.PrepareSqlString(sql);
            DataTable dt = this.ExecuteQuery();
            return dt;
        }
        #endregion

        #region 查询语句直接传递sql语句，返回第一行第一列
        public object MyExecuteScalar(string sql)
        {
            this.PrepareSqlString(sql);
            return this.ExecuteScalar();
        }
        #endregion

        #region 根据sql语句查询记录集的数量
        public int GetCount(string sqlCount)
        {
            this.PrepareSqlString(sqlCount);
            return Convert.ToInt32(this.ExecuteScalar());
        }
        #endregion

        #region 返回查询参数的字符串拼接
        public string GetParametersValues()
        {
            string str = "参数为：";
            for (int i = 0; i < myAdapter.SelectCommand.Parameters.Count; i++)
            {
                str += myAdapter.SelectCommand.Parameters[i].Value.ToString() + ",";
            }
            return str;
        }
        #endregion

        /// 执行多条SQL语句，实现数据库事务。
        ///多条SQL语句
        ///影响的记录数
        public int ExecuteSqlTran(List<String> SQLStringList)
        {
            SqlConnection sqlConn = new SqlConnection(connUrl);//创建数据库连接
            sqlConn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConn;
            SqlTransaction tx = sqlConn.BeginTransaction();
            cmd.Transaction = tx;

            try
            {
                int count = 0;
                for (int n = 0; n < SQLStringList.Count; n++)
                {
                    string strsql = SQLStringList[n];
                    if (strsql.Trim().Length > 1)
                    {
                        cmd.CommandText = strsql;
                        int nRet = cmd.ExecuteNonQuery();
                        count++;
                    }
                }

                tx.Commit();
                return count;
            }
            catch
            {
                tx.Rollback();
                return -1;
            }
        }
        #endregion

        #region oracle数据库相关操作
        /// <summary>
        /// 返回操作的数据数
        /// </summary>
        /// <param name="sql"></param>
        public int myOrcExecuteNonQuery(string sql)
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
        public DataTable myOrcExecuteQuery(string sql)
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
        public int myMyExecuteNonQuery(string sql)
        {
            OpenMyConnection();
            myMyAdapter = new MySqlDataAdapter(sql, myMyConn);
            CloseMyConnection();
            return myMyAdapter.SelectCommand.ExecuteNonQuery();
        }
        /// <summary>
        /// 返回查询得到的DataTable数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable myMyExecuteQuery(string sql)
        {
            OpenMyConnection();
            myMyAdapter = new MySqlDataAdapter(sql, myMyConn);
            DataTable dt = new DataTable();
            myMyAdapter.Fill(dt);
            CloseMyConnection();
            return dt;
        }
        #endregion

        #region DB2数据库相关操作
        /// <summary>
        /// 返回操作的数据数
        /// </summary>
        /// <param name="sql"></param>
        public int myBD2ExecuteNonQuery(string sql)
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
        public DataTable myDB2ExecuteQuery(string sql)
        {
            OpenDB2Connection();
            myBD2Adapter = new OleDbDataAdapter(sql, myDB2Conn);
            DataTable dt = new DataTable();
            myBD2Adapter.Fill(dt);
            CloseBD2Connection();
            return dt;
        }
        #endregion

        #region 利用反射将DataTable自动转换成泛型集合
        public List<T> ToList<T>(DataTable dt) where T : class,new()
        {
            Type t = typeof(T);
            PropertyInfo[] propertys = t.GetProperties();
            List<T> lst = new List<T>();
            string typeName = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                T entity = new T();
                foreach (PropertyInfo pi in propertys)
                {
                    typeName = pi.Name;
                    if (dt.Columns.Contains(typeName))
                    {

                        if (!pi.CanWrite) continue;
                        object value = dr[typeName];
                        if (value == DBNull.Value) continue;
                        if (pi.PropertyType == typeof(string))
                        {
                            pi.SetValue(entity, value.ToString(), null);
                        }
                        else if (pi.PropertyType == typeof(int) || pi.PropertyType == typeof(int?))
                        {
                            pi.SetValue(entity, int.Parse(value.ToString()), null);
                        }
                        else if (pi.PropertyType == typeof(byte) || pi.PropertyType == typeof(byte?))
                        {
                            pi.SetValue(entity, byte.Parse(value.ToString()), null);
                        }
                        else if (pi.PropertyType == typeof(DateTime?) || pi.PropertyType == typeof(DateTime))
                        {
                            pi.SetValue(entity, DateTime.Parse(value.ToString()), null);
                        }
                        else if (pi.PropertyType == typeof(float) || pi.PropertyType == typeof(float?))
                        {
                            pi.SetValue(entity, float.Parse(value.ToString()), null);
                        }
                        else if (pi.PropertyType == typeof(double) || pi.PropertyType == typeof(double?))
                        {
                            pi.SetValue(entity, double.Parse(value.ToString()), null);
                        }
                        else
                        {
                            pi.SetValue(entity, value, null);
                        }
                    }
                }
                lst.Add(entity);
            }
            return lst;
        }
        #endregion

        #region 利用反射将DataTable自动转换成泛型集合(用于webservice,emptyornull转换成" ")
        public List<T> ToServiceList<T>(DataTable dt) where T : class,new()
        {
            Type t = typeof(T);
            PropertyInfo[] propertys = t.GetProperties();
            List<T> lst = new List<T>();
            string typeName = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                T entity = new T();
                foreach (PropertyInfo pi in propertys)
                {
                    typeName = pi.Name;
                    if (dt.Columns.Contains(typeName))
                    {
                        if (!pi.CanWrite) continue;
                        object value = dr[typeName];
                        if (value == DBNull.Value)
                        {
                            //continue;
                            pi.SetValue(entity, " ", null);
                            continue;
                        }
                        else
                        {
                            if (pi.PropertyType == typeof(string))
                            {
                                pi.SetValue(entity, value.ToString(), null);
                            }
                            else if (pi.PropertyType == typeof(int) || pi.PropertyType == typeof(int?))
                            {
                                pi.SetValue(entity, int.Parse(value.ToString()), null);
                            }
                            else if (pi.PropertyType == typeof(byte) || pi.PropertyType == typeof(byte?))
                            {
                                pi.SetValue(entity, byte.Parse(value.ToString()), null);
                            }
                            else if (pi.PropertyType == typeof(DateTime?) || pi.PropertyType == typeof(DateTime))
                            {
                                pi.SetValue(entity, DateTime.Parse(value.ToString()), null);
                                // pi.SetValue(entity, value.ToString(), null);
                            }
                            else if (pi.PropertyType == typeof(float) || pi.PropertyType == typeof(float?))
                            {
                                pi.SetValue(entity, float.Parse(value.ToString()), null);
                            }
                            else if (pi.PropertyType == typeof(double) || pi.PropertyType == typeof(double?))
                            {
                                pi.SetValue(entity, double.Parse(value.ToString()), null);
                            }
                            else
                            {
                                pi.SetValue(entity, value, null);
                            }
                            if (pi.GetValue(entity, null).ToString().Trim() == "")
                                pi.SetValue(entity, " ", null); ;
                        }
                    }
                }
                lst.Add(entity);
            }
            return lst;
        }
        #endregion
    }
}