using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppTools.util;
using AppTools.beans;
using System.IO;
using System.Windows.Forms;
using AppTools.dao;

namespace AppTools.connector
{
    /// <summary>
    /// APP 连接器/接口
    /// </summary>
    class AppConnector
    {
        /// <summary>
        /// 应用APP包路径
        /// </summary>
        /// <param name="packagePath"></param>
        /// <returns></returns>
        public static bool publish(string packagePath) {
            try
            {
                AppInfo appInfo = new AppInfo();
                string result = FMIFileUtil.checkOut(packagePath, appInfo);
                if (result != null)
                {
                    MessageBox.Show(result);
                    return false;
                }
                //string libPath = AppDomain.CurrentDomain.BaseDirectory + "AppLib";
                string libPath = System.Windows.Forms.Application.StartupPath + "\\AppLib";
                appInfo.id = appInfo.guid;
                PackageUtil.copyDir(packagePath, libPath + "/" + appInfo.id);
                AppDao.insert(appInfo);
                return true;
            }catch(Exception ex){

                string exce = "";
                exce += ex.Message + "\r\n";
                exce += ex.Source + "\r\n";
                exce += ex.StackTrace + "\r\n";
                exce += ex.TargetSite + "\r\n";
                exce += ex.Data + "\r\n";

                MessageBox.Show(exce);

                return false;
            }
        }
    }
}
