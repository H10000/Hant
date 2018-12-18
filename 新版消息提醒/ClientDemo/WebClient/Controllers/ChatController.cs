using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace WebClient.Controllers
{
    public class ChatController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetCurrentMessage(string receiveID, string sendID, string messageType, int sendStatus)
        {
            DataTable dt = new DAL.DBMessage().GetMessageCurrent(receiveID, sendID, messageType, sendStatus);
            return Json(JsonConvert.SerializeObject(dt));
        }

        public ActionResult GetMessageHistory(string receiveID, string sendID, string messageType, int sendStatus)
        {
            DataTable dt = new DAL.DBMessage().GetMessageHistory(receiveID, sendID, messageType, sendStatus);
            return Json(JsonConvert.SerializeObject(dt));
        }

        public ActionResult GetUserInfo()
        {
            DataTable dt = new DAL.DBMessage().GetUserInfo();
            return Json(JsonConvert.SerializeObject(dt));
        }

        public ActionResult UploadMessageFile()
        {
            var res = new
            {
                code = 1 //0表示成功，其它表示失败
             ,
                msg = "上传失败" //失败信息
             ,
                data = new
                {
                    src = "" //文件url
                 ,
                    name = "" //文件名
                }
            };

            try
            {
                var f = Request.Files;
                var file = Request.Files[0]; //获取选中文件 
                var filecombin = file.FileName.Split('.');
                if (file == null || String.IsNullOrEmpty(file.FileName) || file.ContentLength == 0 || filecombin.Length < 2)
                {
                    return Json(res);
                }
                //定义本地路径位置
                string local = "Upload\\NewsFile";
                string filePathName = string.Empty;
                string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, local);

                var tmpName = Server.MapPath("~/Upload/NewsFile/");
                var tmp = file.FileName;
                var tmpIndex = 0;
                //判断是否存在相同文件名的文件 相同累加1继续判断
                while (System.IO.File.Exists(tmpName + tmp))
                {
                    tmp = filecombin[0] + "_" + ++tmpIndex + "." + filecombin[1];
                }

                //不带路径的最终文件名
                filePathName = tmp;

                if (!System.IO.Directory.Exists(localPath))
                    System.IO.Directory.CreateDirectory(localPath);
                string localURL = Path.Combine(local, filePathName);
                file.SaveAs(Path.Combine(localPath, filePathName));   //保存图片（文件夹）

                res = new
                {
                    code = 0 //0表示成功，其它表示失败
                 ,
                    msg = "上传成功" //失败信息
                 ,
                    data = new
                    {
                        src = localURL.Trim()//.Replace("\\", "|") //文件url
                     ,
                        name = Path.GetFileNameWithoutExtension(file.FileName) //文件名
                    }
                };
            }
            catch
            {

            }

            return Json(res);
        }
    }
}