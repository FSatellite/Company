using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppTools.beans
{
    /// <summary>
    /// 小程序信息
    /// </summary>
    class AppInfo
    {
        public string id
        {
            set;
            get;
        }                  //程序id
        public string modelName
        {
            set;
            get;
        }            //程序名
        public string fmiVersion
        {
            set;
            get;
        }           //程序版本号
        public string modelIdentifier
        {
            set;
            get;
        }      //程序标识
        public string guid
        {
            set;
            get;
        }                 //程序的Guid编码
        public string description
        {
            set;
            get;
        }          //程序的简要概述
        public string author
        {
            set;
            get;
        }               //程序开发作者
        public string authorId
        {
            set;
            get;
        }             //程序开发者id
        public string generatationTool
        {
            set;
            get;
        }     //程序语言
        public string programType
        {
            set;
            get;
        }          //程序类型
        public string category
        {
            set;
            get;
        }             //专业分类
        public string categoryCode
        {
            set;
            get;
        }         //专业标识
        public string principleType
        {
            set;
            get;
        }        //原理依据类型
        public string principleTypeCode
        {
            set;
            get;
        }    //原理依据类型标识
        public string applicationType
        {
            set;
            get;
        }      //功能应用类型
        public string applicationTypeCode
        {
            set;
            get;
        }  //功能应用类型标识
        public string moduleCategory
        {
            set;
            get;
        }       //程序模块分类
        public string moduleCategoryCode
        {
            set;
            get;
        }   //程序模块分类标识
        public string publishTime
        {
            set;
            get;
        }          //程序发布时间
        public string copyright
        {
            set;
            get;
        }            //程序版本
        public string remark
        {
            set;
            get;
        }               //备注
        public string keyword
        {
            set;
            get;
        }              //关键词
        public string IfShare
        {
            set;
            get;
        }              //是否共享
        public string latestUsageTime
        {
            set;
            get;
        }      //程序最新使用时间

    }
}
