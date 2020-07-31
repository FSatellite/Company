using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ComponentConvert
{
    public class StandardModelController
    {
        /// <summary>
        /// 参数文件路径
        /// </summary>
        private string _paramFilePath;

        /// <summary>
        /// 标准模型
        /// </summary>
        private StandardModel _standardModel = new StandardModel();

        public StandardModelController(string xmlPath)
        {
            this._paramFilePath = xmlPath;
        }

        public StandardModel GetStandardModel()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_paramFilePath);
                XmlElement root = doc.DocumentElement;

                //获取小程序属性
                _standardModel.fmiVersion = root.GetAttribute("fmiVersion");
                _standardModel.modelName = root.GetAttribute("modelName");
                _standardModel.guid = root.GetAttribute("guid");
                _standardModel.description = root.GetAttribute("description");
                _standardModel.author = root.GetAttribute("author");
                _standardModel.version = root.GetAttribute("version");
                _standardModel.copyright = root.GetAttribute("copyright");
                _standardModel.license = root.GetAttribute("license");
                _standardModel.generationTool = root.GetAttribute("generationTool");
                _standardModel.generationDateAndTime = root.GetAttribute("generationDateAndTime");
                _standardModel.VariableNamingConvention = root.GetAttribute("VariableNamingConvention");
                _standardModel.toolType = root.GetAttribute("toolType");
                _standardModel.toolId = root.GetAttribute("toolId");

                GetAllParam();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return _standardModel;
        }

        /// <summary>
        /// 获取所有参数
        /// </summary>
        public void GetAllParam()
        {
            _standardModel.paramInList.Clear();
            _standardModel.paramOutList.Clear();

            string paramInFilePath = this._paramFilePath + "\\Data\\";
            DirectoryInfo folder = new DirectoryInfo(paramInFilePath);
            //遍历该文件夹下所有后缀为.in的文件
            foreach (FileInfo file in folder.GetFiles())
            {
                if (file.Extension == ".in")
                {
                    ParseFile(file.FullName, ref _standardModel.paramInList);
                }

                if (file.Extension == ".out")
                {
                    ParseFile(file.FullName, ref _standardModel.paramOutList);
                }
            }
            ListToDict();
        }

        /// <summary>
        /// 将参数List转为Dict，方便修改参数值
        /// </summary>
        private void ListToDict()
        {
            _standardModel.paramInDict.Clear();
            _standardModel.paramOutDict.Clear();
            foreach (var paramModel in _standardModel.paramInList)
            {
                _standardModel.paramInDict.Add(paramModel.paramName, paramModel);
            }
            foreach (var paramModel in _standardModel.paramOutList)
            {
                _standardModel.paramInDict.Add(paramModel.paramName, paramModel);
            }
        }

        /// <summary>
        /// 解析参数文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="list">参数列表</param>
        private void ParseFile(string filePath, ref List<StandardModelParam> list)
        {
            if (!File.Exists(filePath))
            {
                return;
            }
            using (StreamReader sr = new StreamReader(filePath))
            {
                try
                {
                    string line = sr.ReadLine();
                    int nFlag = 0;//标记是否遇到参数块
                    string paramModel = "";
                    StandardModelParam standardModelParam = null;
                    while (line != null)
                    {
                        //遇到参数块时，nFlag++
                        if (line == "##rowModel" || line == "##matrixModel" || line == "##structModel")
                        {
                            if (standardModelParam != null)
                            {
                                list.Add(standardModelParam);
                                standardModelParam = null;
                            }

                            paramModel = line;
                            nFlag++;
                            line = sr.ReadLine();

                            continue;
                        }

                        if (line == "" || line.IndexOf(@"//") == 0)
                        {
                            if (standardModelParam != null)
                            {
                                list.Add(standardModelParam);
                                standardModelParam = null;
                            }

                            nFlag = 0;
                            paramModel = "";
                            line = sr.ReadLine();

                            continue;
                        }

                        //单变量
                        if (paramModel == "##rowModel")
                        {
                            if (nFlag > 1)
                            {
                                string[] array = Regex.Split(line, "\\s+", RegexOptions.IgnoreCase);
                                //standardModelParam = new StandardModelParam(array[0], array[1], array[2], paramModel, array[4], array[3], Path.GetFileName(filePath));
                                list.Add(standardModelParam);
                                standardModelParam = null;
                            }
                        }

                        //数组变量 | 结构数组
                        if (paramModel == "##matrixModel" || paramModel == "##structModel")
                        {
                            if (nFlag == 2)
                            {
                                standardModelParam = new StandardModelParam();
                                string[] array = Regex.Split(line, "\\s+", RegexOptions.IgnoreCase);
                                standardModelParam.paramName = array[0];
                                standardModelParam.paramDescription = array[1];
                                standardModelParam.paramType = array[2];
                                standardModelParam.paramUnit = array[3];
                                standardModelParam.paramMatrix = array.Length == 5 ? array[4] : "";
                                standardModelParam.paramModel = paramModel;
                                standardModelParam.paramFile = Path.GetFileName(filePath);
                            }
                            if (nFlag > 2)
                            {
                                string[] array = Regex.Split(line, "\\s+", RegexOptions.IgnoreCase);

                                //开辟空间
                                if (standardModelParam.paramValueList.Count == 0)
                                    for (int i = 0; i < array.Length; i++)
                                        standardModelParam.paramValueList.Add(new List<string>());

                                for (int i = 0; i < array.Length; i++)
                                    standardModelParam.paramValueList[i].Add(array[i]);
                            }
                        }

                        nFlag++;
                        line = sr.ReadLine();
                    }
                    //添加最后的数组参数
                    if ((paramModel == "##matrixModel" || paramModel == "##structModel") && standardModelParam != null)
                    {
                        list.Add(standardModelParam);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void WriteParamsToFile()
        {
            string paramInOutFilePath = this._paramFilePath + "\\Data\\";
            foreach (var paramModel in _standardModel.paramInDict)
            {
                WriteParamToFile(paramInOutFilePath, paramModel.Value);
            }

            foreach (var paramModel in _standardModel.paramOutDict)
            {
                WriteParamToFile(paramInOutFilePath, paramModel.Value);
            }
        }

        /// <summary>
        /// 将修改后的参数写入文件
        /// </summary>
        private void WriteParamToFile(string filePath, StandardModelParam paramModel)
        {
            string paramFilePath = filePath + paramModel.paramFile;
            if (File.Exists(paramFilePath))
            {
                StringBuilder stringBuilder = new StringBuilder();
                using (StreamReader sr = new StreamReader(paramFilePath))
                {
                    string line = sr.ReadLine();
                    int nFlag = 0;
                    while (line != null)
                    {
                        if (Regex.Split(line, "\\s+", RegexOptions.IgnoreCase)[0] == paramModel.paramName)
                        {
                            nFlag = paramModel.paramValueList.Count != 0 ? paramModel.paramValueList[0].Count : 0;
                            if (paramModel.paramModel == "##rowModel")
                            {
                                string[] array = Regex.Split(line, "\\s+", RegexOptions.IgnoreCase);
                                array[2] = paramModel.paramValue;
                                stringBuilder.AppendLine(string.Join(" ", array));
                            }
                            else
                            {
                                stringBuilder.AppendLine(line);
                                for (int i = 0; i < paramModel.paramValueList[0].Count; i++)
                                {
                                    string temp = "";
                                    for (int j = 0; j < paramModel.paramValueList.Count; j++)
                                    {
                                        temp += temp == ""
                                            ? paramModel.paramValueList[j][i]
                                            : " " + paramModel.paramValueList[j][i];
                                    }

                                    stringBuilder.AppendLine(temp);
                                }
                            }
                        }
                        else
                        {
                            if (nFlag <= 0)
                            {
                                stringBuilder.AppendLine(line);
                            }
                            nFlag--;//数组数据占位
                        }

                        line = sr.ReadLine();
                    }
                }

                //写入文件
                using (FileStream fs = new FileStream(paramFilePath, FileMode.Create))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(stringBuilder);
                    sw.Close();
                }
            }
        }

        public void WriteXml(string xmlPath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));

                //根节点
                XmlElement xmlRoot = doc.CreateElement("fmiModelDescription");
                xmlRoot.SetAttribute("fmiVersion",_standardModel.fmiVersion);
                xmlRoot.SetAttribute("modelName", _standardModel.modelName);
                xmlRoot.SetAttribute("guid", _standardModel.guid);
                xmlRoot.SetAttribute("description", _standardModel.description);
                xmlRoot.SetAttribute("author", _standardModel.author);
                xmlRoot.SetAttribute("version", _standardModel.version);
                xmlRoot.SetAttribute("copyright", _standardModel.copyright);
                xmlRoot.SetAttribute("license", _standardModel.license);
                xmlRoot.SetAttribute("generationTool", _standardModel.generationTool);
                xmlRoot.SetAttribute("generationDateAndTime", _standardModel.generationDateAndTime);
                xmlRoot.SetAttribute("VariableNamingConvention", _standardModel.VariableNamingConvention);
                xmlRoot.SetAttribute("toolType", _standardModel.toolType);
                xmlRoot.SetAttribute("toolId", _standardModel.toolId);

                XmlElement paramNodes = doc.CreateElement("ModelVariables");
                xmlRoot.AppendChild(paramNodes);

                foreach (var param in _standardModel.paramInList)
                {
                    //参数节点
                    XmlElement paramNode = doc.CreateElement("ScalarVariable");
                    paramNode.SetAttribute("id", Guid.NewGuid().ToString());
                    paramNode.SetAttribute("name", param.paramName);
                    paramNode.SetAttribute("valueReference","");
                    paramNode.SetAttribute("causality", "");
                    paramNode.SetAttribute("description", "");
                    paramNode.SetAttribute("displayName", param.paramDescription);
                    paramNode.SetAttribute("attachFile", param.paramFile);
                    paramNode.SetAttribute("version", "");

                    //参数值节点
                    XmlElement paramValueNode = doc.CreateElement(ParamType.TypeConvert(param.paramType));
                    paramValueNode.SetAttribute("start", param.paramValue);
                    paramValueNode.SetAttribute("declaredType", param.paramType);
                    paramValueNode.SetAttribute("unit", param.paramUnit);
                    paramValueNode.SetAttribute("derivative", "");
                    paramValueNode.SetAttribute("reinit", "");
                    paramValueNode.SetAttribute("min", "");
                    paramValueNode.SetAttribute("max", "");

                    paramNode.AppendChild(paramValueNode);
                    paramNodes.AppendChild(paramNode);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
