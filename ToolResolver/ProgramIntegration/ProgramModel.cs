using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ProgramIntegration
{
    /// <summary>
    /// 小程序XML模型
    /// </summary>
    public class ProgramModel : BaseModel
    {
        public ProgramModel(string xmlPath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlPath);
                XmlElement root = doc.DocumentElement;

                //获取小程序属性
                this.modelName = root.GetAttribute("modelName");
                this.modelIdentifier = root.GetAttribute("modelIdentifier");
                this.fmiVersion = root.GetAttribute("fmiVersion");
                this.guid = root.GetAttribute("guid");
                this.description = root.GetAttribute("description");
                this.author = root.GetAttribute("author");
                this.generationTool = root.GetAttribute("generationTool");
                this.programType = root.GetAttribute("programType");
                this.category = root.GetAttribute("category");
                this.categoryCode = root.GetAttribute("categoryCode");
                this.principleType = root.GetAttribute("principleType");
                this.principleTypeCode = root.GetAttribute("principleTypeCode");
                this.applicationType = root.GetAttribute("applicationType");
                this.applicationTypeCode = root.GetAttribute("applicationTypeCode");
                this.moduleCategory = root.GetAttribute("moduleCategory");
                this.moduleCategoryCode = root.GetAttribute("moduleCategoryCode");
                this.publishTime = root.GetAttribute("publishTime");
                this.copyright = root.GetAttribute("copyright");
                this.remark = root.GetAttribute("remark");
                this.keyword = root.GetAttribute("keyword");

                //获取参数集合
                XmlNode ModelVariablesNode = root.SelectSingleNode("ModelVariables");
                foreach (XmlNode paramNode in ModelVariablesNode.ChildNodes)
                {
                    ScalarVariable scalarVariable = new ScalarVariable();
                    XmlElement paramElement = (XmlElement)paramNode;
                    scalarVariable.name = paramElement.GetAttribute("name");
                    scalarVariable.displayName = paramElement.GetAttribute("displayName");
                    scalarVariable.dataType = paramElement.GetAttribute("dataType");
                    scalarVariable.valueReference = paramElement.GetAttribute("valueReference");
                    scalarVariable.dataFlow = paramElement.GetAttribute("dataFlow");
                    scalarVariable.description = paramElement.GetAttribute("description");
                    scalarVariable.format = paramElement.GetAttribute("format");
                    scalarVariable.unit = paramElement.GetAttribute("unit");
                    scalarVariable.variability = paramElement.GetAttribute("variability");
                    scalarVariable.causality = paramElement.GetAttribute("causality");
                    scalarVariable.keyMap = paramElement.GetAttribute("keyMap");
                    scalarVariable.tagImage = paramElement.GetAttribute("tagImage");
                    modelVariables.Add(scalarVariable);
                }
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// 参数集合
        /// </summary>
        public List<ScalarVariable> modelVariables = new List<ScalarVariable>();
    }

    /// <summary>
    /// 参数模型
    /// </summary>
    public class ScalarVariable
    {
        /// <summary>
        /// 变量名称
        /// </summary>
        public string name;

        /// <summary>
        /// 显示名称
        /// </summary>
        public string displayName;

        /// <summary>
        /// 变量的数据类型
        /// </summary>
        public string dataType;

        /// <summary>
        /// 变量默认参考值
        /// </summary>
        public string valueReference;

        /// <summary>
        /// 变量数据流向
        /// </summary>
        public string dataFlow;

        /// <summary>
        /// 变量详细描述
        /// </summary>
        public string description;

        /// <summary>
        /// 变量数据格式
        /// </summary>
        public string format;

        /// <summary>
        /// 变量物理单位
        /// </summary>
        public string unit;

        /// <summary>
        /// 变量可变性
        /// </summary>
        public string variability;

        /// <summary>
        /// 变量性质
        /// </summary>
        public string causality;

        /// <summary>
        /// 参数引索
        /// </summary>
        public string keyMap;

        /// <summary>
        /// 关联图片名称
        /// </summary>
        public string tagImage;
    }
}
