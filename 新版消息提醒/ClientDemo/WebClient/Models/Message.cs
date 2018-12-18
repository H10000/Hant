using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClient.Models
{
    public class Message
    {

        /// <summary>
        /// 消息id
        /// </summary>
        public int MessageID { get; set; }

        /// <summary>
        /// 发送人id
        /// </summary>
        public string SendId { get; set; }

        /// <summary>
        /// 发送人名字
        /// </summary>
        public string SendName { get; set; }

        /// <summary>
        /// 接收id
        /// </summary>
        public string ReceiveId { get; set; }

        /// <summary>
        /// 接收人姓名
        /// </summary>
        public string ReceiveName { get; set; }

        /// <summary>
        /// 发送消息
        /// </summary>
        public string MessageContent { get; set; }

        /// <summary>
        /// 发送消息
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// 消息状态
        /// </summary>
        public int SendStatus { get; set; }

        /// <summary>
        /// 消息产生时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
