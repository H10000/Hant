using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayNexus.NewsServer.Models
{
    public class ReqInfo
    {
        public string sendID { get; set; }
        public string receiveID { get; set; }
        public string messageType { get; set; }
        public int sendStatus { get; set; }
    }
}
