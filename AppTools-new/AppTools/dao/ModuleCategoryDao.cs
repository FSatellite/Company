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
    /// <summary>
    /// 程序分类Dao
    /// </summary>
    class ModuleCategoryDao
    {
        public static int createTable() {
            string sql = "CREATE TABLE IF NOT EXISTS ZYCT_ModuleCategory (moduleCategoryCode text PRIMARY KEY NOT NULL,moduleCategory text)";

            SQLiteCommand command = SqliteHelper.CreateCommand(sql);

            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int insert(ModuleCategory moduleCategory)
        {
            StringBuilder pre_ = new StringBuilder();
            pre_.Append("replace into ZYCT_ModuleCategory (");

            StringBuilder pro_ = new StringBuilder();
            pro_.Append(" VALUES (");
            int i = 0;
            foreach (PropertyInfo p in moduleCategory.GetType().GetProperties())
            {
                i++;
                if (p.GetValue(moduleCategory, null) != null)
                {
                    pre_.Append(p.Name);
                    pro_.Append("'" + p.GetValue(moduleCategory, null) + "'");
                    if (i < moduleCategory.GetType().GetProperties().Length)
                    {
                        pre_.Append(",");
                        pro_.Append(",");
                    }
                }
            }
            pre_.Append(") ");
            pro_.Append(")");
            string sql = (pre_.ToString() + pro_.ToString()).Replace(",)", ")");
            SQLiteCommand command = new SQLiteCommand(sql, SqliteHelper.getConnection());

            return command.ExecuteNonQuery();
        }
        /// <summary>
        /// 查询所有分类
        /// </summary>
        /// <returns></returns>
        public static List<ModuleCategory> getAll()
        {
            try
            {
                List<ModuleCategory> list = new List<ModuleCategory>();
                string sql = "SELECT  moduleCategory,moduleCategoryCode from ZYCT_MODULECATEGORY";
                SQLiteCommand command = new SQLiteCommand(sql, SqliteHelper.getConnection());
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ModuleCategory modelCategory = new ModuleCategory();
                    foreach (PropertyInfo p in modelCategory.GetType().GetProperties())
                    {
                        if (reader[p.Name].GetType() != typeof(System.DBNull))
                        {
                            p.SetValue(modelCategory, reader[p.Name], null);
                        }
                    }
                    list.Add(modelCategory);
                }
                return list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }

}
