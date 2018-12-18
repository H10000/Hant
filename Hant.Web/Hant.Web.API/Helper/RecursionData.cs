using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hant.Web.API.Helper
{
    public class RecursionData
    {
        /// <summary>
        /// 生成这样的json[{"name": "11","children": [{"name": "22"}]}]
        /// </summary>
        /// <typeparam name="T">需要转换的类</typeparam>
        /// <param name="ts">需要转换类的数据集</param>
        /// <param name="ts_Mark">备份初始数据（方法中的递归需要）</param>
        /// <param name="Fields">用到的字段（与JsonNodes数组一一对应）</param>
        /// <param name="JsonNodes">转换后的JSON的节点名称（与Fields数组一一对应）</param>
        /// <param name="Flag">标记父子级的字段名称（与InitFlag、ID配对使用）</param>
        /// <param name="ID">与Flag中父级值相同的字段（与Flag、InitFlag配对使用）</param>
        /// <param name="InitFlag">标记字段的初始值（与Flag、ID配对使用）通过初始值找出第一级，然后递归</param>
        /// <returns></returns>
        public static JArray RecursionToJson<T>(IEnumerable<T> ts, IEnumerable<T> ts_Mark, string[] Fields = null, string[] JsonNodes = null, string Flag = "", string ID = "", string InitFlag = "0")
        {
            JArray jarray = new JArray();
            foreach (T t in ts)
            {
                Type type = t.GetType();
                PropertyInfo[] ps = type.GetProperties();
                if (string.IsNullOrEmpty(Flag))//没有父子级的情况
                {
                    JObject jobject = new JObject();
                    foreach (PropertyInfo p in ps)
                    {
                        if (Fields == null || JsonNodes == null)//默认所有字段
                        {
                            jobject.Add(p.Name, p.GetValue(t).ToString());
                        }
                        else//根据Fields去字段
                        {
                            if (Fields.Contains(p.Name))
                            {
                                jobject.Add(JsonNodes[Fields.ToList().IndexOf(p.Name)], p.GetValue(t).ToString());
                            }
                        }
                    }
                    jarray.Add(jobject as JToken);
                }
                else//有父子级的情况
                {
                    PropertyInfo p1 = type.GetProperty(Flag);
                    if (p1.GetValue(t).ToString() == InitFlag)
                    {
                        JObject jobject = new JObject();
                        foreach (PropertyInfo p in ps)
                        {
                            if (Fields == null || JsonNodes == null)//默认所有字段
                            {
                                jobject.Add(p.Name, p.GetValue(t).ToString());
                            }
                            else//根据Fields去字段
                            {
                                if (Fields.Contains(p.Name))
                                {
                                    jobject.Add(JsonNodes[Fields.ToList().IndexOf(p.Name)], p.GetValue(t).ToString());
                                }
                            }
                        }
                        IEnumerable<T> child_ts = ts_Mark.Where(e => e.GetType().GetProperty(Flag).GetValue(e).ToString() == type.GetProperty(ID).GetValue(t).ToString()).ToList();
                        if (child_ts.Count() > 0)
                        {
                            jobject.Add("children", RecursionToJson<T>(child_ts, ts, Fields, JsonNodes, Flag, ID, type.GetProperty(ID).GetValue(t).ToString()) as JToken);
                        }
                        jarray.Add(jobject as JToken);
                    }
                }
            }
            return jarray;
        }
    }
}