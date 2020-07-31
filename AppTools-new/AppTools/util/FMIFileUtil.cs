using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AppTools.beans;
using System.Xml;
using System.Windows.Forms;

namespace AppTools.util
{
    /// <summary>
    /// FMI文件处理
    /// </summary>
    class FMIFileUtil
    {
        /// <summary>
        /// 检查XML文件
        /// </summary>
        /// <param name="packagePath"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string checkOut(string packagePath, AppInfo app)
        {
            FileInfo xml = new FileInfo(packagePath + "/Program.xml");

            if (!xml.Exists)
            {
                return "未找到程序描述文件Program.xml";
            }
            XmlElement xmlNodeRoot;
            XmlDocument xmlDocument;
            try
            {
                xmlDocument = new XmlDocument();
                using (Stream stream = new FileStream(xml.FullName, FileMode.Open, FileAccess.Read))
                {
                    xmlDocument.Load(stream);
                    xmlNodeRoot = xmlDocument.DocumentElement;
                    XmlAttributeCollection ac = xmlNodeRoot.Attributes;

                    app.modelName = ac["modelName"].Value;
                    app.modelIdentifier = ac["modelIdentifier"].Value;
                    app.fmiVersion = ac["fmiVersion"].Value;
                    app.guid = ac["guid"].Value;
                    app.description = ac["description"].Value;
                    app.author = ac["author"].Value;
                    app.generatationTool = ac["generationTool"].Value;
                    app.programType = ac["programType"].Value;
                    app.category = ac["category"].Value;
                    app.categoryCode = ac["categoryCode"].Value;
                    app.principleType = ac["principleType"].Value;
                    app.principleTypeCode = ac["principleTypeCode"].Value;
                    app.applicationType = ac["applicationType"].Value;
                    app.applicationTypeCode = ac["applicationTypeCode"].Value;
                    app.moduleCategory = ac["moduleCategory"].Value;
                    app.moduleCategoryCode = ac["moduleCategoryCode"].Value;
                    app.publishTime = ac["publishTime"].Value;
                    app.publishTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); 
                    app.copyright = ac["copyright"].Value;
                    app.remark = ac["remark"].Value;
                    app.keyword = ac["keyword"].Value;

                    stream.Close();
                }
                if (app.guid == null || "".Equals(app.guid))
                {
                    return "guid不能为空";
                }
                return null;
            }
            catch (Exception e)
            {
                string exce = "";
                exce += e.Message + "\r\n";
                exce += e.Source + "\r\n";
                exce += e.StackTrace + "\r\n";
                exce += e.TargetSite + "\r\n";
                exce += e.Data + "\r\n";
                MessageBox.Show(AppWin.gloabForm, "Program.xml文件不符合规范，导致错误："+exce, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "Program.xml文件不符合规范";
            }
        }
         /// <summary>
        /// 保存xml
        /// </summary>
        /// </summary>
        /// <param name="xml""th"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public static string saveXML(string xmlPath,AppInfo app) {
             FileInfo xml = new FileInfo(xmlPath);
            if (!xml.Exists)
            {
                MessageBox.Show(AppWin.gloabForm, "xml文件不存在，请检查", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "xml文件不存在，请检查";
            }
            XmlElement xmlNodeRoot;
            XmlDocument xmlDocument;
            try
            {
                xmlDocument = new XmlDocument();
                using (Stream stream = new FileStream(xml.FullName, FileMode.Open, FileAccess.ReadWrite))
                {
                    xmlDocument.Load(stream);
                    xmlNodeRoot = xmlDocument.DocumentElement;
                    XmlAttributeCollection ac = xmlNodeRoot.Attributes;

                    ac["modelName"].Value=app.modelName ;
                    ac["modelIdentifier"].Value = app.modelIdentifier;
                    ac["fmiVersion"].Value = app.fmiVersion;
                    ac["description"].Value = app.description;
                    ac["author"].Value = app.author;
                    ac["generationTool"].Value = app.generatationTool;
                    ac["programType"].Value = app.programType;
                    ac["category"].Value = app.category;
                    ac["categoryCode"].Value = app.categoryCode;
                    ac["principleType"].Value = app.principleType;
                    ac["principleTypeCode"].Value = app.principleTypeCode;
                    ac["applicationType"].Value = app.applicationType;
                    ac["applicationTypeCode"].Value = app.applicationTypeCode;
                    ac["moduleCategory"].Value = app.moduleCategory;
                    ac["moduleCategoryCode"].Value = app.moduleCategoryCode;
                    ac["remark"].Value = app.remark;
                    ac["keyword"].Value = app.keyword;

                    //stream.Close();
                }
                xmlDocument.Save(xmlPath);
            }catch(Exception e){
                string exce = "";
                exce += e.Message + "\r\n";
                exce += e.Source + "\r\n";
                exce += e.StackTrace + "\r\n";
                exce += e.TargetSite + "\r\n";
                exce += e.Data + "\r\n";
                MessageBox.Show(AppWin.gloabForm, exce, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return exce;
            }
            return null;
        }
    }
}
