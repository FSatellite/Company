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
    class AppDao
    {
        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <returns></returns>
        public static int createTable() {

            string sql = "CREATE TABLE IF NOT EXISTS ZYCT_APP (id text PRIMARY KEY NOT NULL,modelName text, fmiVersion text," +
            "modelIdentifier text,guid text,description text,author text,authorId text,generatationTool text," +
            "programType text,category text,categoryCode text,principleType text,principleTypeCode text," +
            "applicationType text,applicationTypeCode text,moduleCategory text,moduleCategoryCode text," +
            "publishTime text,copyright text,remark text,keyword text,IfShare text default '0',latestUsageTime text)";

        SQLiteCommand command = SqliteHelper.CreateCommand(sql);

         return command.ExecuteNonQuery();
        }
        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int insert(AppInfo model)
        {
            StringBuilder pre_ = new StringBuilder();
            pre_.Append("replace into ZYCT_APP (");

            StringBuilder pro_ = new StringBuilder();
            pro_.Append(" VALUES (");
            int i = 0;
            foreach (PropertyInfo p in model.GetType().GetProperties())
            {
                i++;
                if (p.GetValue(model, null) != null)
                {
                    pre_.Append(p.Name);
                    pro_.Append("'" + p.GetValue(model, null) + "'");
                    if (i < model.GetType().GetProperties().Length)
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
      /// 根据功能分类查询最近使用
      /// </summary>
      /// <param name="num"></param>
      /// <param name="applicationTypeCode"></param>
      /// <returns></returns>
        public static List<AppInfo> selectRecentUse(int num, string applicationTypeCode)
        {
            List<AppInfo> list = new List<AppInfo>();
            string sql = "select * from ZYCT_APP ";
            if (!"all".Equals(applicationTypeCode) && !"".Equals(applicationTypeCode))
            {
                sql += " where applicationTypeCode='" + applicationTypeCode + "'";
            }
            sql += "order by latestUsageTime desc limit " + num + " offset 0";
            
            SQLiteCommand command = new SQLiteCommand(sql, SqliteHelper.getConnection());
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                AppInfo model = new AppInfo();
                foreach (PropertyInfo p in model.GetType().GetProperties())
                {
                    if (reader[p.Name].GetType() != typeof(System.DBNull))
                    {
                        p.SetValue(model, reader[p.Name], null);
                    }

                }
                list.Add(model);
            }
            // Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);
            return list;
        }

        /// <summary>
        /// 查询最近使用的程序
        /// </summary>
        /// <param name="num"></param>
        /// <param name="applicationTypeCode"></param>
        /// <returns></returns>
        public static List<AppInfo> selectRecentUseModule(int num, string applicationTypeCode)
        {
            List<AppInfo> list = new List<AppInfo>();
            string sql = "select * from ZYCT_APP where latestUsageTime is not null ";
            if (!"all".Equals(applicationTypeCode) && !"".Equals(applicationTypeCode))
            {
                sql += " and applicationTypeCode='" + applicationTypeCode + "'";
            }
            sql += "order by latestUsageTime desc limit " + num + " offset 0";

            SQLiteCommand command = new SQLiteCommand(sql, SqliteHelper.getConnection());
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                AppInfo model = new AppInfo();
                foreach (PropertyInfo p in model.GetType().GetProperties())
                {
                    if (reader[p.Name].GetType() != typeof(System.DBNull))
                    {
                        p.SetValue(model, reader[p.Name], null);
                    }

                }
                list.Add(model);
            }
            // Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);
            return list;
        }
        /// <summary>
        /// 查询分类
        /// </summary>
        /// <returns></returns>
        public static List<ModuleCategory> selectMouduleCategory(string applicationType)
        {
            List<ModuleCategory> list = new List<ModuleCategory>();
            string sql = "SELECT DISTINCT moduleCategory,moduleCategoryCode from ZYCT_APP where 1=1";
            if (!"all".Equals(applicationType) && !"".Equals(applicationType))
            {
                sql += " and applicationTypeCode='" + applicationType + "'";
            }
            SQLiteCommand command = new SQLiteCommand(sql, SqliteHelper.getConnection());
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ModuleCategory model = new ModuleCategory();
                model.moduleCategory = reader["moduleCategory"].ToString();
                model.moduleCategoryCode = reader["moduleCategoryCode"].ToString();
                
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 查询所有程序
        /// </summary>
        /// <returns></returns>
        public static List<AppInfo> getAllModels(string applicationTypeCode)
        {
            List<AppInfo> list = new List<AppInfo>();
            string sql = "SELECT * from ZYCT_APP";
            if (!"all".Equals(applicationTypeCode) && !"".Equals(applicationTypeCode))
            {
                sql += " where applicationTypeCode = '"+applicationTypeCode +"'";
            }
            
            SQLiteCommand command = new SQLiteCommand(sql, SqliteHelper.getConnection());
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                AppInfo appInfo = new AppInfo();
                foreach (PropertyInfo p in appInfo.GetType().GetProperties())
                {
                    if (reader[p.Name].GetType() != typeof(System.DBNull))
                    {
                        p.SetValue(appInfo, reader[p.Name], null);
                    }
                }

                list.Add(appInfo);
            }
            return list;
        }


        /// <summary>
        /// 根据分类进行分页查询
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="applicationTypeCode"></param>
        /// <param name="moduleCategoryCode"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static PageModel<AppInfo> selectByModuleCategoryCode(int pageNum, int pageSize,
               string applicationTypeCode, string moduleCategoryCode)
        {
            PageModel<AppInfo> pageModel = new PageModel<AppInfo>();
            pageModel.pageNum = pageNum;
            pageModel.pageSize = pageSize;

            List<AppInfo> list = new List<AppInfo>();
            int offset = pageSize * (pageNum - 1);
            int total = 0;

            string countSql = "select count(id) total from ZYCT_APP where  moduleCategoryCode='" + moduleCategoryCode + "'";
            if (!"all".Equals(applicationTypeCode) && !"".Equals(applicationTypeCode))
            {
                countSql += " and applicationTypeCode='" + applicationTypeCode + "'";
            }
            SQLiteCommand command1 = new SQLiteCommand(countSql, SqliteHelper.getConnection());
            SQLiteDataReader reader1 = command1.ExecuteReader();
            while (reader1.Read())
            {
                total = int.Parse(reader1["total"].ToString());
            }
            pageModel.totalPage=total/pageSize;
            if (total % pageSize>0)
            {
                pageModel.totalPage += 1;
            }

            string sql = "select * from ZYCT_APP where moduleCategoryCode='" + moduleCategoryCode + "' ";
                if (!"all".Equals(applicationTypeCode) && !"".Equals(applicationTypeCode))
            {
                sql += (" and applicationTypeCode='" + applicationTypeCode + "'");
            }
                sql +=(" limit " + pageSize + " offset " + offset);
            SQLiteCommand command = new SQLiteCommand(sql, SqliteHelper.getConnection());
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                AppInfo model = new AppInfo();
                foreach (PropertyInfo p in model.GetType().GetProperties())
                {
                    if (reader[p.Name].GetType() != typeof(System.DBNull))
                    {
                        p.SetValue(model, reader[p.Name], null);
                    }
                }
                list.Add(model);
            }
            pageModel.list = list;
            return pageModel;
        }
        /// <summary>
        /// 根据应用类型和用户输入进行搜索
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="applicationTypeCode"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static PageModel<AppInfo> search(int pageNum, int pageSize,
               string applicationTypeCode,string keywords)
        {
            PageModel<AppInfo> pageModel = new PageModel<AppInfo>();
            pageModel.pageNum = pageNum;
            pageModel.pageSize = pageSize;

            List<AppInfo> list = new List<AppInfo>();
            int offset = pageSize * (pageNum - 1);
            int total = 0;

            string countSql = "select count(id) total from ZYCT_APP where  (modelName like '%" + keywords 
                + "%') ";
            if (!"all".Equals(applicationTypeCode) && !"".Equals(applicationTypeCode))
            {
                countSql += " and applicationTypeCode='" + applicationTypeCode + "'";
            }
            SQLiteCommand command1 = new SQLiteCommand(countSql, SqliteHelper.getConnection());
            SQLiteDataReader reader1 = command1.ExecuteReader();
            while (reader1.Read())
            {
                total = int.Parse(reader1["total"].ToString());
            }
            pageModel.totalPage = total / pageSize;
            if (total % pageSize > 0)
            {
                pageModel.totalPage += 1;
            }

            string sql = "select * from ZYCT_APP where  (modelName like '%" + keywords
                + "%') ";
            if (!"all".Equals(applicationTypeCode) && !"".Equals(applicationTypeCode))
            {
                sql += (" and applicationTypeCode='" + applicationTypeCode + "'");
            }
            sql += (" limit " + pageSize + " offset " + offset);
            SQLiteCommand command = new SQLiteCommand(sql, SqliteHelper.getConnection());
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                AppInfo model = new AppInfo();
                foreach (PropertyInfo p in model.GetType().GetProperties())
                {
                    if (reader[p.Name].GetType() != typeof(System.DBNull))
                    {
                        p.SetValue(model, reader[p.Name], null);
                    }
                }
                list.Add(model);
            }
            pageModel.list = list;
            return pageModel;
        }
        /// <summary>
        /// 查询本地应用程序
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="applicationTypeCode"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static PageModel<AppInfo> selectNative(int pageNum, int pageSize, String modelName)
        {
            PageModel<AppInfo> pageModel = new PageModel<AppInfo>();
            pageModel.pageNum = pageNum;
            pageModel.pageSize = pageSize;

            List<AppInfo> list = new List<AppInfo>();
            int offset = pageSize * (pageNum - 1);
            int total = 0;

            string countSql = "select count(id) total from ZYCT_APP where id = guid";
           

            if (modelName != "")
            {
                countSql += " and modelName like '%"+modelName+"%'";
            }

            SQLiteCommand command1 = new SQLiteCommand(countSql, SqliteHelper.getConnection());
            SQLiteDataReader reader1 = command1.ExecuteReader();
            while (reader1.Read())
            {
                total = int.Parse(reader1["total"].ToString());
            }
            pageModel.totalPage = total / pageSize;
            if (total % pageSize > 0)
            {
                pageModel.totalPage += 1;
            }

            string sql = "select * from ZYCT_APP where id = guid";
            if (modelName != "")
            {
                sql += " and modelName like '%" + modelName + "%'";
            }

            sql += ("  order by publishtime desc limit " + pageSize + " offset " + offset);
            SQLiteCommand command = new SQLiteCommand(sql, SqliteHelper.getConnection());
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                AppInfo model = new AppInfo();
                foreach (PropertyInfo p in model.GetType().GetProperties())
                {
                    if (reader[p.Name].GetType() != typeof(System.DBNull))
                    {
                        p.SetValue(model, reader[p.Name], null);
                    }
                }
                list.Add(model);
            }
            pageModel.list = list;
            return pageModel;
        }
        /// <summary>
        /// 获取小程序详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static AppInfo getAppInfo(string id)
        {
            AppInfo appInfo = null;
            SQLiteCommand command = new SQLiteCommand("select * from ZYCT_APP where id='" + id + "'", SqliteHelper.getConnection());
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                appInfo = new AppInfo();
                foreach (PropertyInfo p in appInfo.GetType().GetProperties())
                {
                    if (reader[p.Name].GetType() != typeof(System.DBNull))
                    {
                        p.SetValue(appInfo, reader[p.Name], null);
                    }
                }
            }
            return appInfo;
        }
        /// <summary>
        /// 删除工具
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int delete(string id) {
            string sql = "delete from ZYCT_APP where id='" + id + "'";

            SQLiteCommand command = SqliteHelper.CreateCommand(sql);

            return command.ExecuteNonQuery();
        }
        /// <summary>
        /// 根据guid查询程序
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static AppInfo getByGuid(string guid)
        {
            AppInfo appInfo = null;
            SQLiteCommand command = new SQLiteCommand("select * from ZYCT_APP where guid='" + guid + "'", SqliteHelper.getConnection());
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                appInfo = new AppInfo();
                foreach (PropertyInfo p in appInfo.GetType().GetProperties())
                {
                    if (reader[p.Name].GetType() != typeof(System.DBNull))
                    {
                        p.SetValue(appInfo, reader[p.Name], null);
                    }
                }
            }
            return appInfo;
        }

        /// <summary>
        /// 根据专业和名称查询程序
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static AppInfo getByCategoryAndName(string categoryCode,string modelName)
        {
            AppInfo appInfo = null;
            SQLiteCommand command = new SQLiteCommand("select * from ZYCT_APP where categoryCode='" + categoryCode + "' and modelName = '"+modelName +"'", SqliteHelper.getConnection());
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                appInfo = new AppInfo();
                foreach (PropertyInfo p in appInfo.GetType().GetProperties())
                {
                    if (reader[p.Name].GetType() != typeof(System.DBNull))
                    {
                        p.SetValue(appInfo, reader[p.Name], null);
                    }
                }
            }
            return appInfo;
        }

        /// <summary>
        /// 更改程序程序共享状态
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static int updateIfShare(string id, string ifShare)
        {
            SQLiteCommand command = new SQLiteCommand("update ZYCT_APP set IfShare = '"+ifShare+"' where id='" + id + "'", SqliteHelper.getConnection());
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// 更改程序最后使用时间
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static int updateLastUseTime(string id, string lastUseTime)
        {
            SQLiteCommand command = new SQLiteCommand("update ZYCT_APP set latestUsageTime = '" + lastUseTime + "' where id='" + id + "'", SqliteHelper.getConnection());
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// 根据guid更改程序
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static int updateByGuid(AppInfo info)
        {
            string str = "id = '" + info.id + "',modelName = '" + info.modelName + "',fmiVersion='" + info.fmiVersion + "',modelIdentifier='" + info.modelIdentifier + "',description = '" +
                info.description + "',author='" + info.author + "',authorId='" + info.authorId + "',generatationTool='"+info.generatationTool+"',programType='"+info.programType+
                "',category='"+info.category+"',categoryCode='"+info.categoryCode+"',principleType='"+info.principleType+"',principleTypeCode='"+info.principleTypeCode+
                "',applicationType='" + info.applicationType + "',applicationTypeCode='" + info.applicationTypeCode + "',moduleCategory='" + info.moduleCategory + "',moduleCategoryCode='"+
                info.moduleCategoryCode + "',publishTime='" + info.publishTime + "',copyright='" + info.copyright + "',remark='" + info.remark + "',keyword='"+info.keyword+"'";
            SQLiteCommand command = new SQLiteCommand("update ZYCT_APP set "+str+" where guid='" + info.guid + "'", SqliteHelper.getConnection());
            return command.ExecuteNonQuery();
        }
    }
}
