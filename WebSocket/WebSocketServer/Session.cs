﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
namespace WebSocketServer
{
    public class Session
    {
        private Socket _sockeclient;
        private byte[] _buffer;
        private string _ip;
        private int _port;
        private bool _isweb = false;
        private string _userID;



        public Socket SockeClient
        {
            set { _sockeclient = value; }
            get { return _sockeclient; }
        }

        public byte[] buffer
        {
            set { _buffer = value; }
            get { return _buffer; }
        }

        public string IP
        {
            set { _ip = value; }
            get { return _ip; }
        }

        public int Port
        {
            set { _port = value; }
            get { return _port; }
        }

        public bool isWeb
        {
            set { _isweb = value; }
            get { return _isweb; }
        }

        public string UserID
        {
            set { _userID = value; }
            get { return _userID; }
        }
    }
}