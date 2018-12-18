using Hant.Web.API.DAL;
using Hant.Web.API.DAL.Entity;
using Hant.Web.API.Helper;
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

        #region 用户
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

        #region 添加用户
        [Route("SaveUserInfo")]
        [HttpGet]
        public HttpResponseMessage SaveUserInfo(string name, string phone, string role, string ou)
        {
            var res = new { Result = "OK" };
            string salt = Hant.Frame.Helper.EncryptPwd.CreateSalt();
            string SHA512Pwd = Hant.Frame.Helper.EncryptPwd.CreatePwdHashSHA512("123456", salt);
            string UserID = new BLL.Common.Common().GetUserID();
            sys_user i1 = new sys_user
            {
                UserID = UserID,
                UserName = name,
                Mobile = phone,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Status = 1
            };
            db.Sys_userRepository.Insert(i1);
            sys_user_authorization i2 = new sys_user_authorization
            {
                UserID = UserID,
                LoginName = name,
                LoginPwd = SHA512Pwd,
                Salt = salt,
                LoginType = (int)LoginType.LoginName,
                Status = 1,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            db.Sys_user_authorizationRepository.Insert(i2);
            foreach (string r in role.Split(','))
            {
                sys_user_role_relation i3 = new sys_user_role_relation
                {
                    RoleID = r,
                    UserID = UserID,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                db.Sys_user_role_relationRepository.Insert(i3);
            }
            sys_user_group_relation i4 = new sys_user_group_relation
            {
                OuID = ou,
                UserID = UserID,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            db.Sys_user_group_relationRepository.Insert(i4);
            db.Save();
            return new Models.JsonpResultAPI<object>(res);
        }
        #endregion

        #region 修改用户
        [Route("UpdateUserInfo")]
        [HttpGet]
        public HttpResponseMessage UpdateUserInfo(string name, string phone, string role, string ou, int id)
        {
            sys_user user = db.Sys_userRepository.Find(id);
            user.UserName = name;
            user.Mobile = phone;
            db.Sys_userRepository.Update(user, new string[] { "UserName", "Mobile" });
            db.Sys_user_group_relationRepository.DeleteByCondition(e => e.UserID == user.UserID);
            db.Sys_user_role_relationRepository.DeleteByCondition(e => e.UserID == user.UserID);
            foreach (string r in role.Split(','))
            {
                sys_user_role_relation i3 = new sys_user_role_relation
                {
                    RoleID = r,
                    UserID = user.UserID,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                db.Sys_user_role_relationRepository.Insert(i3);
            }
            sys_user_group_relation i4 = new sys_user_group_relation
            {
                OuID = ou,
                UserID = user.UserID,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            db.Sys_user_group_relationRepository.Insert(i4);

            db.Save();
            var res = new { Result = "OK" };
            return new Models.JsonpResultAPI<object>(res);
        }
        #endregion

        #region 删除用户
        [Route("DeleteUserInfo")]
        [HttpPost]
        public HttpResponseMessage DeleteUserInfo([FromBody] int[] id)
        {
            foreach (int i in id)
            {
                sys_user user = db.Sys_userRepository.Find(i);
                if (user != null)
                {
                    sys_user_authorization user_author = db.Sys_user_authorizationRepository.Get(e => e.UserID == user.UserID).FirstOrDefault();
                    if (user_author != null)
                    {
                        db.Sys_user_authorizationRepository.Delete(user_author.ID);
                    }
                    db.Sys_userRepository.Delete(i);
                }

            }
            db.Save();
            var res = new { Result = "OK" };
            return new Models.JsonpResultAPI<object>(res);
        }
        #endregion
        #endregion

        #region 角色
        #region 获取角色信息列表
        [Route("GetRoleInfoList")]
        [HttpGet]
        public HttpResponseMessage GetRoleInfoList(int page = 0, int limit = 0, string SearchName = "", int SearchStatus = 1)
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

        #region 删除角色
        [Route("DeleteRoleInfo")]
        [HttpPost]
        public HttpResponseMessage DeleteRoleInfo([FromBody] int[] id)
        {
            foreach (int i in id)
            {
                db.Sys_roleRepository.Delete(i);
            }
            db.Save();
            var res = new { Result = "OK" };
            return new Models.JsonpResultAPI<object>(res);
        }
        #endregion

        #region 获取角色树列表
        [Route("GetRoleInfoTreeList")]
        [HttpGet]
        public HttpResponseMessage GetRoleInfoTreeList(int page = 0, int limit = 0, string SearchName = "", int SearchStatus = 1)
        {
            IEnumerable<sys_role> ls = db.Sys_roleRepository.Get(fliter: e => !string.IsNullOrEmpty(SearchName) ? e.RoleName.Contains(SearchName) : 1 == 1
            && e.Status == SearchStatus, orderBy: q => q.OrderBy(e => e.ID), page: page, limit: limit);
            int Num = db.Sys_roleRepository.Num(fliter: e => !string.IsNullOrEmpty(SearchName) ? e.RoleName.Contains(SearchName) : 1 == 1
            && e.Status == SearchStatus);
            JArray jarray = RecursionData.RecursionToJson<sys_role>(ls, ls, new string[] { "RoleID", "RoleName" }, new string[2] { "value", "name" });
            var res = new { code = "", msg = "", count = Num, data = JToken.FromObject(jarray) };
            return new Models.JsonpResultAPI<object>(res);
        }
        #endregion

        #endregion

        #region 组织
        #region 获取组织信息列表
        [Route("GetGroupInfoList")]
        [HttpGet]
        public HttpResponseMessage GetGroupInfoList(int page = 0, int limit = 0, string SearchName = "", int SearchStatus = 1)
        {
            IEnumerable<sys_group> ls = db.Sys_groupRepository.Get(fliter: e => !string.IsNullOrEmpty(SearchName) ? e.ParentOuID == SearchName : e.ParentOuID == "0"
            && e.Status == SearchStatus, orderBy: q => q.OrderBy(e => e.ID), page: page, limit: limit);
            int Num = db.Sys_groupRepository.Num(fliter: e => !string.IsNullOrEmpty(SearchName) ? e.ParentOuID == SearchName : e.ParentOuID == "1"
            && e.Status == SearchStatus);
            var res = new { code = "", msg = "", count = Num, data = JToken.FromObject(ls) };
            return new Models.JsonpResultAPI<object>(res);
        }
        #endregion

        #region 修改组织状态
        [Route("UpdateGroupStatus")]
        [HttpGet]
        public HttpResponseMessage UpdateGroupStatus(int ID, int Status)
        {
            sys_group group = db.Sys_groupRepository.Find(ID);
            group.Status = Status;
            db.Sys_groupRepository.Update(group, new string[] { "Status" });
            db.Save();
            var res = new { Result = "OK" };
            return new Models.JsonpResultAPI<object>(res);
        }
        #endregion

        #region 添加组织
        [Route("SaveGroupInfo")]
        [HttpGet]
        public HttpResponseMessage SaveGroupInfo(string name, string parentOuID)
        {
            var res = new { Result = "OK" };
            if (db.Sys_groupRepository.Get(e => e.OuName == name && e.ParentOuID == parentOuID).Count() == 0)
            {
                sys_group group = new sys_group
                {
                    OuID = Guid.NewGuid().ToString("N"),
                    OuName = name,
                    ParentOuID = parentOuID == null ? "0" : parentOuID,
                    Status = 1,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                db.Sys_groupRepository.Insert(group);
                db.Save();
            }
            else
            {
                res = new { Result = "组织已存在" };
            }
            return new Models.JsonpResultAPI<object>(res);
        }
        #endregion

        #region 修改组织
        [Route("UpdateGroupInfo")]
        [HttpGet]
        public HttpResponseMessage UpdateGroupInfo(string name, int id)
        {
            sys_group group = db.Sys_groupRepository.Find(id);
            group.OuName = name;
            db.Sys_groupRepository.Update(group, new string[] { "OuName" });
            db.Save();
            var res = new { Result = "OK" };
            return new Models.JsonpResultAPI<object>(res);
        }
        #endregion

        #region 删除组织
        [Route("DeleteGroupInfo")]
        [HttpPost]
        public HttpResponseMessage DeleteGroupInfo([FromBody] int[] id)
        {
            foreach (int i in id)
            {
                db.Sys_groupRepository.Delete(i);
            }
            db.Save();
            var res = new { Result = "OK" };
            return new Models.JsonpResultAPI<object>(res);
        }
        #endregion

        #region 获取组织树的数据
        [Route("GetGroupTreeInfoList")]
        [HttpGet]
        public HttpResponseMessage GetGroupTreeInfoList(int page = 0, int limit = 0, string SearchName = "", int SearchStatus = 1)
        {
            IEnumerable<sys_group> ls = db.Sys_groupRepository.Get(fliter: e => !string.IsNullOrEmpty(SearchName) ? e.OuName.Contains(SearchName) : 1 == 1
            && e.Status == SearchStatus, orderBy: q => q.OrderBy(e => e.ID), page: page, limit: limit);
            int Num = db.Sys_groupRepository.Num(fliter: e => !string.IsNullOrEmpty(SearchName) ? e.OuName.Contains(SearchName) : 1 == 1
            && e.Status == SearchStatus);
            JArray jarray = RecursionData.RecursionToJson<sys_group>(ls, ls, new string[] { "OuID", "OuName" }, new string[2] { "value", "name" }, "ParentOuID", "OuID", "0");
            var res = new { code = "", msg = "", count = Num, data = JToken.FromObject(jarray) };
            return new Models.JsonpResultAPI<object>(res);
        }
        #endregion
        #endregion

        #region 用户与角色、组织关系

        #region 用户与角色关系列表
        [Route("GetUserRoleRelation")]
        [HttpGet]
        public HttpResponseMessage GetUserRoleRelation(string UserID)
        {
            IEnumerable<sys_user_role_relation> ls = db.Sys_user_role_relationRepository.Get(fliter: e => e.UserID == UserID);
            return new Models.JsonpResultAPI<object>(ls);
        }
        #endregion

        #region 用户与组织关系列表
        [Route("GetUserOuRelation")]
        [HttpGet]
        public HttpResponseMessage GetUserOuRelation(string UserID)
        {
            IEnumerable<sys_user_group_relation> ls = db.Sys_user_group_relationRepository.Get(fliter: e => e.UserID == UserID);
            return new Models.JsonpResultAPI<object>(ls);
        }
        #endregion

        #endregion
    }
}