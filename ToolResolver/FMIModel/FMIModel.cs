using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using IniParser;

namespace FMIModel
{
    /// <summary>
    /// XML模型
    /// </summary>
    public class FMIModel
    {
        /// <summary>
        /// FMI版本
        /// </summary>
        public string fmiVersion;

        /// <summary>
        /// 模型名称
        /// </summary>
        public string modelName;

        /// <summary>
        /// 程序的Guid编码
        /// </summary>
        public string guid;

        /// <summary>
        /// 模型的简要概况
        /// </summary>
        public string description;

        /// <summary>
        /// 作者的名称或组织
        /// </summary>
        public string author;

        /// <summary>
        /// 模型的可选版本
        /// </summary>
        public string version;

        /// <summary>
        /// 有关此FMU的知识产权版权的可选信息
        /// </summary>
        public string copyright;

        /// <summary>
        /// 有关此FMU的知识产权许可的可选信息
        /// </summary>
        public string license;

        /// <summary>
        /// 生成XML文件的工具的可选名称
        /// </summary>
        public string generationTool;

        /// <summary>
        /// 日期
        /// </summary>
        public string generationDateAndTime;

        /// <summary>
        /// 枚举变量。-flat：扁平化变量名称-structured：带有“.”的分层结构变量名称。
        /// </summary>
        public string VariableNamingConvention;

        /// <summary>
        /// 工具类型
        /// </summary>
        public string toolType;

        /// <summary>
        /// 工具guid
        /// </summary>
        public string toolId;

        /// <summary>
        /// 参数集合
        /// </summary>
        public List<ScalarVariableModel> ScalarVariables = new List<ScalarVariableModel>();

        public FMIModel(string path)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                //doc.Load("G:\\ToolResolver\\modelDescription.xml");
                doc.Load(path);
                //获取根节点  获取模型属性
                XmlElement root = doc.DocumentElement;
                this.fmiVersion = root.GetAttribute("fmiVersion");
                this.modelName = root.GetAttribute("modelName");
                this.guid = root.GetAttribute("guid");
                this.description = root.GetAttribute("description");
                this.author = root.GetAttribute("author");
                this.version = root.GetAttribute("version");
                this.copyright = root.GetAttribute("copyright");
                this.license = root.GetAttribute("license");
                this.generationTool = root.GetAttribute("generationTool");
                this.generationDateAndTime = root.GetAttribute("generationDateAndTime");
                this.VariableNamingConvention = root.GetAttribute("VariableNamingConvention");
                this.toolType = root.GetAttribute("toolType");
                this.toolId = root.GetAttribute("toolId");

                //获取参数集合
                XmlNode modelVariablesNode = root.SelectSingleNode("ModelVariables");
                foreach (XmlNode scalarVariableNode in modelVariablesNode.ChildNodes)   
                {
                    ScalarVariableModel scalarVariable = new ScalarVariableModel();
                    XmlElement nodeElement = (XmlElement)scalarVariableNode;
                    scalarVariable.id = nodeElement.GetAttribute("id");
                    scalarVariable.name = nodeElement.GetAttribute("name");
                    scalarVariable.valueReference = nodeElement.GetAttribute("valueReference");
                    scalarVariable.causality = nodeElement.GetAttribute("causality");
                    scalarVariable.variability = nodeElement.GetAttribute("variability");
                    scalarVariable.description = nodeElement.GetAttribute("description");
                    scalarVariable.displayName = nodeElement.GetAttribute("displayName");
                    scalarVariable.attachFile = nodeElement.GetAttribute("attachFile");
                    scalarVariable.version = nodeElement.GetAttribute("version");

                    //获取参数值
                    foreach (XmlNode valueNode in scalarVariableNode.ChildNodes)
                    {
                        XmlElement valueElment = (XmlElement)valueNode;
                        scalarVariable.scalarVariableValue.declaredType = valueElment.GetAttribute("declaredType");
                        if (valueElment.GetAttribute("declaredType") == "double[]" || valueElment.GetAttribute("declaredType") == "double[][]")
                        {
                            string keyValueFilePath = Path.GetDirectoryName(path) + "\\" + scalarVariable.attachFile;
                            scalarVariable.scalarVariableValue.start =
                                IniParser.IniParser.GetParamValue(keyValueFilePath, scalarVariable.name);
                        }
                        else
                        {
                            scalarVariable.scalarVariableValue.start = valueElment.GetAttribute("start");
                        }
                        scalarVariable.scalarVariableValue.unit = valueElment.GetAttribute("unit");
                        scalarVariable.scalarVariableValue.derivative = valueElment.GetAttribute("derivative");
                        scalarVariable.scalarVariableValue.reinit = valueElment.GetAttribute("reinit");
                        scalarVariable.scalarVariableValue.min = valueElment.GetAttribute("min");
                        scalarVariable.scalarVariableValue.max = valueElment.GetAttribute("max");
                    }
                    ScalarVariables.Add(scalarVariable);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void ExportFmiFile(string filePath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath + "\\modelDescription.xml");
                XmlElement root = doc.DocumentElement;
                root.Attributes["fmiVersion"].Value = this.fmiVersion;
                root.Attributes["modelName"].Value = this.modelName;
                root.Attributes["guid"].Value = this.guid;
                root.Attributes["author"].Value = this.author;
                root.Attributes["description"].Value = this.description;
                root.Attributes["version"].Value = this.version;
                root.Attributes["toolId"].Value = this.toolId;
                root.Attributes["toolType"].Value = this.toolType;
                root.Attributes["copyright"].Value = this.copyright;
                root.Attributes["generationTool"].Value = this.generationTool;
                root.Attributes["generationDateAndTime"].Value = this.generationDateAndTime;
                root.Attributes["variableNamingConvention"].Value = this.VariableNamingConvention;

                XmlNode ModelVariablesNode = root.SelectSingleNode("ModelVariables");
                foreach (XmlNode paramNode in ModelVariablesNode.ChildNodes)
                {
                    string value = "";
                    for (int i = 0; i < this.ScalarVariables.Count; i++)
                    {
                        if (paramNode.Attributes["name"].Value == this.ScalarVariables[i].name)
                        {
                            value = this.ScalarVariables[i].scalarVariableValue.start;
                            break;
                        }
                    }
                    foreach (XmlNode paramValueNode in paramNode.ChildNodes)
                    {
                        paramValueNode.Attributes["start"].Value = value;
                    }
                }
                doc.Save(filePath + "\\modelDescription.xml");
            }
            catch (Exception ex)
            {

            }
        }
    }
    

    /// <summary>
    /// 变量模型
    /// </summary>
    public class ScalarVariableModel
    {
        /// <summary>
        /// 变量的标识
        /// </summary>
        public string id;

        /// <summary>
        /// 变量的英文名称
        /// </summary>
        public string name;

        /// <summary>
        /// 变量参考值
        /// </summary>
        public string valueReference;

        /// <summary>
        /// 
        /// </summary>
        public string causality;

        /// <summary>
        /// 变量连续性
        /// </summary>
        public string variability;

        /// <summary>
        /// 变量描述信息
        /// </summary>
        public string description;

        /// <summary>
        /// 变量显示名称
        /// </summary>
        public string displayName;

        /// <summary>
        /// 变量的索引文件
        /// </summary>
        public string attachFile;

        /// <summary>
        /// 变量的版本
        /// </summary>
        public string version;

        /// <summary>
        /// 参数值类
        /// </summary>
        public ScalarVariableValue scalarVariableValue = new ScalarVariableValue();
    }

    /// <summary>
    /// 变量值
    /// </summary>
    public class ScalarVariableValue
    {
        /// <summary>
        /// 变量类型
        /// </summary>
        public string declaredType;

        /// <summary>
        /// 变量初始值
        /// </summary>
        public string start;

        /// <summary>
        /// 单位
        /// </summary>
        public string unit;

        /// <summary>
        /// 索引
        /// </summary>
        public string derivative;

        /// <summary>
        /// 
        /// </summary>
        public string reinit;

        /// <summary>
        /// 最小值
        /// </summary>
        public string min;

        /// <summary>
        /// 最大值
        /// </summary>
        public string max;
    }
}
