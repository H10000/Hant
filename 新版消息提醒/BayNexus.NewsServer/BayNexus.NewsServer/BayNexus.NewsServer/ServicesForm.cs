using BayNexus.NewsServer.BLL;
using BayNexus.NewsServer.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BayNexus.NewsServer.Models;
using Microsoft.AspNet.SignalR;
using BayNexus.NewsServer.Hubs;
using BayNexus.NewsServer.BLL.MessageProcess;

namespace BayNexus.NewsServer
{
    public partial class ServicesForm : Form
    {
        public ServicesForm()
        {
            //不起用线程安全
            //Control.CheckForIllegalCrossThreadCalls = false;
            new ServicesManager(ConfigHelper.GetServiceAddress).Start();
            InitializeComponent();
        }

        object lockobj = new object();

        public void AddItems(User user)
        {
            ListViewItem item = new ListViewItem(new string[] { user.UserID, user.UserName, DateTime.Now.ToString(), user.DeviceType == "1" ? "PC" : "PDA" });
            if (this.lv_Users.InvokeRequired)
            {
                Action<ListViewItem> a = (i) =>
                {
                    this.lv_Users.Items.Add(i);
                };
                this.Invoke(a, item);
            }
            else
            {
                this.lv_Users.Items.Add(item);
            }
        }

        public void RemoveItems(User user)
        {
            lock (lockobj)
            {
                if (this.lv_Users.InvokeRequired)
                {
                    Action<User> a = (u) =>
                    {
                        for (int i = 0; i < this.lv_Users.Items.Count; i++)
                        {
                            if (this.lv_Users.Items[i].Text == u.UserID)
                            {
                                this.lv_Users.Items.RemoveAt(i);
                            }
                        }
                    };
                    this.Invoke(a, user);
                }
                else
                {
                    for (int i = 0; i < this.lv_Users.Items.Count; i++)
                    {
                        if (this.lv_Users.Items[i].Text == user.UserID)
                        {
                            this.lv_Users.Items.RemoveAt(i);
                        }
                    }
                }
            }
        }
        private IHubContext HubContext { get; set; }
        private void btn_send_Click(object sender, EventArgs e)
        {
            BayNexus.NewsServer.Models.Message message = new BayNexus.NewsServer.Models.Message()
            {
                SendId = "Sys_001",
                SendName = "系统提示",
                ReceiveId = "",
                MessageContent = txt_SendMessage.Text,
                MessageType = "all",
                CreateTime = DateTime.Now
            };
            
            //保存数据库消息
            MessageService.MessageServiceSoapClient client = new MessageService.MessageServiceSoapClient();
            client.SaveMessage(message.MessageContent, message.MessageType, message.SendId, message.SendName, message.ReceiveId, "", DateTime.Now.ToString(), "1", DateTime.Now.ToString());
        }

    }
}
