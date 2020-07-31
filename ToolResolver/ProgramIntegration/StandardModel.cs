using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ProgramIntegration
{
    public class StandardModel : BaseModel
    {
        public StandardModel(string xmlPath)
        {
            this.paramFilePath = Path.GetDirectoryName(xmlPath);
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

                GetAllParam();
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// 获取所有参数
        /// </summary>
        public void GetAllParam()
        {
            this.paramInList.Clear();
            this.paramOutList.Clear();

            string paramInFilePath = this.paramFilePath + "\\Data\\";
            DirectoryInfo folder = new DirectoryInfo(paramInFilePath);
            //遍历该文件夹下所有后缀为.in的文件
            foreach (FileInfo file in folder.GetFiles())
            {
                if (file.Extension == ".in")
                {
                    ParseFile(file.FullName,ref this.paramInList);
                }

                if (file.Extension == ".out")
                {
                    ParseFile(file.FullName,ref this.paramOutList);
                }
            }
            ListToDict();
        }

        /// <summary>
        /// 将参数List转为Dict，方便修改参数值
        /// </summary>
        private void ListToDict()
        {
            this.paramInDict.Clear();
            this.paramOutDict.Clear();
            foreach (var paramModel in this.paramInList)
            {
                this.paramInDict.Add(paramModel.paramName,paramModel);
            }
            foreach (var paramModel in this.paramOutList)
            {
                this.paramInDict.Add(paramModel.paramName, paramModel);
            }
        }

        /// <summary>
        /// 解析参数文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="list">参数列表</param>
        private void ParseFile(string filePath,ref List<StandardModelParam> list)
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
                                standardModelParam = new StandardModelParam(array[0],array[1],array[2],paramModel,array[4],array[3],Path.GetFileName(filePath));
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

                }
            }
        }

        public void WriteParamsToFile()
        {
            string paramInOutFilePath = this.paramFilePath + "\\Data\\";
            foreach (var paramModel in paramInDict)
            {
                WriteParamToFile(paramInOutFilePath,paramModel.Value);
            }

            foreach (var paramModel in paramOutDict)
            {
                WriteParamToFile(paramInOutFilePath, paramModel.Value);
            }
        }

        /// <summary>
        /// 将修改后的参数写入文件
        /// </summary>
        private void WriteParamToFile(string filePath,StandardModelParam paramModel)
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
                        if (Regex.Split(line,"\\s+",RegexOptions.IgnoreCase)[0] == paramModel.paramName)
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
                using (FileStream fs = new FileStream(paramFilePath,FileMode.Create))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(stringBuilder);
                    sw.Close();
                }
            }
        }

        /// <summary>
        /// 参数文件路径
        /// </summary>
        private string paramFilePath;

        /// <summary>
        /// 输入参数
        /// </summary>
        public List<StandardModelParam> paramInList = new List<StandardModelParam>();
        public Dictionary<string,StandardModelParam> paramInDict = new Dictionary<string, StandardModelParam>();

        /// <summary>
        /// 输出参数
        /// </summary>
        public List<StandardModelParam> paramOutList = new List<StandardModelParam>();
        public Dictionary<string,StandardModelParam> paramOutDict = new Dictionary<string, StandardModelParam>();
    }

    public class StandardModelParam
    {
        public StandardModelParam(){}
        public StandardModelParam(string _paramName, string _paramDescription, string _paramValue, string _paramModel,string _paramUnit, string _paramType, string _paramFile)
        {
            this.paramName = _paramName;
            this.paramDescription = _paramDescription;
            this.paramValue = _paramValue;
            this.paramModel = _paramModel;
            this.paramUnit = _paramUnit;
            this.paramType = _paramType;
            this.paramFile = _paramFile;
        }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string paramName;

        /// <summary>
        /// 参数描述
        /// </summary>
        public string paramDescription;

        /// <summary>
        /// 参数值
        /// </summary>
        public string paramValue;

        /// <summary>
        /// 参数块名称
        /// </summary>
        public string paramModel;

        /// <summary>
        /// 参数维度
        /// </summary>
        public string paramMatrix;

        /// <summary>
        /// 参数单位
        /// </summary>
        public string paramUnit;

        /// <summary>
        /// 参数类型
        /// </summary>
        public string paramType;

        /// <summary>
        /// 参数所在文件
        /// </summary>
        public string paramFile;

        public List<List<string>> paramValueList = new List<List<string>>();
    }
}
