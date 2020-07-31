using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ToolResolver
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length == 2)//平台启动
                Application.Run(new Form1(args[0], args[1]));
            else if (args.Length == 0)//单独启动
                Application.Run(new Form1());
            else if (args.Length == 1)//工具库启动
                Application.Run(new Form1(args[0], true));
        }
    }
}
