using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using newsDataManage;

namespace TestApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            new newsDataManage.BLL.Business().newOrdersMethod();
            new newsDataManage.BLL.Business().UnExtOrdersMethod();
            new newsDataManage.BLL.Business().unThreeStoolMethod();
        }
    }
}
