using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web.Http;
using Hant.Web.API.DAL;
using Hant.Web.API.DAL.Entity;
using Hant.Web.API.Helper;

namespace Hant.Web.API.Controllers
{
    [RoutePrefix("account")]
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
            return new Models.JsonpResultAPI_JSONP<object>(res, callback);
        }
        #endregion

        #region 发送短信验证码
        [Route("SendShortMessage")]
        [HttpGet]
        public HttpResponseMessage SendShortMessage(string PhoneNum, string callback)
        {
            var res = new { Result = "OK" };
            return new Models.JsonpResultAPI_JSONP<object>(res, callback);
        }
        #endregion

        #region 验证用户名是否已存在
        [Route("ValidateNameIsExist")]
        [HttpGet]
        public HttpResponseMessage ValidateNameIsExist(string Name, string callback)
        {
            var res = new { Result = "OK" };
            if (db.Sys_user_authorizationRepository.Get(fliter: e => e.LoginName == Name).Count() > 0)
            {
                res = new { Result = "NO" };
            }
            return new Models.JsonpResultAPI_JSONP<object>(res, callback);
        }
        #endregion

        #region 用户名注册-保存用户信息
        [Route("SaveZhuCeInfoOfLoginName")]
        [HttpGet]
        public HttpResponseMessage SaveZhuCeInfoOfLoginName(string LoginName, string Pwd, string callback)
        {
            var res = new { Result = "OK" };
            string salt = Hant.Frame.Helper.EncryptPwd.CreateSalt();
            string SHA512Pwd = Hant.Frame.Helper.EncryptPwd.CreatePwdHashSHA512(Pwd, salt);
            string UserID = new BLL.Common.Common().GetUserID();
            sys_user i1 = new sys_user
            {
                UserID = UserID,
                UserName = LoginName,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Status = 1
            };
            db.Sys_userRepository.Insert(i1);
            sys_user_authorization i2 = new sys_user_authorization
            {
                UserID = UserID,
                LoginName = LoginName,
                LoginPwd = SHA512Pwd,
                Salt = salt,
                LoginType = (int)LoginType.LoginName,
                Status = 1,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            db.Sys_user_authorizationRepository.Insert(i2);
            db.Save();
            return new Models.JsonpResultAPI_JSONP<object>(res, callback);
        }
        #endregion

        #region 用户登录-用户名密码登录
        [HttpGet]
        [Route("ValidateDengLuByLoginName")]
        public HttpResponseMessage ValidateDengLuByLoginName(string LoginName, string Pwd, string callback)
        {
            var res = new { Result = "OK" };
            IEnumerable<sys_user_authorization> LoginInfo = db.Sys_user_authorizationRepository.Get(fliter: e => e.LoginName == LoginName);
            if (LoginInfo.Count() == 0)
            {
                res = new { Result = "1" };//不存在用户
            }
            else if (LoginInfo.FirstOrDefault().Status != 1)
            {
                res = new { Result = "1" };//用户状态未禁用
            }
            else
            {
                string SHA512Pwd = Hant.Frame.Helper.EncryptPwd.CreatePwdHashSHA512(Pwd, LoginInfo.FirstOrDefault().Salt);
                if (SHA512Pwd != LoginInfo.FirstOrDefault().LoginPwd)
                {
                    res = new { Result = "2" };//密码不正确
                }
            }
            return new Models.JsonpResultAPI_JSONP<object>(res, callback);
        }
        #endregion
    }
}