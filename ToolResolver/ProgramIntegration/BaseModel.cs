using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ProgramIntegration
{
    public class BaseModel
    {
        public BaseModel(){}
        public BaseModel(string xmlPath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlPath);
                XmlElement root = doc.DocumentElement;

                this.applicationTypeCode = root.GetAttribute("applicationTypeCode");
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// 程序名
        /// </summary>
        public string modelName;

        /// <summary>
        /// 程序标识
        /// </summary>
        public string modelIdentifier;

        /// <summary>
        /// 程序版本号
        /// </summary>
        public string fmiVersion;

        /// <summary>
        /// 程序的Guid编码
        /// </summary>
        public string guid;

        /// <summary>
        /// 程序的简要概述
        /// </summary>
        public string description;

        /// <summary>
        /// 程序开发作者
        /// </summary>
        public string author;

        /// <summary>
        /// 程序语言
        /// </summary>
        public string generationTool;

        /// <summary>
        /// 程序类型
        /// </summary>
        public string programType;

        /// <summary>
        /// 专业分类
        /// </summary>
        public string category;

        /// <summary>
        /// 专业分类英文代号
        /// </summary>
        public string categoryCode;

        /// <summary>
        /// 原理依据类型
        /// </summary>
        public string principleType;

        /// <summary>
        /// 原理依据类型代号
        /// </summary>
        public string principleTypeCode;

        /// <summary>
        /// 功能应用类型
        /// </summary>
        public string applicationType;

        /// <summary>
        /// 功能应用代号
        /// </summary>
        public string applicationTypeCode;

        /// <summary>
        /// 程序模块分类
        /// </summary>
        public string moduleCategory;

        /// <summary>
        /// 程序模块分类代号
        /// </summary>
        public string moduleCategoryCode;

        /// <summary>
        /// 程序发布时间
        /// </summary>
        public string publishTime;

        /// <summary>
        /// 程序版权
        /// </summary>
        public string copyright;

        /// <summary>
        /// 备注
        /// </summary>
        public string remark;

        /// <summary>
        /// 关键词
        /// </summary>
        public string keyword;
    }
}
