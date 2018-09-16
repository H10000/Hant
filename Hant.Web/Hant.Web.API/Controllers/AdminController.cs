using Hant.Web.API.DAL;
using Hant.Web.API.DAL.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Hant.Web.API.Controllers
{
    [RoutePrefix("admin")]
    public class AdminController : ApiController
    {
        UnitOfWork db = new UnitOfWork();

        #region 获取用户信息列表
        [Route("GetUserInfoList")]
        [HttpGet]
        public HttpResponseMessage GetUserInfoList(int page, int limit, string SearchUserName = "", string SearchPhone = "", int SearchStatus = 1)
        {
            IEnumerable<sys_user> ls = db.Sys_userRepository.Get(fliter: e => !string.IsNullOrEmpty(SearchUserName) ? e.UserName.Contains(SearchUserName) : 1 == 1 && !string.IsNullOrEmpty(SearchPhone) ? e.Mobile.Contains(SearchPhone) : 1 == 1
            , orderBy: q => q.OrderBy(e => e.ID), page: page, limit: limit);
            int Num = db.Sys_userRepository.Num();
            var res = new { code = "", msg = "", count = Num, data = JToken.FromObject(ls) };
            return new Models.JsonpResultAPI<object>(res);
        }
        #endregion

        #region 修改账号状态
        [Route("UpdateUserStatus")]
        [HttpGet]
        public HttpResponseMessage UpdateUserStatus(int ID, int Status)
        {
            sys_user user = db.Sys_userRepository.Find(ID);
            user.Status = Status;
            db.Sys_userRepository.Update(user, new string[] { "Status" });
            db.Save();
            var res = new { Result = "OK" };
            return new Models.JsonpResultAPI<object>(res);
        }
        #endregion
    }
}