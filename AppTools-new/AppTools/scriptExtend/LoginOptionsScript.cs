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
    class LoginOptionsScript
    {
        /// <summary>
        /// 注册LoginOptionsScript对象
        /// </summary>
        /// <param name="kworld"></param>
        public static void registScript(JSObject kworld)
        {
            var LoginOptionsObj = kworld.AddObject("LoginOptions");
            insert(LoginOptionsObj);
            selectOptions(LoginOptionsObj);
            updateAutoLogin(LoginOptionsObj);
            updateRememberPwd(LoginOptionsObj);
        }
        /// <summary>
        /// 添加登录选项
        /// </summary>
        /// <param name="AppWorkObj"></param>
        public static void insert(JSObject LoginOptionsObj)
        {
            var insertOptionsFun = LoginOptionsObj.AddFunction("insert");
            insertOptionsFun.Execute += (func, args) =>
            {
                LoginOptions options = new LoginOptions();
                if (args.Arguments.Length == 4)
                {
                    options.userName = args.Arguments[0].StringValue;
                    options.password = args.Arguments[1].StringValue;
                    options.rememberPwd = args.Arguments[2].StringValue;
                    options.autoLogin = args.Arguments[3].StringValue;
                }
                else
                {
                    MessageBox.Show("参数错误");
                    args.SetReturnValue("[]");
                    return;
                }
                
                LoginOptionsDao.insert(options);
                MessageState state = new MessageState();
                state.state = "success";
                state.data = options;
                state.msg = "添加成功";
                string jsonStr = JSONUtil.ToJSON(state);
                args.SetReturnValue(jsonStr);
                return;
            };
        }
        /// <summary>
        /// 查询登录选项
        /// </summary>
        /// <param name="AppInfoObj"></param>
        public static void selectOptions(JSObject LoginOptionsObj)
        {
            var selectOptionsFun = LoginOptionsObj.AddFunction("selectOptions");
            selectOptionsFun.Execute += (func, args) =>
            {
                try
                {

                    LoginOptions options = LoginOptionsDao.getLoginOptions();
                    string jsonStr = JSONUtil.ToJSON(options);
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
        /// 更改自动登录
        /// </summary>
        /// <param name="AppInfoObj"></param>
        public static void updateAutoLogin(JSObject LoginOptionsObj)
        {
            var updateAutoLoginFun = LoginOptionsObj.AddFunction("updateAutoLogin");
            updateAutoLoginFun.Execute += (func, args) =>
            {
                try
                {
                    string autoLogin = "";
                    if (args.Arguments.Length < 1)
                    {
                        MessageBox.Show("参数错误");
                        args.SetReturnValue("{}");
                        return;
                    }
                    else
                    {
                        autoLogin = args.Arguments[0].StringValue;

                    }
                    int num = LoginOptionsDao.updateAutoLogin(autoLogin);
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
        /// <summary>
        /// 更改记住密码
        /// </summary>
        /// <param name="AppInfoObj"></param>
        public static void updateRememberPwd(JSObject LoginOptionsObj)
        {
            var updateRememberPwdFun = LoginOptionsObj.AddFunction("updateRememberPwd");
            updateRememberPwdFun.Execute += (func, args) =>
            {
                try
                {
                    string userName = "";
                    string password = "";
                    string rememberPwd = "";
                    if (args.Arguments.Length < 3)
                    {
                        MessageBox.Show("参数错误");
                        args.SetReturnValue("{}");
                        return;
                    }
                    else
                    {
                        userName = args.Arguments[0].StringValue;
                        password = args.Arguments[1].StringValue;
                        rememberPwd = args.Arguments[2].StringValue;

                    }
                    int num = LoginOptionsDao.updateRememberPwd(userName,password,rememberPwd);
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
