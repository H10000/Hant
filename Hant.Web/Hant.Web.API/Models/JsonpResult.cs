using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Hant.Web.API.Models
{
    public class JsonpResult<T> : ActionResult
    {
        private T Obj { get; set; }
        private string CallbackName { get; set; }

        public JsonpResult(T obj,string callbackname)
        {
            Obj = obj;
            CallbackName = callbackname;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var jsonp = CallbackName + "(" + JsonConvert.SerializeObject(Obj) +")";
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.Write(jsonp);
        }
    }
}