using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BayNexus.NewsServer.Helper
{
    public class StaticMethod<T> where T : new()
    {
        #region DataTable转换成实体类

        /// <summary>
        /// 填充对象列表：用DataSet的第一个表填充实体类
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <returns></returns>
        public static List<T> FillModel(DataSet ds)
        {
            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return FillModel(ds.Tables[0]);
            }
        }

        /// <summary>  
        /// 填充对象列表：用DataSet的第index个表填充实体类
        /// </summary>  
        public static List<T> FillModel(DataSet ds, int index)
        {
            if (ds == null || ds.Tables.Count <= index || ds.Tables[index].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return FillModel(ds.Tables[index]);
            }
        }

        /// <summary>  
        /// 填充对象列表：用DataTable填充实体类
        /// </summary>  
        public static List<T> FillModel(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            List<T> modelList = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                //T model = (T)Activator.CreateInstance(typeof(T));  
                T model = new T();
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    BindingFlags flag = BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance;//忽略大小写
                    PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName, flag);
                    try
                    {
                        //typeof(Nullable<System.DateTime>)
                        if (propertyInfo != null && dr[i] != DBNull.Value)
                        {
                            if (propertyInfo.PropertyType == dt.Columns[i].DataType)
                            {
                                propertyInfo.SetValue(model, ValueConvert(dr[i], propertyInfo.PropertyType), null);
                            }
                            else
                            {
                                propertyInfo.SetValue(model, ValueConvert(dr[i], propertyInfo.PropertyType), null);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }


                }

                modelList.Add(model);
            }
            return modelList;
        }

        public static object ValueConvert(object input, Type type)
        {
            object outvalue = null;
            if (type == typeof(int))
            {
                try
                {
                    outvalue = Convert.ToInt32(input.ToString());

                }
                catch
                {
                    outvalue = 0;
                }
            }
            else if (type == typeof(decimal))
            {
                try
                {
                    outvalue = Convert.ToDecimal(input.ToString());

                }
                catch
                {
                    outvalue = 0;
                }
            }
            else if (type == typeof(float))
            {
                try
                {
                    outvalue = float.Parse(input.ToString());
                }
                catch
                {
                    outvalue = 0;
                }
            }
            else if (type == typeof(double))
            {
                try
                {
                    outvalue = Convert.ToDouble(input.ToString());
                }
                catch
                {
                    outvalue = 0;
                }
            }
            else if (type == typeof(DateTime))
            {
                try
                {
                    outvalue = Convert.ToDateTime(input.ToString());
                }
                catch
                {
                    outvalue = DateTime.MinValue;
                }
            }
            else if (type == typeof(string))
            {
                outvalue = input.ToString();
            }
            else if (type == typeof(DateTime?))
            {
                if (input.ToString() == "")
                {
                    outvalue = null;
                }
                else
                {
                    try
                    {
                        outvalue = Convert.ToDateTime(input.ToString());
                    }
                    catch
                    {
                        outvalue = DateTime.MinValue;
                    }
                }
            }
            return outvalue;
        }

        /// <summary>  
        /// 填充对象：用DataRow填充实体类
        /// </summary>  
        public static T FillModel(DataRow dr)
        {
            if (dr == null)
            {
                return default(T);
            }

            //T model = (T)Activator.CreateInstance(typeof(T));  
            T model = new T();

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);
                if (propertyInfo != null && dr[i] != DBNull.Value)
                    propertyInfo.SetValue(model, dr[i], null);
            }
            return model;
        }

        #endregion

        #region 实体类转换成DataTable

        /// <summary>
        /// 实体类转换成DataSet
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public static DataSet FillDataSet(List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            else
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(FillDataTable(modelList));
                return ds;
            }
        }

        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public static DataTable FillDataTable(List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            DataTable dt = CreateData(modelList[0]);

            foreach (T model in modelList)
            {
                DataRow dataRow = dt.NewRow();
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(model, null);
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// 根据实体类得到表结构
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        private static DataTable CreateData(T model)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                try
                {
                    dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
                }
                catch
                {
                    dataTable.Columns.Add(new DataColumn(propertyInfo.Name, typeof(string)));
                    //throw ex;
                }
            }
            return dataTable;
        }

        #endregion
    }
}
