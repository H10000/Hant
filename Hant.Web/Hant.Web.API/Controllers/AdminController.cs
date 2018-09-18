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
            IEnumerable<sys_user> ls = db.Sys_userRepository.Get(fliter: e => !string.IsNullOrEmpty(SearchUserName) ? e.UserName.Contains(SearchUserName) : 1 == 1
            && !string.IsNullOrEmpty(SearchPhone) ? e.Mobile.Contains(SearchPhone) : 1 == 1 && e.Status == SearchStatus
            , orderBy: q => q.OrderBy(e => e.ID), page: page, limit: limit);
            int Num = db.Sys_userRepository.Num(fliter: e => !string.IsNullOrEmpty(SearchUserName) ? e.UserName.Contains(SearchUserName) : 1 == 1
            && !string.IsNullOrEmpty(SearchPhone) ? e.Mobile.Contains(SearchPhone) : 1 == 1 && e.Status == SearchStatus);
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

        #region 获取角色信息列表
        [Route("GetRoleInfoList")]
        [HttpGet]
        public HttpResponseMessage GetRoleInfoList(int page, int limit, string SearchName = "", int SearchStatus = 1)
        {
            IEnumerable<sys_role> ls = db.Sys_roleRepository.Get(fliter: e => !string.IsNullOrEmpty(SearchName) ? e.RoleName.Contains(SearchName) : 1 == 1
            && e.Status == SearchStatus, orderBy: q => q.OrderBy(e => e.ID), page: page, limit: limit);
            int Num = db.Sys_roleRepository.Num(fliter: e => !string.IsNullOrEmpty(SearchName) ? e.RoleName.Contains(SearchName) : 1 == 1
            && e.Status == SearchStatus);
            var res = new { code = "", msg = "", count = Num, data = JToken.FromObject(ls) };
            return new Models.JsonpResultAPI<object>(res);
        }
        #endregion

        #region 修改角色状态
        [Route("UpdateRoleStatus")]
        [HttpGet]
        public HttpResponseMessage UpdateRoleStatus(int ID, int Status)
        {
            sys_role role = db.Sys_roleRepository.Find(ID);
            role.Status = Status;
            db.Sys_roleRepository.Update(role, new string[] { "Status" });
            db.Save();
            var res = new { Result = "OK" };
            return new Models.JsonpResultAPI<object>(res);
        }
        #endregion

        #region 添加角色
        [Route("SaveRoleInfo")]
        [HttpGet]
        public HttpResponseMessage SaveRoleInfo(string name)
        {
            var res = new { Result = "OK" };
            if (db.Sys_roleRepository.Get(e => e.RoleName == name).Count() == 0)
            {
                sys_role role = new sys_role
                {
                    RoleID = Guid.NewGuid().ToString("N"),
                    RoleName = name,
                    Status = 1,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                db.Sys_roleRepository.Insert(role);
                db.Save();
            }
            else
            {
                res = new { Result = "角色已存在" };
            }
            return new Models.JsonpResultAPI<object>(res);
        }
        #endregion

        #region 修改角色
        [Route("UpdateRoleInfo")]
        [HttpGet]
        public HttpResponseMessage UpdateRoleInfo(string name, int id)
        {
            sys_role role = db.Sys_roleRepository.Find(id);
            role.RoleName = name;
            db.Sys_roleRepository.Update(role, new string[] { "RoleName" });
            db.Save();
            var res = new { Result = "OK" };
            return new Models.JsonpResultAPI<object>(res);
        }
        #endregion

        #region 修改角色
        [Route("DeleteRoleInfo")]
        [HttpGet]
        public HttpResponseMessage DeleteRoleInfo(int id)
        {
            db.Sys_roleRepository.Delete(id);
            db.Save();
            var res = new { Result = "OK" };
            return new Models.JsonpResultAPI<object>(res);
        }
        #endregion

    }
}