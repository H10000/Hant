using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketServer
{
  public class Account
    {
        private string _userID;
        private string _ip;

        public string UserID
        {
            set { _userID = value; }
            get { return _userID; }
        }

        public string IP
        {
            set { _ip = value; }
            get { return _ip; }
        }
    }
}
