using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AppTools.util;
using AppTools.beans;
using System.IO;

namespace AppTools
{
    using Chromium;
    using Chromium.WebBrowser;
    using Chromium.Remote;
    using NetDimension.NanUI;

    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                string token = "";
                string copyPath = "";
                if (args.Length == 1)
                {
                    token = args[0];
                    args = null;
                }
                else if (args.Length == 2)
                {
                    token = args[1];
                    copyPath = args[0];
                    args = null;
                }
                if (args == null || args.Length == 0)
                {
                    GloabConfig.readConfigFile();
                    args = new string[2];
                    args[0] = GloabConfig.serverUrl;
                    args[1] = GloabConfig.devModel + "";
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //指定CEF架构和文件目录结构，并初始化CEF
                if (Bootstrap.Load(settings =>
                {
                    //禁用日志
                    settings.LogSeverity = Chromium.CfxLogSeverity.Disable;

                    //指定中文为当前CEF环境的默认语言
                    settings.AcceptLanguageList = "zh-CN";
                    settings.Locale = "zh-CN";

                    //注册嵌入资源，并为指定资源指定一个假的域名knowind.shareWroks.local
                    //Bootstrap.RegisterAssemblyResources(System.Reflection.Assembly.GetExecutingAssembly(), "knowind.shareWroks.local");

                }, commandLine =>
                {
                    //在启动参数中添加disable-web-security开关，禁用跨域安全检测    
                    commandLine.AppendSwitch("disable-web-security");
                    commandLine.AppendSwitch("disable-infobars");
                    commandLine.AppendSwitch("allow-file-acess-from-files");
                    commandLine.AppendSwitch("enable-file-cookies");
                    commandLine.AppendSwitch("ignore-certificate-errors");
                }))
                {
                    AppWin appWind = new AppWin(args[0] + "/index.html", args[1], GloabConfig.width, GloabConfig.height,copyPath);
                    Application.Run(appWind);
                }

            }
            catch (Exception ex)
            {

                string exce = "";
                exce += ex.Message + "\r\n";
                exce += ex.Source + "\r\n";
                exce += ex.StackTrace + "\r\n";
                exce += ex.TargetSite + "\r\n";
                exce += ex.Data + "\r\n";

                MessageBox.Show(null, exce, "运行错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //string path = "D:\\Apprun\\ZhuQiVolume";
            //DirectoryInfo TheFolder = new DirectoryInfo(path);
            //List<FileItem> packages = new List<FileItem>();
            //PackageUtil.findFiles(path, packages);
            //AppInfo appinfo=new AppInfo();
            //string checkResult = FMIFileUtil.checkOut(path, TheFolder.Name,appinfo);
            //MessageState msg = new MessageState();
            //PackageInfo packageInfo = new PackageInfo();
            //packageInfo.package = packages;
            //packageInfo.appInfo = appinfo; 
            //msg.data = packageInfo;
            //msg.state = "success";
            //msg.msg = "";
            //if(checkResult!=null){
            //    msg.state = "error";
            //    msg.msg = checkResult;
            //}
            //string jsonStr = JSONUtil.ToJSON(msg);
            //MessageBox.Show(jsonStr);
        }
    }
}
