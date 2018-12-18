﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Threading;
using System.Collections;

namespace WebSocket
{
    public struct UserInfo
    {
        public string username; //用户名
        public Socket clientSocket; //
        public string IP; //用户ip
        public string xb;
    }

    class Test
    {
        private static Socket listener;
        private static Hashtable ht;
        static void Main(string[] args)
        {
            int port = 1818;//监听端口为1818端口
            ht = new Hashtable();//用于存放客户端的连接socket
            byte[] buffer = new byte[1024];

            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, port);
            listener = new Socket(localEP.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEP);
                listener.Listen(10);

                Console.WriteLine("等待客户端连接....");
                while (true)
                {
                    Thread th = new Thread(Receive);
                    th.Start();

                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        /// <summary>
        /// 线程调用
        /// </summary>
        private static void Receive()
        {
            Socket clientSocket = listener.Accept(); //接收到客户端的连接
            clientSocket.Blocking = true;
            IPEndPoint clientipe = (IPEndPoint)clientSocket.RemoteEndPoint;
            //    Console.WriteLine("[" + clientipe.Address.ToString() + "] Connected");
            if (!ht.ContainsKey(clientipe.Address.ToString()))
            {
                //将ip地址设置为hashTable的key值 若hashTable中存在该ip地址则不再ht中添加socket以免发送重复数据
                ht.Add(clientipe.Address.ToString(), clientSocket);
            }
            Console.WriteLine("接收到了客户端：ip" + clientSocket.RemoteEndPoint.ToString() + "的连接");
            byte[] buffer = new byte[1024];
            int length = clientSocket.Receive(buffer);
            clientSocket.Send(PackHandShakeData(GetSecKeyAccetp(buffer, length)));
            Console.WriteLine("已经发送握手协议了....");

            //接收用户姓名信息
            length = clientSocket.Receive(buffer);
            string xm = AnalyticData(buffer, length);




            while (true)
            {
                //接受客户端数据
                Console.WriteLine("等待客户端数据....");
                length = clientSocket.Receive(buffer);//接受客户端信息
                string clientMsg = AnalyticData(buffer, length);
                Console.WriteLine("接受到客户端数据：" + clientMsg);

                //发送数据
                string sendMsg = "" + clientMsg;
                Console.WriteLine("发送数据：“" + sendMsg + "” 至客户端....");
                //遍历hashTable中的数据获取Socket发送数据
                foreach (DictionaryEntry de in ht)
                {
                    Socket sc = (Socket)de.Value;
                    sc.Send(PackData(clientSocket.RemoteEndPoint.ToString() + xm + "说：" + sendMsg));

                }
                Thread.Sleep(1000);
                //clientSocket.Send(PackData(sendMsg));
            }
        }
        /// <summary>
        /// 打包握手信息
        /// </summary>
        /// <param name="secKeyAccept">Sec-WebSocket-Accept</param>
        /// <returns>数据包</returns>
        private static byte[] PackHandShakeData(string secKeyAccept)
        {
            var responseBuilder = new StringBuilder();
            responseBuilder.Append("HTTP/1.1 101 Switching Protocols" + Environment.NewLine);
            responseBuilder.Append("Upgrade: websocket" + Environment.NewLine);
            responseBuilder.Append("Connection: Upgrade" + Environment.NewLine);
            responseBuilder.Append("Sec-WebSocket-Accept: " + secKeyAccept + Environment.NewLine + Environment.NewLine);
            //如果把上一行换成下面两行，才是thewebsocketprotocol-17协议，但居然握手不成功，目前仍没弄明白！
            //responseBuilder.Append("Sec-WebSocket-Accept: " + secKeyAccept + Environment.NewLine);
            //responseBuilder.Append("Sec-WebSocket-Protocol: chat" + Environment.NewLine);

            return Encoding.UTF8.GetBytes(responseBuilder.ToString());
        }

        /// <summary>
        /// 生成Sec-WebSocket-Accept
        /// </summary>
        /// <param name="handShakeText">客户端握手信息</param>
        /// <returns>Sec-WebSocket-Accept</returns>
        private static string GetSecKeyAccetp(byte[] handShakeBytes, int bytesLength)
        {
            string handShakeText = Encoding.UTF8.GetString(handShakeBytes, 0, bytesLength);
            string key = string.Empty;
            Regex r = new Regex(@"Sec\-WebSocket\-Key:(.*?)\r\n");
            Match m = r.Match(handShakeText);
            if (m.Groups.Count != 0)
            {
                key = Regex.Replace(m.Value, @"Sec\-WebSocket\-Key:(.*?)\r\n", "$1").Trim();
            }
            byte[] encryptionString = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(key + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"));
            return Convert.ToBase64String(encryptionString);
        }

        /// <summary>
        /// 解析客户端数据包
        /// </summary>
        /// <param name="recBytes">服务器接收的数据包</param>
        /// <param name="recByteLength">有效数据长度</param>
        /// <returns></returns>
        private static string AnalyticData(byte[] recBytes, int recByteLength)
        {
            if (recByteLength < 2) { return string.Empty; }

            bool fin = (recBytes[0] & 0x80) == 0x80; // 1bit，1表示最后一帧  
            if (!fin)
            {
                return string.Empty;// 超过一帧暂不处理 
            }

            bool mask_flag = (recBytes[1] & 0x80) == 0x80; // 是否包含掩码  
            if (!mask_flag)
            {
                return string.Empty;// 不包含掩码的暂不处理
            }

            int payload_len = recBytes[1] & 0x7F; // 数据长度  

            byte[] masks = new byte[4];
            byte[] payload_data;

            if (payload_len == 126)
            {
                Array.Copy(recBytes, 4, masks, 0, 4);
                payload_len = (UInt16)(recBytes[2] << 8 | recBytes[3]);
                payload_data = new byte[payload_len];
                Array.Copy(recBytes, 8, payload_data, 0, payload_len);

            }
            else if (payload_len == 127)
            {
                Array.Copy(recBytes, 10, masks, 0, 4);
                byte[] uInt64Bytes = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    uInt64Bytes[i] = recBytes[9 - i];
                }
                UInt64 len = BitConverter.ToUInt64(uInt64Bytes, 0);

                payload_data = new byte[len];
                for (UInt64 i = 0; i < len; i++)
                {
                    payload_data[i] = recBytes[i + 14];
                }
            }
            else
            {
                Array.Copy(recBytes, 2, masks, 0, 4);
                payload_data = new byte[payload_len];
                Array.Copy(recBytes, 6, payload_data, 0, payload_len);

            }

            for (var i = 0; i < payload_len; i++)
            {
                payload_data[i] = (byte)(payload_data[i] ^ masks[i % 4]);
            }

            return Encoding.UTF8.GetString(payload_data);
        }


        /// <summary>
        /// 打包服务器数据
        /// </summary>
        /// <param name="message">数据</param>
        /// <returns>数据包</returns>
        private static byte[] PackData(string message)
        {
            byte[] contentBytes = null;
            byte[] temp = Encoding.UTF8.GetBytes(message);

            if (temp.Length < 126)
            {
                contentBytes = new byte[temp.Length + 2];
                contentBytes[0] = 0x81;
                contentBytes[1] = (byte)temp.Length;
                Array.Copy(temp, 0, contentBytes, 2, temp.Length);
            }
            else if (temp.Length < 0xFFFF)
            {
                contentBytes = new byte[temp.Length + 4];
                contentBytes[0] = 0x81;
                contentBytes[1] = 126;
                contentBytes[2] = (byte)(temp.Length & 0xFF);
                contentBytes[3] = (byte)(temp.Length >> 8 & 0xFF);
                Array.Copy(temp, 0, contentBytes, 4, temp.Length);
            }
            else
            {
                // 暂不处理超长内容  
            }

            return contentBytes;
        }
    }

}