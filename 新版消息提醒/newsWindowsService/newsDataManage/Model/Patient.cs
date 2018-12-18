using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newsDataManage.Model
{
    public class Patient
    {
        public string PatientID { get; set; }
        public int VisitID { get; set; }
        public string PatientName { get; set; }
        public string NurseCellCode { get; set; }
    }
}
