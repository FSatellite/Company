using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppTools.beans
{
    /// <summary>
    /// 登录选项
    /// </summary>
    class LoginOptions
    {
        public string userName
        {
            set;
            get;
        }                  //用户名
        public string password
        {
            set;
            get;
        }            //密码
        public string rememberPwd
        {
            set;
            get;
        }              //是否记住密码：0-否，1-是
        public string autoLogin
        {
            set;
            get;
        }      //自动登录：0-否，1-是

    }
}
