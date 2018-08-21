using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

namespace Hant.Web.API.Controllers
{
    public class AccountController : ApiController
    {
        #region 获取验证码图片
        [HttpGet]
        public string GetImageCode()
        {
            var res = new { result = Helper.ImgeHelper.ImgToBase64String(Helper.ImgeHelper.CreateCheckCodeImage(Helper.ImgeHelper.getCode())) };
            return JsonConvert.SerializeObject(res);
        }
        #endregion
    }
}