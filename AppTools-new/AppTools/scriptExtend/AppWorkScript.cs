using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chromium.WebBrowser;
using AppTools.beans;
using AppTools.util;
using AppTools.dao;
using System.IO;
using System.Windows.Forms;

namespace AppTools.scriptExtend
{
    class AppWorkScript
    {
        /// <summary>
        /// 注册AppInfoScript对象
        /// </summary>
        /// <param name="kworld"></param>
        public static void registScript(JSObject kworld)
        {
            var AppWorkObj = kworld.AddObject("AppWork");
            run(AppWorkObj);
            newApp(AppWorkObj);
            editApp(AppWorkObj);
            testApp(AppWorkObj);
        }
        /// <summary> 
        /// 运行小程序
        /// </summary>
        /// <param name="AppWorkObj"></param>
        public static void run(JSObject AppWorkObj)
        {
            var runFun = AppWorkObj.AddFunction("run");
            runFun.Execute += (func, args) =>
            {
                try {
                    if(args.Arguments.Length<1){
                        AppWin.gloabForm.showMessage("小程序id不能为空", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        args.SetReturnValue(false);
                    }
                    string id=args.Arguments[0].ToString();
                    AppInfo app=AppDao.getAppInfo(id);
                    if(app==null){
                        AppWin.gloabForm.showMessage("未找到小程序，请确认id是否正确", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        args.SetReturnValue(false);
                    }
                    //string path = AppDomain.CurrentDomain.BaseDirectory + "AppLib";
                    string path = System.Windows.Forms.Application.StartupPath + "\\AppLib";
                    DirectoryInfo dir = new DirectoryInfo(path+"/"+id);
                    if (dir.Exists)
                    {
                        foreach (DirectoryInfo NextFolder in dir.GetDirectories())
                        {
                            string executer = (string)ExecuterEnum.table[app.applicationTypeCode];
                            System.Diagnostics.Process process = new System.Diagnostics.Process();
                            //process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "ExecuterLib\\" + executer + "\\" + executer+".exe";
                            process.StartInfo.FileName = System.Windows.Forms.Application.StartupPath + "\\ExecuterLib\\" + executer + "\\" + executer + ".exe";
                            process.StartInfo.Arguments = NextFolder.FullName;
                            process.Start();
                        }
                    }
                    args.SetReturnValue(true);
                    Application.Exit();
                }catch(Exception ex){
                    string exce = "";
                    exce += ex.Message + "\r\n";
                    exce += ex.Source + "\r\n";
                    exce += ex.StackTrace + "\r\n";
                    exce += ex.TargetSite + "\r\n";
                    exce += ex.Data + "\r\n";

                    AppWin.gloabForm.showMessage(exce, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    args.SetReturnValue(false);
                }
            };
        }

        /// <summary>
        /// 新建组件
        /// </summary>
        /// <param name="AppWorkObj"></param>
        public static void newApp(JSObject AppWorkObj)
        {
            var newFun = AppWorkObj.AddFunction("newApp");
            newFun.Execute += (func, args) =>
            {
                try
                {
                    //MessageBox.Show("test");
                    string guid = Guid.NewGuid().ToString();
                    //新建组件路径
                    if (!System.IO.Directory.Exists(System.Windows.Forms.Application.StartupPath + "\\AppLib\\" + guid))
                        System.IO.Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "\\AppLib\\" + guid);
                    string compPath = System.Windows.Forms.Application.StartupPath + "\\AppLib\\" + guid;
                    //IDE路径
                    string idePath = System.Windows.Forms.Application.StartupPath + "\\TSCompilerCfg";
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    //process.StartInfo.WorkingDirectory = idePath;
                    process.StartInfo.FileName = idePath + "\\TSCompilerCfg.exe";
                    process.StartInfo.Arguments = " new" + " " + guid + " " + compPath;
                    process.Start();
                    process.WaitForExit();

                    //获取组件信息
                    AppInfo appInfo = ProgramXmlParser.GetAppInfo(guid);
                    string jsonStr = JSONUtil.ToJSON(appInfo);
                    //返回组件的信息
                    args.SetReturnValue(jsonStr);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    args.SetReturnValue(false);
                }
            };
        }

        /// <summary>
        /// 编辑组件
        /// </summary>
        /// <param name="AppWorkObj"></param>
        public static void editApp(JSObject AppWorkObj)
        {
            var editFun = AppWorkObj.AddFunction("editApp");
            editFun.Execute += (func, args) =>
            {
                try
                {
                    string guid = args.Arguments[0].ToString();
                    //string guid = "2A2834B5-FC6D-48D1-87DE-105C328A4543";
                    //组件路径
                    string compPath = System.Windows.Forms.Application.StartupPath + "\\AppLib\\" + guid;
                    //IDE路径
                    string idePath = System.Windows.Forms.Application.StartupPath + "\\TSCompilerCfg";
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.WorkingDirectory = idePath;
                    process.StartInfo.FileName = "TSCompilerCfg.exe";
                    process.StartInfo.Arguments = " open" + " " + guid + " " + compPath;
                    process.Start();
                    process.WaitForExit();
                    process.Close();
                    //返回组件的信息
                    args.SetReturnValue(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    args.SetReturnValue(false);
                }
            };
        }

        //验证程序
        public static void testApp(JSObject AppWorkObj)
        {
            var testFun = AppWorkObj.AddFunction("testApp");
            testFun.Execute += (func, args) =>
            {
                try
                {
                    string guid = args.Arguments[0].ToString();
                    string compPath = Application.StartupPath + "\\AppLib\\" + guid + "\\" + guid;

                    //工具解析器路径
                    string shareWorksPath = Application.StartupPath;
                    int nPos = shareWorksPath.LastIndexOf('\\');
                    shareWorksPath = shareWorksPath.Substring(0, nPos);
                    string toolResolver = shareWorksPath + "\\ToolResolver\\ToolResolver.exe";
                    toolResolver = @"C:\ShareWorks\ToolResolver\ToolResolver.exe";

                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = toolResolver;
                    process.StartInfo.Arguments = compPath;
                    process.Start();
                    process.Close();

                    args.SetReturnValue(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    args.SetReturnValue(false);
                }
            };
        }
    }
}
