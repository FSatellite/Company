using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AppTools.scriptExtend;
using AppTools.dao;
using AppTools.util;
using AppTools.remote;

namespace AppTools
{
    using Chromium;
    using Chromium.WebBrowser;
    using Chromium.Remote;
    using NetDimension.NanUI;

    public partial class AppWin : Formium
    {
        bool openDev = false;
        public static AppWin gloabForm = null;
        public string copyPath;

        public AppWin(string url,string openDev,int width,int height,string copyPath)
            : base(url)
        {
            this.copyPath = copyPath;
            InitializeComponent();
            //-----------初始化数据库------------------------
              AppDao.createTable();
            //-----------------------------------------------
            this.ClientSize = new System.Drawing.Size(width, height);
            Rectangle rect = Screen.GetWorkingArea(this);
            Point p = new Point(5, rect.Height - height+8);
            this.Location = p;
            if (AppWin.gloabForm == null)
            {
                AppWin.gloabForm = this;
                AppWin.gloabForm.FormClosing += new FormClosingEventHandler(AppWin_closeing);
            }
            this.KeyPreview = true;
            //
            //
            if (openDev != null)
            {
                this.openDev = Boolean.Parse(openDev);
            }
            regisScript();
            this.KeyDown += new KeyEventHandler(this.Form1_KeyDown);
            LoadHandler.OnLoadEnd += LoadHandler_OnLoadEnd;
           
        }
        /// <summary>
        /// 注册javaScript脚本函数
        /// </summary>
        private void regisScript() {
            var kworld = GlobalObject.AddObject("Kworld");
            IoOption.registScript(kworld,this.copyPath);
            Window.registScript(kworld);
            AppInfoScript.registScript(kworld);
            AppWorkScript.registScript(kworld);
            AppModel.registScript(kworld);
            ModuleCategoryScript.registScript(kworld);
            LoginOptionsScript.registScript(kworld);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show(this, e.KeyCode + "", "调用提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void LoadHandler_OnLoadEnd(object sender, Chromium.Event.CfxOnLoadEndEventArgs e)
        {
            string callTool = "";
            if (this.copyPath == "")
                callTool = "ShareWorks";
            else
                callTool = "ToolResolver";
            this.ExecuteJavascript("config.AppToolsClient_Addr='" + GloabConfig.serverUrl + "';" + "config.callTool='" + callTool + "'");
            // Check if it is the main frame when page has loaded.
            if (e.Frame.IsMain && openDev)
            {
                //Open DevTools window to watch js console output messages.
                Chromium.ShowDevTools();
            }
        }

        public delegate void showMessageCallBack(string msg, MessageBoxButtons but, MessageBoxIcon ico);

        public void showMessage(string msg, MessageBoxButtons but, MessageBoxIcon ico)
        {
            // InvokeRequired需要比较调用线程ID和创建线程ID
            // 如果它们不相同则返回true
            if (this.InvokeRequired)
            {
                showMessageCallBack d = new showMessageCallBack(showMessage);
                this.Invoke(d, new Object[] { msg, but, ico });
            }
            else
            {
                MessageBox.Show(this, msg, "提示", but, ico);
            }
        }

        private void AppWin_closeing(object sender, FormClosingEventArgs e)
        {
            // 注意判断关闭事件reason来源于窗体按钮，否则用菜单退出时无法退出!
            //if (e.CloseReason == CloseReason.UserClosing)
            //{
            //    //取消"关闭窗口"事件
            //    e.Cancel = true; // 取消关闭窗体 

            //    //使关闭时窗口向右下角缩小的效果
            //    this.WindowState = FormWindowState.Minimized;
            //    //this.notifyIcon1.Visible = true;
            //    //this.m_cartoonForm.CartoonClose();
            //    this.Hide();
            //    return;
            //}
            //MessageBox.Show("关闭");
            AppWin.gloabForm.FormClosing -= new FormClosingEventHandler(AppWin_closeing);
            Application.Exit();
        }
    }
}
