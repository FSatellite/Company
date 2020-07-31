using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;
using Chromium.WebBrowser;
using AppTools.beans;
using AppTools.util;
using AppTools.dao;
using AppTools.remote;
using Microsoft.Win32;
using Ionic.Zip;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Common.Utility;
using DotNet.Utilities;

namespace AppTools.scriptExtend
{
    /// <summary>
    /// 基础文件操作
    /// </summary>
    class IoOption
    {
        static string copyPath = "";

        /// <summary>
        /// 注册IoOption对象
        /// </summary>
        /// <param name="kworld"></param>
        public static void registScript(JSObject kworld,string _copyPath)
        {
            copyPath = _copyPath;
            var IoOptionObj = kworld.AddObject("IoOption");
            choosePackage(IoOptionObj);
            saveXMLAndPublish(IoOptionObj);
            uploadModel(IoOptionObj);
            downloadModel(IoOptionObj);
            autoStart(IoOptionObj);
            createHtmlFile(IoOptionObj);
            CopyCompToPath(IoOptionObj);
        }

        public static void registScript(JSObject kworld)
        {
            var IoOptionObj = kworld.AddObject("IoOption");
            choosePackage(IoOptionObj);
            saveXMLAndPublish(IoOptionObj);
            uploadModel(IoOptionObj);
            downloadModel(IoOptionObj);
            autoStart(IoOptionObj);
            createHtmlFile(IoOptionObj);
            CopyCompToPath(IoOptionObj);
        }
        
        /// <summary>
        /// 选择程序包目录
        /// </summary>
        /// <param name="IoOptionObj"></param>
        public static void choosePackage(JSObject IoOptionObj)
        {
            var saveTextFile = IoOptionObj.AddFunction("choosePackage");
            saveTextFile.Execute += (func, args) =>
            {
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.Description = "请选择小程序包";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(dialog.SelectedPath))
                    {
                        //args.SetReturnValue("");
                    }
                    else
                    {
                        string path = dialog.SelectedPath;
                        DirectoryInfo TheFolder = new DirectoryInfo(path);
                        List<FileItem> packages = new List<FileItem>();
                        PackageUtil.findFiles(path, packages);
                        AppInfo appinfo = new AppInfo();
                        string checkResult = FMIFileUtil.checkOut(path, appinfo);
                        MessageState msg = new MessageState();
                        PackageInfo packageInfo = new PackageInfo();
                        packageInfo.path = path;
                        packageInfo.package = packages;
                        packageInfo.appInfo = appinfo;
                        msg.data = packageInfo;
                        msg.state = "success";
                        msg.msg = "经校验程序包符合规范";
                        if (checkResult != null)
                        {
                            msg.state = "error";
                            msg.msg = checkResult;
                        }
                        string jsonStr = JSONUtil.ToJSON(msg);
                        args.SetReturnValue(jsonStr);
                    }
                }
            };
        }

        /// <summary>
        /// 上传程序包
        /// </summary>
        /// <param name="IoOptionObj"></param>
        public static void uploadModel(JSObject IoOptionObj)
        {
            var saveTextFile = IoOptionObj.AddFunction("uploadModel");
            saveTextFile.Execute += (func, args) =>
            {
                if (args.Arguments.Length < 2)
                {
                    MessageState state1 = new MessageState();
                    state1.state = "error";
                    state1.msg = "参数不能为空";
                    args.SetReturnValue(JSONUtil.ToJSON(state1));
                    return;

                }
                string id = args.Arguments[0].StringValue;
                string x_auth_token = args.Arguments[1].StringValue;
                try
                {

                    //获取小程序文件夹名
                    AppInfo appInfo = AppDao.getAppInfo(id);
                    string guid = appInfo.guid;
                    //获取当前项目路径
                    string path = Directory.GetCurrentDirectory();
                    path += "\\AppLib\\" + guid + "\\" + guid;
                    DirectoryInfo TheFolder = new DirectoryInfo(path);
                    List<FileItem> packages = new List<FileItem>();
                    PackageUtil.findFiles(path, packages);
                    AppInfo appinfo = new AppInfo();
                    string checkResult = FMIFileUtil.checkOut(path, appinfo);
                    string filepath = path + ".zip";
                    //ZipFileUtil.ZipDir(path, filepath, 9);
                    DotNetZipFileUtil.ExeCompTwo(path, filepath);
                    String url = GloabConfig.knowledgeAppUrl+"model/upload?x-auth-token=" + x_auth_token;
                    FileItem item = new FileItem();
                    item.name = filepath.Substring(filepath.LastIndexOf("\\") + 1);
                    item.path = filepath;
                    MessageState state = UploadFile.uploadFile(item, appInfo, url);
                    if (state.state == "success")
                    {
                        //删除Zip文件
                        System.IO.File.Delete(item.path);
                    }
                    //args.SetReturnValue(JSONUtil.ToJSON(state));

                    MessageState state2 = new MessageState(); ;
                    state2.state = "success";
                    state2.msg = "共享成功";
                    args.SetReturnValue(JSONUtil.ToJSON(state2));
                    return;
                }
                catch (Exception e)
                {
                    MessageState state2 = new MessageState(); ;
                    state2.state = "error";
                    state2.msg = "共享失败";
                    state2.data = x_auth_token;
                    args.SetReturnValue(JSONUtil.ToJSON(state2));
                }
            };
        }

        /// <summary>
        /// 下载程序包
        /// </summary>
        /// <param name="IoOptionObj"></param>
        public static void downloadModel(JSObject IoOptionObj) {
            var saveTextFile = IoOptionObj.AddFunction("downloadModel");
            saveTextFile.Execute += (func, args) =>
            {

                MessageState state = new MessageState();
                if (args.Arguments.Length < 2)
                {
                    state.state = "error";
                    state.msg = "参数不能为空";
                    args.SetReturnValue(JSONUtil.ToJSON(state));
                    return;

                }
                string id = args.Arguments[0].StringValue;
                string x_auth_token = args.Arguments[1].StringValue;
                //根据id查询程序信息

                String url = GloabConfig.knowledgeAppUrl + "model/getById/" + id + "?x-auth-token=" + x_auth_token;
                HttpWebRequest webrequest = null;
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(SecurityValidate);
                    webrequest = WebRequest.Create(url) as HttpWebRequest;
                    webrequest.ProtocolVersion = HttpVersion.Version11;
                }
                else
                {
                    webrequest = WebRequest.Create(url) as HttpWebRequest;
                }
                webrequest.Method = "GET";
                webrequest.Timeout = 100000;
                var httpWebResponse = (HttpWebResponse)webrequest.GetResponse();
                string Content = null;
                using (Stream resStream = httpWebResponse.GetResponseStream())//得到回写的流
                {
                    StreamReader newReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8);
                    Content = newReader.ReadToEnd();
                    newReader.Close();
                }
                if (Content.Contains("success"))
                {
                    MessageState mst = JSONUtil.FromJSON<MessageState>(Content);
                    if (mst.data == null)
                    {
                        state.state = "error";
                        state.msg = "程序不存在或已删除";
                        args.SetReturnValue(JSONUtil.ToJSON(state));
                        return;
                    }
                    else {
                        AppInfo appInfo = JSONUtil.FromJSON<AppInfo>(JSONUtil.ToJSON(mst.data));
                        String guid = appInfo.guid;
                        string filePath = Application.StartupPath;
                        filePath += "\\AppLib\\" + guid + "\\";
                        MessageBox.Show(filePath);
                        string path1 = filePath + guid + ".zip";
                        String downloadUrl = GloabConfig.knowledgeAppUrl + "model/download/" + id + "?x-auth-token=" + x_auth_token;
                        bool flag = DownloadFile.HttpDownload(downloadUrl, path1);
                        if (flag)
                        {
                            try
                            {
                                //如果下载成功，删除temp文件夹
                               Directory.Delete(filePath + "temp", true);
                                //解压zip文件
                                //ZipFileUtil.UnZip(path1, "");                               
                                DotNetZipFileUtil.ExeAllDeComp(path1, "");
                                
                                //保存文件信息

                                AppInfo info = AppDao.getByGuid(guid);
                                if (info == null)
                                {
                                    AppDao.insert(appInfo);
                                }
                                else
                                {
                                    //更新
                                    AppDao.updateByGuid(appInfo);
                                }
                                state.state = "success";
                                state.msg = "程序下载成功";
                                state.data = appInfo;
                                args.SetReturnValue(JSONUtil.ToJSON(state));
                                return;
                            }
                            catch (Exception e)
                            {
                                state.state = "error";
                                state.msg = "程序下载失败111";
                                args.SetReturnValue(JSONUtil.ToJSON(e));
                                return;
                            }
                            finally {
                                //删除zip文件
                                File.Delete(path1);
                            }
                        }
                        else {
                            state.state = "error";
                            state.msg = "程序下载失败222";
                            args.SetReturnValue(JSONUtil.ToJSON(state));
                            return;
                        }
                        
                    }

                }
                else {
               
                        state.state = "error";
                        state.msg = "程序不存在或已删除";
                        args.SetReturnValue(JSONUtil.ToJSON(state));
                        return;
                   
                }
            };
        }
  /// <summary>
        /// 保存xml并发布程序
        /// </summary>
        /// <param name="IoOptionObj"></param>
        public static void saveXMLAndPublish(JSObject IoOptionObj)
        {
            var saveXMLAndPublish = IoOptionObj.AddFunction("saveXMLAndPublish");
            saveXMLAndPublish.Execute += (func, args) =>
            {
                try {
                    if (args.Arguments.Length < 2)
                    {
                        args.SetReturnValue(false);
                        return;
                    }
                    string packagePath = args.Arguments[0].ToString();
                    string data = args.Arguments[1].ToString();
                    //FileInfo xmlfile = new FileInfo(packagePath + "/Program.xml");
                    //if (!xmlfile.Exists)
                    //{
                    //    args.SetReturnValue(false);
                    //    return;
                    //}
                    AppInfo appInfo = (AppInfo)JSONUtil.FromJSON<AppInfo>(data);
                    //string result = FMIFileUtil.saveXML(xmlfile.FullName, appInfo);
                    //if (result != null)
                    //{
                    //    args.SetReturnValue(false);
                    //    return;
                    //}
                    //string libPath = AppDomain.CurrentDomain.BaseDirectory + "AppLib";
                    //appInfo.id = appInfo.guid;
                    //PackageUtil.copyDir(packagePath, libPath + "/" + appInfo.id);
                    AppDao.insert(appInfo);

                    args.SetReturnValue(true);

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
        /// 修改程序在注册表中的键值 
        /// </summary>
        /// <param name="IoOptionObj"></param>
        public static void autoStart(JSObject IoOptionObj)
        {
            var autoStartFun = IoOptionObj.AddFunction("autoStart");
            autoStartFun.Execute += (func, args) =>
            {
                if (args.Arguments.Length < 1)
                {
                    args.SetReturnValue(false);
                    return;
                }
                bool isAuto = args.Arguments[0].BoolValue;
                try
                {
                    //if (isAuto == true)
                    //{
                    //    RegistryKey R_local = Registry.LocalMachine;//RegistryKey R_local = Registry.CurrentUser;
                    //    RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    //    R_run.SetValue("应用名称", Application.ExecutablePath);
                    //    R_run.Close();
                    //    R_local.Close();
                    //}
                    //else
                    //{
                    //    RegistryKey R_local = Registry.LocalMachine;//RegistryKey R_local = Registry.CurrentUser;
                    //    RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    //    R_run.DeleteValue("应用名称", false);
                    //    R_run.Close();
                    //    R_local.Close();
                    //}

                    //GlobalVariant.Instance.UserConfig.AutoStart = isAuto;
                }
                catch (Exception)
                {
                    //MessageBoxDlg dlg = new MessageBoxDlg();
                    //dlg.InitialData("您需要管理员权限修改", "提示", MessageBoxButtons.OK, MessageBoxDlgIcon.Error);
                    //dlg.ShowDialog();
                    MessageBox.Show("您需要管理员权限修改", "提示");
                }
            };
        }

        public static void createHtmlFile(JSObject IoOptionObj)
        { 
         var createHtmlFileFun = IoOptionObj.AddFunction("createHtmlFile");
         createHtmlFileFun.Execute += (func, args) =>
         {
             if (args.Arguments.Length < 3)
             {
                 args.SetReturnValue(false);
                 return;
             }
             try
             {
                 string datapath = args.Arguments[0].StringValue;
                 string htmlStr = args.Arguments[1].StringValue;
                 string htmlPath = datapath + "\\help\\Abstract.html";
                 if (File.Exists(htmlPath))
                 {
                     //如果文件已经存在，则改名
                     File.Move(htmlPath, datapath + "\\help\\Abstract.html.back");
                 }
                 FileStream fileStream = new FileStream(htmlPath, FileMode.Create, FileAccess.Write);
                 StreamWriter sw = new StreamWriter(fileStream);
                 sw.WriteLine(htmlStr);
                 sw.Close();
                 File.Delete(datapath + "\\help\\Abstract.html.back");
                 args.SetReturnValue(true); 
             }catch(Exception ex)
             {
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
        public static bool SecurityValidate(object sender,
                            X509Certificate certificate,
                            X509Chain chain,
                           SslPolicyErrors errors)
        {
            //直接确认，不然打不开，会出现超时错误
            return true;
        }

        /// <summary>
        /// 将组件拷贝到工具解析器指定的路径
        /// </summary>
        public static void CopyCompToPath(JSObject IoOptionObj)
        {
            var copyFileFunc = IoOptionObj.AddFunction("copyFile");
            copyFileFunc.Execute += (func, args) =>
            {
                try
                {
                    string guid = args.Arguments[0].ToString();
                    //组件所在路径
                    string filePath = System.Windows.Forms.Application.StartupPath + "\\AppLib\\" + guid + "\\" + guid;
                    CopyDirectory(filePath, copyPath);
                    //将所选组件信息写入文件，方便工具解析器获取
                    WriteGUID(Application.StartupPath,guid);
                    args.SetReturnValue(true);
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    args.SetReturnValue(false);
                }
            };
        }

        private static void CopyDirectory(string srcdir, string desdir)
        {
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);
            string desfolderdir = desdir + "\\" + folderName;
            if (desdir.LastIndexOf("\\") == (desdir.Length - 1))
            {
                desfolderdir = desdir + folderName;
            }

            //判断组件是否存在
            if (Directory.Exists(desfolderdir))
            {
                DirFile.ClearDirectory(desfolderdir);
                DirFile.DeleteDirectory(desfolderdir);
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);
 
            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }
                    CopyDirectory(file, desfolderdir);
                }
                else // 否则直接copy文件
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);
                    srcfileName = desfolderdir + "\\" + srcfileName;
                    if (!Directory.Exists(desfolderdir))
                    {
                        Directory.CreateDirectory(desfolderdir);
                    }
                    File.Copy(file, srcfileName,true);
                }
            }
        }

        private static void WriteGUID(string path,string guid)
        {
            try
            {
                if (File.Exists(path + "\\GUID.txt"))
                    File.Delete(path + "\\GUID.txt");
                FileStream fs = new FileStream(path + "\\GUID.txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(guid);
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
