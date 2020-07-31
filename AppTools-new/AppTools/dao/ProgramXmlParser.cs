using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppTools.beans;
using System.Xml;
namespace AppTools.dao
{
    class ProgramXmlParser
    {
        public static AppInfo GetAppInfo(string guid)
        {
            try
            {
                AppInfo appInfo = new AppInfo();
                XmlDocument doc = new XmlDocument();
                doc.Load(System.Windows.Forms.Application.StartupPath + "\\AppLib\\" + guid + "\\" + guid + "\\Program.xml");
                XmlElement root = doc.DocumentElement;

                //获取小程序属性
                appInfo.id = root.GetAttribute("guid");
                appInfo.modelName = root.GetAttribute("modelName");
                appInfo.modelIdentifier = root.GetAttribute("modelIdentifier");
                appInfo.fmiVersion = root.GetAttribute("fmiVersion");
                appInfo.guid = root.GetAttribute("guid");
                appInfo.description = root.GetAttribute("description");
                appInfo.author = root.GetAttribute("author");
                appInfo.programType = root.GetAttribute("programType");
                appInfo.category = root.GetAttribute("category");
                appInfo.categoryCode = root.GetAttribute("categoryCode");
                appInfo.principleType = root.GetAttribute("principleType");
                appInfo.principleTypeCode = root.GetAttribute("principleTypeCode");
                appInfo.applicationType = root.GetAttribute("applicationType");
                appInfo.applicationTypeCode = root.GetAttribute("applicationTypeCode");
                appInfo.moduleCategory = root.GetAttribute("moduleCategory");
                appInfo.moduleCategoryCode = root.GetAttribute("moduleCategoryCode");
                appInfo.publishTime = root.GetAttribute("publishTime");
                appInfo.copyright = root.GetAttribute("copyright");
                appInfo.remark = root.GetAttribute("remark");
                appInfo.keyword = root.GetAttribute("keyword");

                return appInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
