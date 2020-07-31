using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chromium.WebBrowser;
using AppTools.beans;
using AppTools.util;
using System.Windows.Forms;

namespace AppTools.scriptExtend
{
    class Window
    {
        /// <summary>
        /// 注册IoOption对象
        /// </summary>
        /// <param name="kworld"></param>
        public static void registScript(JSObject kworld)
        {
            var WindowObj = kworld.AddObject("Window");
            Open(WindowObj);
        }
        /// <summary>
        /// 打开新窗口
        /// </summary>
        /// <param name="IoOptionObj"></param>
        public static void Open(JSObject IoOptionObj)
        {
            var OpenObj = IoOptionObj.AddFunction("Open");
            OpenObj.Execute += (func, args) =>
            {
                if (args.Arguments.Length < 4)
                {
                    MessageBox.Show(AppWin.gloabForm, "参数个数不正确", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    args.SetReturnValue(false);
                    return;
                }
                string url=args.Arguments[0].ToString();
                string  openDev= args.Arguments[1].ToString();
                int width = int.Parse(args.Arguments[2].ToString());
                int height=int.Parse(args.Arguments[3].ToString());
               // DialogWin win = new DialogWin(GloabConfig.serverUrl+"/views/publishProgram/dialog/packBagePro.html", openDev, width, height);
                DialogWin win = new DialogWin(url, openDev, width, height);
                win.ShowDialog();

                args.SetReturnValue(true);
            };
        }
    }
}
