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
    class AppInfoScript
    {
        /// <summary>
        /// 注册AppInfoScript对象
        /// </summary>
        /// <param name="kworld"></param>
        public static void registScript(JSObject kworld)
        {
            var AppInfoObj = kworld.AddObject("AppInfo");
            selectRecentUse(AppInfoObj);
            selectRecentUseModule(AppInfoObj);
            selectMouduleCategory(AppInfoObj);
            selectPageByModuleCategoryCode(AppInfoObj);
            delete(AppInfoObj);
            search(AppInfoObj);
            selectNativeApp(AppInfoObj);
            getAllModels(AppInfoObj);
            getByGuid(AppInfoObj);
            getByCategoryAndName(AppInfoObj);
            updateIfShare(AppInfoObj);
            updateLastUseTime(AppInfoObj);
        }

        /// <summary>
        /// 查询最近在用的
        /// </summary>
        /// <param name="AppInfoObj"></param>
        public static void selectRecentUse(JSObject AppInfoObj)
        {
            var selectRecentUseFun = AppInfoObj.AddFunction("selectRecentUse");
            selectRecentUseFun.Execute += (func, args) =>
            {
                int num=3;
                string applicationTypeCode = "all";
                if(args.Arguments.Length>0){
                    num=args.Arguments[0].IntValue;
                }
                if (args.Arguments.Length > 1)
                {
                    applicationTypeCode = args.Arguments[1].ToString();
                }
                List<AppInfo> list = AppDao.selectRecentUse(num, applicationTypeCode);
                string jsonStr = JSONUtil.ToJSON(list);
                args.SetReturnValue(jsonStr);
            };
        }

        /// <summary>
        /// 查询最近在用的
        /// </summary>
        /// <param name="AppInfoObj"></param>
        public static void selectRecentUseModule(JSObject AppInfoObj)
        {
            var selectRecentUseFun = AppInfoObj.AddFunction("selectRecentUseModule");
            selectRecentUseFun.Execute += (func, args) =>
            {
                int num = 3;
                string applicationTypeCode = "all";
                if (args.Arguments.Length > 0)
                {
                    num = args.Arguments[0].IntValue;
                }
                if (args.Arguments.Length > 1)
                {
                    applicationTypeCode = args.Arguments[1].ToString();
                }
                List<AppInfo> list = AppDao.selectRecentUseModule(num, applicationTypeCode);
                string jsonStr = JSONUtil.ToJSON(list);
                args.SetReturnValue(jsonStr);
            };
        }

        /// <summary>
        /// 查询所有程序
        /// </summary>
        /// <param name="AppInfoObj"></param>
        public static void getAllModels(JSObject AppInfoObj)
        {
            var selectMouduleCategoryFun = AppInfoObj.AddFunction("getAllModels");
            selectMouduleCategoryFun.Execute += (func, args) =>
            {
                string applicationTypeCode = "all";
                if (args.Arguments.Length > 0)
                {
                    applicationTypeCode = args.Arguments[0].ToString();
                }
                List<AppInfo> list = AppDao.getAllModels(applicationTypeCode);
                string jsonStr = JSONUtil.ToJSON(list);
                args.SetReturnValue(jsonStr);
            };
        }

        /// <summary>
        /// 查询模块分类
        /// </summary>
        /// <param name="AppInfoObj"></param>
        public static void selectMouduleCategory(JSObject AppInfoObj)
        {
            var selectMouduleCategoryFun = AppInfoObj.AddFunction("selectMouduleCategory");
            selectMouduleCategoryFun.Execute += (func, args) =>
            {
                string applicationTypeCode = "all";
                if (args.Arguments.Length >1)
                {
                    applicationTypeCode = args.Arguments[0].ToString();
                }
                List<ModuleCategory> list = AppDao.selectMouduleCategory(applicationTypeCode);
                string jsonStr = JSONUtil.ToJSON(list);
                args.SetReturnValue(jsonStr);
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AppInfoObj"></param>
        public static void delete(JSObject AppInfoObj)
        {
            var deleteFun = AppInfoObj.AddFunction("delete");
            deleteFun.Execute += (func, args) =>
            {
                try
                {
                if (args.Arguments.Length <2)
                {
                    MessageBox.Show("id不能为空");
                    args.SetReturnValue(false);
                    return;
                }
                string id=args.Arguments[0].ToString();
                string guid = args.Arguments[1].ToString();
                int num=AppDao.delete(id);
                if (num > 0)
                {
                    //string libPath = AppDomain.CurrentDomain.BaseDirectory + "AppLib";
                    string libPath = System.Windows.Forms.Application.StartupPath + "\\AppLib";
                    Directory.Delete(libPath + "/" + guid,true);
                    args.SetReturnValue(true);
                }
                else
                {
                    MessageBox.Show("未找到目标对象！");
                    args.SetReturnValue(false);
                }
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
        /// 搜索
        /// </summary>
        /// <param name="AppInfoObj"></param>
        public static void search(JSObject AppInfoObj)
        {
           var searchFun = AppInfoObj.AddFunction("search");
           searchFun.Execute += (func, args) =>
           {
               int pageNum = 1;
               int pageSize = 10;
               string applicationTypeCode = "all";
               string keyword = "";
               if (args.Arguments.Length < 4)
               {
                   MessageBox.Show("参数错误，int pageNum, int pageSize,string applicationType, string keyword");
                   args.SetReturnValue("[]");
                   return;
               }
               else
               {
                   pageNum = args.Arguments[0].IntValue;
                   pageSize = args.Arguments[1].IntValue;
                   applicationTypeCode = args.Arguments[2].ToString();
                   keyword = args.Arguments[3].ToString();
               }
               PageModel<AppInfo> pagemodel = AppDao.search(pageNum, pageSize, applicationTypeCode, keyword);
               string jsonStr = JSONUtil.ToJSON(pagemodel);
               args.SetReturnValue(jsonStr);
           };
        }
        /// <summary>
        /// 根据模块分类和功能分类进行分页查询
        /// </summary>
        /// <param name="AppInfoObj"></param>
        public static void selectPageByModuleCategoryCode(JSObject AppInfoObj)
        {
         var selectByModuleCategoryCodeFun = AppInfoObj.AddFunction("selectPageByModuleCategoryCode");
         selectByModuleCategoryCodeFun.Execute += (func, args) =>
         {
             try
             {
                 int pageNum = 1;
                 int pageSize = 10;
                 string applicationTypeCode = "all";
                 string moduleCategoryCode = "";
                 if (args.Arguments.Length < 4)
                 {
                     MessageBox.Show("参数错误，int pageNum, int pageSize,string applicationType, string moduleCategoryCode");
                     args.SetReturnValue("[]");
                     return;
                 }
                 else
                 {
                     pageNum = args.Arguments[0].IntValue;
                     pageSize= args.Arguments[1].IntValue;
                     applicationTypeCode = args.Arguments[2].ToString();
                     moduleCategoryCode = args.Arguments[3].ToString();
                 }
                 PageModel<AppInfo> pagemodel=AppDao.selectByModuleCategoryCode(pageNum,pageSize,applicationTypeCode,moduleCategoryCode);
                 string jsonStr = JSONUtil.ToJSON(pagemodel);
                 args.SetReturnValue(jsonStr);
             }catch(Exception ex){
                 string exce = "";
                 exce += ex.Message + "\r\n";
                 exce += ex.Source + "\r\n";
                 exce += ex.StackTrace + "\r\n";
                 exce += ex.TargetSite + "\r\n";
                 exce += ex.Data + "\r\n";

                 AppWin.gloabForm.showMessage(exce, MessageBoxButtons.OK, MessageBoxIcon.Error);
                 args.SetReturnValue("[]");
             }
         };
        }
        /// <summary>
        /// 查询bending应用程序
        /// </summary>
        /// <param name="AppInfoObj"></param>
        public static void selectNativeApp(JSObject AppInfoObj)
        {
            var selectByModuleCategoryCodeFun = AppInfoObj.AddFunction("selectNativeApp");
            selectByModuleCategoryCodeFun.Execute += (func, args) =>
            {
                try {
                    int pageNum = 1;
                    int pageSize = 10;
                    string modelName="";

                    if (args.Arguments.Length < 2)
                    {
                        MessageBox.Show("参数错误，int pageNum, int pageSize不能为空");
                        args.SetReturnValue("{}");
                        return;
                    }
                    else
                    {
                        pageNum = args.Arguments[0].IntValue;
                        pageSize = args.Arguments[1].IntValue;
                        if (args.Arguments.Length > 2)
                        {
                            modelName = args.Arguments[2].StringValue;
                        }
                    }
                    PageModel<AppInfo> pagemodel = AppDao.selectNative(pageNum, pageSize,modelName);
                    string jsonStr = JSONUtil.ToJSON(pagemodel);
                    args.SetReturnValue(jsonStr);
                }catch(Exception ex){
                    string exce = "";
                    exce += ex.Message + "\r\n";
                    exce += ex.Source + "\r\n";
                    exce += ex.StackTrace + "\r\n";
                    exce += ex.TargetSite + "\r\n";
                    exce += ex.Data + "\r\n";

                    AppWin.gloabForm.showMessage(exce, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    args.SetReturnValue("{}");
                }
            };
        }
        /// <summary>
        /// 根据guid查询程序
        /// </summary>
        /// <param name="AppInfoObj"></param>
        public static void getByGuid(JSObject AppInfoObj)
        {
            var selectByModuleCategoryCodeFun = AppInfoObj.AddFunction("getByGuid");
            selectByModuleCategoryCodeFun.Execute += (func, args) =>
            {
                try
                {
                    string guid = "";
                    if (args.Arguments.Length < 1)
                    {
                        MessageBox.Show("参数错误，string guid不能为空");
                        args.SetReturnValue("{}");
                        return;
                    }
                    else
                    {                       
                         guid = args.Arguments[0].StringValue;
                        
                    }
                    AppInfo appInfo = AppDao.getByGuid(guid);
                    string jsonStr = JSONUtil.ToJSON(appInfo);
                    args.SetReturnValue(jsonStr);
                }
                catch (Exception ex)
                {
                    string exce = "";
                    exce += ex.Message + "\r\n";
                    exce += ex.Source + "\r\n";
                    exce += ex.StackTrace + "\r\n";
                    exce += ex.TargetSite + "\r\n";
                    exce += ex.Data + "\r\n";

                    AppWin.gloabForm.showMessage(exce, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    args.SetReturnValue("{}");
                }
            };
        }

        /// <summary>
        /// 根据专业和名称查询程序
        /// </summary>
        /// <param name="AppInfoObj"></param>
        public static void getByCategoryAndName(JSObject AppInfoObj)
        {
            var selectByModuleCategoryCodeFun = AppInfoObj.AddFunction("getByCategoryAndName");
            selectByModuleCategoryCodeFun.Execute += (func, args) =>
            {
                try
                {
                    string categoryCode = "";
                    string modelName = "";
                    if (args.Arguments.Length < 2)
                    {
                        MessageBox.Show("参数错误，string categoryCode,string modelName不能为空");
                        args.SetReturnValue("{}");
                        return;
                    }
                    else
                    {
                        categoryCode = args.Arguments[0].StringValue;
                        modelName = args.Arguments[1].StringValue;

                    }
                    AppInfo appInfo = AppDao.getByCategoryAndName(categoryCode,modelName);
                    string jsonStr = JSONUtil.ToJSON(appInfo);
                    args.SetReturnValue(jsonStr);
                }
                catch (Exception ex)
                {
                    string exce = "";
                    exce += ex.Message + "\r\n";
                    exce += ex.Source + "\r\n";
                    exce += ex.StackTrace + "\r\n";
                    exce += ex.TargetSite + "\r\n";
                    exce += ex.Data + "\r\n";

                    AppWin.gloabForm.showMessage(exce, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    args.SetReturnValue("{}");
                }
            };
        }

        /// <summary>
        /// 更改程序的共享状态
        /// </summary>
        /// <param name="AppInfoObj"></param>
        public static void updateIfShare(JSObject AppInfoObj)
        {
            var updateIfShareFun = AppInfoObj.AddFunction("updateIfShare");
            updateIfShareFun.Execute += (func, args) =>
            {
                try
                {
                    string id = "";
                    string ifShare = "";
                    if (args.Arguments.Length < 2)
                    {
                        MessageBox.Show("参数错误，string categoryCode,string modelName不能为空");
                        args.SetReturnValue("{}");
                        return;
                    }
                    else
                    {
                        id = args.Arguments[0].StringValue;
                        ifShare = args.Arguments[1].StringValue;

                    }
                    int num = AppDao.updateIfShare(id, ifShare);
                    args.SetReturnValue(num+"");
                }
                catch (Exception ex)
                {
                    string exce = "";
                    exce += ex.Message + "\r\n";
                    exce += ex.Source + "\r\n";
                    exce += ex.StackTrace + "\r\n";
                    exce += ex.TargetSite + "\r\n";
                    exce += ex.Data + "\r\n";

                    AppWin.gloabForm.showMessage(exce, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    args.SetReturnValue("{}");
                }
            };
        }

        /// <summary>
        /// 更改程序最后使用时间
        /// </summary>
        /// <param name="AppInfoObj"></param>
        public static void updateLastUseTime(JSObject AppInfoObj)
        {
            var updateLastUseTimeFun = AppInfoObj.AddFunction("updateLastUseTime");
            updateLastUseTimeFun.Execute += (func, args) =>
            {
                try
                {
                    string id = "";
                    string lastUseTime = "";
                    if (args.Arguments.Length < 2)
                    {
                        MessageBox.Show("参数错误，id,lastUseTime不能为空");
                        args.SetReturnValue("{}");
                        return;
                    }
                    else
                    {
                        id = args.Arguments[0].StringValue;
                        lastUseTime = args.Arguments[1].StringValue;

                    }
                    int num = AppDao.updateLastUseTime(id, lastUseTime);
                    args.SetReturnValue(num + "");
                }
                catch (Exception ex)
                {
                    string exce = "";
                    exce += ex.Message + "\r\n";
                    exce += ex.Source + "\r\n";
                    exce += ex.StackTrace + "\r\n";
                    exce += ex.TargetSite + "\r\n";
                    exce += ex.Data + "\r\n";

                    AppWin.gloabForm.showMessage(exce, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    args.SetReturnValue("{}");
                }
            };
        }
    }
}
