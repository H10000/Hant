using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ClientDemo
{
    class Program
    {

        static void Main(string[] args)
        {
            HubConnection connection = null;
            IHubProxy myHub = null;
            string url = "http://localhost:8889";


            IDictionary<string, string> dl = new Dictionary<string, string>();
            dl.Add("UserID", "hant");
            dl.Add("UserName", "韩涛");
            dl.Add("DepartmentID", "001");
            dl.Add("DeviceType","1");//1代表PC客户端
            connection = new HubConnection(url, dl);
            //类名必须与服务端一致
            myHub = connection.CreateHubProxy("SignalRHub");

            //方法名必须与服务端一致
            myHub.On<object>("receive", (message) =>
            {
                JObject jobject = message as JObject;
                Console.WriteLine("发送人："+jobject["username"]);
                Console.WriteLine("消  息：" + jobject["content"]);
            });
            connection.Start().ContinueWith(task =>
            {
                if (!task.IsFaulted)
                {
                    Console.WriteLine("连接成功");
                }
                else
                {
                    Console.WriteLine("连接失败");
                }
            }).Wait();
            Console.ReadLine();
        }
    }
}
