using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using AppTools.util;
using Chromium.WebBrowser;
using AppTools.beans;

namespace AppTools.remote
{
    class AppModel
    {
/// <summary>
        /// 注册IoOption对象
        /// </summary>
        /// <param name="kworld"></param>
        public static void registScript(JSObject kworld)
        {
            var AppModelObj = kworld.AddObject("AppModel");
            uploadModel(AppModelObj);
        }
        
        /// <summary>
        /// 上传程序包
        /// </summary>
        /// <param name="IoOptionObj"></param>
        public static void uploadModel(JSObject AppModelObj)
        {
            var saveTextFile = AppModelObj.AddFunction("uploadModel");
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
                        ZipFileUtil.ZipDir(path, appinfo.modelName + ".zip", 9);
                    }
                }
            };
        }
    }
}
