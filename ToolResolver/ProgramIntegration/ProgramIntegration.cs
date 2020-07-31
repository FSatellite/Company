using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Windows.Forms;
namespace ProgramIntegration
{
    public class ProgramIntegration
    {
        /// <summary>
        /// 程序启动命令
        /// </summary>
        public enum StartCmd
        {
            NEW,//新建
            OPEN//打开
        }

        /// <summary>
        /// 程序启动路径
        /// </summary>
        private string startPath;

        /// <summary>
        /// 组件所在路径
        /// </summary>
        private string filePath;
        
        /// <summary>
        /// 组件启动命令
        /// </summary>
        private StartCmd startCmd;

        /// <summary>
        /// 程序GUID
        /// </summary>
        public string guid;

        public ProgramModel programModel = null;

        public BaseModel baseModel = null;

        public StandardModel standardModel = null;

        public ProgramIntegration() { }
        public ProgramIntegration(string startPath,string filePath,StartCmd cmd,string appGuid = "")
        {
            this.startPath = startPath;
            this.startCmd = cmd;
            this.guid = appGuid;
            this.filePath = filePath;
            baseModel = new BaseModel(this.filePath + "\\" + this.guid + "\\Program.xml");
        }

        /// <summary>
        /// IDE启动
        /// </summary>
        public BaseModel GetProgramModel()
        {
            /*string cmdStr = "";
            if (this.startCmd == StartCmd.NEW)
                cmdStr = "new";
            else
                cmdStr = "open";
            if (cmdStr == "new")
                this.guid = Guid.NewGuid().ToString().ToUpper();

            Process p = new Process();
            //设置要启动的应用程序
            p.StartInfo.FileName = "cmd.exe";
            //是否使用操作系统shell启动
            p.StartInfo.UseShellExecute = false;
            // 接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardInput = true;
            //输出信息
            p.StartInfo.RedirectStandardOutput = true;
            // 输出错误
            p.StartInfo.RedirectStandardError = true;
            //不显示程序窗口
            p.StartInfo.CreateNoWindow = true;
            //启动程序
            p.Start();

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(this.startPath + "\\TSCompilerCfg\\TSCompilerCfg.exe " + cmdStr + " " + this.guid + " " + this.startPath + "\\AppLib &exit");

            p.StandardInput.AutoFlush = true;
            p.StandardError.ReadToEnd();
            p.WaitForExit();
            p.Close();
            */
            if (baseModel.applicationTypeCode == "Caculate")
            {
                this.programModel = new ProgramModel(this.filePath + "\\" + this.guid + "\\Program.xml");
                return this.programModel;
            }
            else
            {
                this.standardModel = new StandardModel(this.filePath + "\\" + this.guid + "\\Program.xml");
                return this.standardModel;
            }
            //this.programModel = new ProgramModel(this.filePath + "\\" + this.guid + "\\Program.xml");
            ////读取组件XML
            //return this.programModel;
        }

        /// <summary>
        /// 运行组件
        /// </summary>
        /// <param name="dict"></param>
        public Dictionary<string,string> AppRun(Dictionary<string,string> dict)
        {
            Dictionary<string, string> resultDict = new Dictionary<string, string>();
            if (baseModel.applicationTypeCode == "Caculate")
            {
                string exePath = this.startPath + "\\AppExcuter\\AppExcuter.exe";
                string xmlPath = this.filePath + "\\" + this.guid + "\\Program.xml";
                string parasPath = this.filePath + "\\" + this.guid + "\\paras.in";
                string resultPath = this.filePath + "\\" + this.guid + "\\result.out";

                //更新xml文件数据
                UpdateXml(dict);

                this.programModel = new ProgramModel(this.filePath + "\\" + this.guid + "\\Program.xml");

                //将输入参数写入paras.in文件中
                WriteParasFile(parasPath);

                //调用AppExcuter进行计算
                Process p = new Process();
                p.StartInfo.WorkingDirectory = this.startPath + "\\AppExcuter";
                //p.StartInfo.WorkingDirectory = @"G:\AppProgram\AppExcuter\AppExcuter\bin\Release\";
                p.StartInfo.FileName = "AppExcuter.exe";
                p.StartInfo.Arguments = " " + xmlPath + " " + parasPath + " " + resultPath;
                p.Start();
                p.WaitForExit();
                
                //获取计算结果值
                ReadReultFile(resultPath, resultDict);
                p.Close();
            }
            else //StandardModel
            {
                UpdateStandardModel(dict);

                string exePath = this.filePath + "\\" + this.guid + "\\Runfile\\";
                if (Environment.Is64BitOperatingSystem == true)
                    exePath += "x64\\Program.exe";
                else
                    exePath += "x86\\Program.exe";
                Process p = new Process();
                p.StartInfo.FileName = exePath;
                p.Start();
                p.WaitForExit();

                this.standardModel.GetAllParam();
                ReadStandardResult(resultDict);
                p.Close();
            }
           
            return resultDict;
        }

        /// <summary>
        /// 更新标准模型的输入文件
        /// </summary>
        /// <param name="dict"></param>
        public void UpdateStandardModel(Dictionary<string, string> dict)
        {
            //清空原始数据
            foreach (var paramModel in this.standardModel.paramInDict)
                paramModel.Value.paramValueList.Clear();

            foreach (var param in dict)
            {
                if (this.standardModel.paramInDict.ContainsKey(param.Key))
                {
                    if (this.standardModel.paramInDict[param.Key].paramModel == "##rowModel")
                    {
                        this.standardModel.paramInDict[param.Key].paramValue = param.Value;
                    }
                    else
                    {
                        string[] array = param.Value.Split(';');
                        foreach (var paramValue in array)
                            this.standardModel.paramInDict[param.Key].paramValueList.Add(paramValue.Split(',').ToList());
                    }
                }
            }
            this.standardModel.WriteParamsToFile();
        }

        /// <summary>
        /// 读取标准模型结果
        /// </summary>
        public void ReadStandardResult(Dictionary<string,string> dict)
        {
            //将标准模型参数转化为dict
            foreach (var paramModel in this.standardModel.paramOutList)
            {
                if (paramModel.paramModel == "##rowModel")
                    dict.Add(paramModel.paramName,paramModel.paramValue);
                else
                {
                    List<string> tempList = new List<string>();
                    for (int j = 0; j < paramModel.paramValueList.Count; j++)
                    {
                        string temp = string.Join(",", paramModel.paramValueList[j].ToArray());
                        tempList.Add(temp);
                    }
                    dict.Add(paramModel.paramName,string.Join(";",tempList.ToArray()));
                }
            }
        }

        /// <summary>
        /// 获取已有的关联关系
        /// </summary>
        public void GetRelationship(Dictionary<string,string> dict)
        {
            if (File.Exists(this.filePath + "\\Relationship.xml"))
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(this.filePath + "\\Relationship.xml");
                    XmlElement root = doc.DocumentElement;
                    foreach (XmlElement paramNode in root.ChildNodes)
                    {
                        dict.Add(paramNode.GetAttribute("TaskParam"), paramNode.GetAttribute("ToolParam"));
                    }
                }
                catch (Exception e)
                {

                }
                
            }
        }

        public void GetRelationship(string filePath,Dictionary<string, string> dict)
        {
            if (File.Exists(filePath + "\\Relationship.xml"))
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filePath + "\\Relationship.xml");
                    XmlElement root = doc.DocumentElement;
                    foreach (XmlElement paramNode in root.ChildNodes)
                    {
                        dict.Add(paramNode.GetAttribute("TaskParam"), paramNode.GetAttribute("ToolParam"));
                    }
                }
                catch (Exception e)
                {

                }

            }
        }

        /// <summary>
        /// 写paras.in文件
        /// </summary>
        public void WriteParasFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                FileStream fs = new FileStream(filePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                int nParamNum = 0;//记录参数个数
                for (int i = 0; i < this.programModel.modelVariables.Count; i++)
                {
                    if (this.programModel.modelVariables[i].dataFlow == "input")
                    {
                        nParamNum++;
                    }
                }
                sw.WriteLine(nParamNum + " variables\n");
                for (int i = 0; i < this.programModel.modelVariables.Count; i++)
                {
                    if (this.programModel.modelVariables[i].dataFlow == "input")
                    {
                        sw.WriteLine(this.programModel.modelVariables[i].valueReference + " " + this.programModel.modelVariables[i].name);
                    }
                }
                sw.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                
            }
        }
                                                                                                                                            
        /// <summary>
        /// 读取结果文件并更新xml
        /// </summary>
        /// <param name="filePath"></param>
        public void ReadReultFile(string filePath,Dictionary<string,string> dict)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    StreamReader sr = new StreamReader(filePath);
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        line = line.Replace("\t","");
                        string[] strArray = Regex.Split(line, "\\s+", RegexOptions.IgnoreCase);
                        dict.Add(strArray[1], strArray[0]);
                        line = sr.ReadLine();
                    }
                    sr.Close();
                    UpdateXml(dict);
                }
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// 更新xml
        /// </summary>
        /// <param name="dict"></param>
        public void UpdateXml(Dictionary<string, string> dict)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(this.filePath + "\\" + this.guid + "\\Program.xml");
                XmlElement root = doc.DocumentElement;
                XmlNode ModelVariablesNode = root.SelectSingleNode("ModelVariables");
                foreach (XmlNode paramNode in ModelVariablesNode.ChildNodes)
                {
                    XmlElement paramElement = (XmlElement)paramNode;
                    paramElement.SetAttribute("valueReference", dict[paramElement.GetAttribute("name")]);
                }
                doc.Save(this.filePath + "\\" + this.guid + "\\Program.xml");
            }
            catch (Exception e)
            {

            }
        }
    }
}
