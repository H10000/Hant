﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Hant.Web.API.Models
{
    public class JsonpResultAPI<T> : HttpResponseMessage
    {
        private T Obj { get; set; }

        public JsonpResultAPI(T obj)
        {
            Obj = obj;
            var jsonp = JsonConvert.SerializeObject(Obj);
            Content = new StringContent(jsonp, Encoding.GetEncoding("UTF-8"), "application/json");
        }
    }
}