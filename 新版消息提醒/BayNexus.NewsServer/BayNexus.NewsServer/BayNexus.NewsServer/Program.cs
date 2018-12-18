using System;
using System.Windows.Forms;

namespace BayNexus.NewsServer
{
    static class Program
    {
        public static ServicesForm from;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            from = new ServicesForm();
            Application.Run(from);
        }
    }
}
