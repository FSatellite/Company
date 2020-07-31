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
    class ModuleCategoryScript
    {
        /// <summary>
        /// 注册AppInfoScript对象
        /// </summary>
        /// <param name="kworld"></param>
        public static void registScript(JSObject kworld)
        {
            var ModuleCategoryObj = kworld.AddObject("ModuleCategory");
            selectMouduleCategory(ModuleCategoryObj);
            selectPageByModuleCategoryCode(ModuleCategoryObj);
            delete(ModuleCategoryObj);
            search(ModuleCategoryObj);
        }

        /// <summary>
        /// 查询模块分类
        /// </summary>
        /// <param name="AppInfoObj"></param>
        public static void selectMouduleCategory(JSObject ModuleCategoryObj)
        {
            var selectMouduleCategoryFun = ModuleCategoryObj.AddFunction("selectMouduleCategory");
            selectMouduleCategoryFun.Execute += (func, args) =>
            {
                string applicationTypeCode = "all";
                if (args.Arguments.Length >1)
                {
                    applicationTypeCode = args.Arguments[0].ToString();
                }
                List<ModuleCategory> list = ModuleCategoryDao.getAll();
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
                if (args.Arguments.Length <1)
                {
                    MessageBox.Show("id不能为空");
                    args.SetReturnValue(false);
                    return;
                }
                string id=args.Arguments[0].ToString();
                int num=AppDao.delete(id);
                if (num > 0)
                {
                    //string libPath = AppDomain.CurrentDomain.BaseDirectory + "AppLib";
                    string libPath = System.Windows.Forms.Application.StartupPath + "\\AppLib";
                    Directory.Delete(libPath + "/" + id,true);
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
    }
}
