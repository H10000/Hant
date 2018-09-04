﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web.Http;
using Hant.Web.API.DAL;
using Hant.Web.API.DAL.Entity;

namespace Hant.Web.API.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        UnitOfWork db = new UnitOfWork();
        #region 获取验证码图片
        [Route("GetImageCode")]
        [HttpGet]
        public HttpResponseMessage GetImageCode(string callback)
        {
            string imgcode = Helper.ImgeHelper.getCode();
            string base64Str = Helper.ImgeHelper.ImgToBase64String(Helper.ImgeHelper.CreateCheckCodeImage(imgcode));
            var res = new { imgcode = imgcode.ToLower(), base64Str };
            return new Models.JsonpResultAPI<object>(res, callback);
        }
        #endregion
        #region 发送短信验证码
        [Route("SendShortMessage")]
        [HttpGet]
        public HttpResponseMessage SendShortMessage(string PhoneNum, string callback)
        {
            var res = new { Result = "OK" };
            return new Models.JsonpResultAPI<object>(res, callback);
        }
        #endregion
        #region 验证用户名是否已存在
        [Route("ValidateNameIsExist")]
        [HttpGet]
        public HttpResponseMessage ValidateNameIsExist(string Name, string callback)
        {
            var res = new { Result = "OK" };
            if (db.Sys_user_authorizationRepository.Get(fliter: e => e.LoginName == Name).ToList().Count > 0)
            {
                res = new { Result = "NO" };
            }
            return new Models.JsonpResultAPI<object>(res, callback);
        }
        #endregion
    }
}