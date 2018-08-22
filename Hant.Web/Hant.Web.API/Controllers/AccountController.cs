using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web.Http;

namespace Hant.Web.API.Controllers
{
    public class AccountController : ApiController
    {
        #region 获取验证码图片
        [HttpGet]
        public HttpResponseMessage GetImageCode(string callback)
        {
            string imgcode = Helper.ImgeHelper.getCode();
            string base64Str = Helper.ImgeHelper.ImgToBase64String(Helper.ImgeHelper.CreateCheckCodeImage(imgcode));
            var res = new { imgcode, base64Str };
            return new Models.JsonpResultAPI<object>(res, callback);
        }
        #endregion
    }
}