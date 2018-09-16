using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Hant.Web.API.Models
{
    public class JsonpResultAPI_JSONP<T> : HttpResponseMessage 
    {
        private T Obj { get; set; }
        private string CallbackName { get; set; }

        public JsonpResultAPI_JSONP(T obj, string callbackname)
        {
            Obj = obj;
            CallbackName = callbackname;
            var jsonp = CallbackName + "(" + JsonConvert.SerializeObject(Obj) + ")";
            Content = new StringContent(jsonp, Encoding.GetEncoding("UTF-8"), "application/json"); 
        }
    }
}