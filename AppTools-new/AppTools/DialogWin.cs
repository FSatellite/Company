using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AppTools.scriptExtend;
using AppTools.util;


namespace AppTools
{
    using Chromium;
    using Chromium.WebBrowser;
    using Chromium.Remote;
    using NetDimension.NanUI;

    public partial class DialogWin : Formium
    {
        bool openDev = false;
        public static DialogWin gloabDialogWin = null;

        public DialogWin(string url, string openDev, int width, int height)
            : base(url)
        {
            InitializeComponent();
            this.ClientSize = new System.Drawing.Size(width, height);
            Rectangle rect = Screen.GetWorkingArea(this);
            if (DialogWin.gloabDialogWin == null)
            {
                DialogWin.gloabDialogWin = this;
                DialogWin.gloabDialogWin.FormClosing += new FormClosingEventHandler(DialogWin_closeing);
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

        private void regisScript()
        {
            var kworld = GlobalObject.AddObject("Kworld");
            IoOption.registScript(kworld);
            Window.registScript(kworld);
            AppInfoScript.registScript(kworld);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show(this, e.KeyCode + "", "调用提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void LoadHandler_OnLoadEnd(object sender, Chromium.Event.CfxOnLoadEndEventArgs e)
        {
            // Check if it is the main frame when page has loaded.
            this.ExecuteJavascript("config.AppToolsClient_Addr='" + GloabConfig.serverUrl + "'");
            if (e.Frame.IsMain && openDev)
            {
                //Open DevTools window to watch js console output messages.
                Chromium.ShowDevTools();
            }
        }

        private void DialogWin_closeing(object sender, FormClosingEventArgs e)
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
            DialogWin.gloabDialogWin.FormClosing -= new FormClosingEventHandler(DialogWin_closeing);
            //this.Dispose();
        }
    }
}
