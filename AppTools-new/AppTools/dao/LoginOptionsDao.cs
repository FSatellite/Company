using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppTools.beans;
using System.Data.SQLite;
using System.Reflection;
using System.Windows.Forms;

namespace AppTools.dao
{
    class LoginOptionsDao
    {
        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <returns></returns>
        public static int createTable() {

            string sql = "CREATE TABLE IF NOT EXISTS ZYCT_LOGINOPTIONS (userName text, password text,rememberPwd text default '0',autoLogin text default '0')";

        SQLiteCommand command = SqliteHelper.CreateCommand(sql);

         return command.ExecuteNonQuery();
        }
        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int insert(LoginOptions loginOptions)
        {
            StringBuilder pre_ = new StringBuilder();
            pre_.Append("replace into ZYCT_LOGINOPTIONS (");

            StringBuilder pro_ = new StringBuilder();
            pro_.Append(" VALUES (");
            int i = 0;
            foreach (PropertyInfo p in loginOptions.GetType().GetProperties())
            {
                i++;
                if (p.GetValue(loginOptions, null) != null)
                {
                    pre_.Append(p.Name);
                    pro_.Append("'" + p.GetValue(loginOptions, null) + "'");
                    if (i < loginOptions.GetType().GetProperties().Length)
                    {
                        pre_.Append(",");
                        pro_.Append(",");
                    }
                }
            }
            pre_.Append(") ");
            pro_.Append(")");
            string sql = (pre_.ToString() + pro_.ToString()).Replace(",)",")");
            SQLiteCommand command = new SQLiteCommand(sql, SqliteHelper.getConnection());

            return command.ExecuteNonQuery();
        }

     
        /// <summary>
        /// 查询登录选项
        /// </summary>
        /// <returns></returns>
        public static LoginOptions getLoginOptions()
        {
            LoginOptions loginOptions = null;
            string sql = "SELECT * from ZYCT_LOGINOPTIONS"; 
            SQLiteCommand command = new SQLiteCommand(sql, SqliteHelper.getConnection());
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                loginOptions = new LoginOptions();
                foreach (PropertyInfo p in loginOptions.GetType().GetProperties())
                {
                    if (reader[p.Name].GetType() != typeof(System.DBNull))
                    {
                        p.SetValue(loginOptions, reader[p.Name], null);
                    }
                }

                
            }
            return loginOptions;
        }
         /// <summary>
        /// 更改自动登录
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static int updateAutoLogin(string autoLogin)
        {
            SQLiteCommand command = new SQLiteCommand("update ZYCT_LOGINOPTIONS set autoLogin= '" + autoLogin + "'", SqliteHelper.getConnection());
            return command.ExecuteNonQuery();
        }
        /// <summary>
        /// 更改记住密码
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static int updateRememberPwd(string userName, string password, string rememberPwd)
        {
            SQLiteCommand command = new SQLiteCommand("update ZYCT_LOGINOPTIONS set userName= '" + userName + "',password = '"+password+"',rememberPwd ='"+rememberPwd+"'", SqliteHelper.getConnection());
            return command.ExecuteNonQuery();
        }
    }
}
