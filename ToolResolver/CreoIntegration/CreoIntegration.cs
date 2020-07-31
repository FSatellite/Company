using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using pfcls;

namespace CreoIntegration
{
    public class CreoIntegration
    {
        CCpfcAsyncConnection creoConnection;
        IpfcAsyncConnection asyncConncerion;//creo进程
        IpfcBaseSession session;

        public string modelPath;
        public string modelName;//模型名称
        public string tempModelPath;//临时模型路径
        public IpfcModel model;
        //public List<CreoParamModel> creoParamModelList = new List<CreoParamModel>();

        public CreoIntegration(string modelPath,string creoPath)
        {
            this.modelPath = modelPath;
            CreoStart(creoPath);
        }

        /// <summary>
        /// Creo启动
        /// </summary>
        /// <returns></returns>
        public bool CreoStart(string creoPath)
        {
            try
            {
                creoConnection = new CCpfcAsyncConnection();
                asyncConncerion = creoConnection.Start(creoPath + " -g:no_graphics -i:rpc_input", null);
                //asyncConncerion.WaitForEvents();
            }
            catch (Exception e)
            {
                CreoStop();
            }
            if (asyncConncerion.IsRunning())
                return true;
            else
                return false;
        }

        /// <summary>
        /// Creo停止
        /// </summary>
        public void CreoStop()
        {
            asyncConncerion.End();
        }

        /// <summary>
        /// 获取模型参数
        /// </summary>
        public CreoModel GetCreoModel()
        {
            try
            {
                CreoModel creoModel = new CreoModel();
                string []array = this.modelPath.Split('\\');
                creoModel.modelName = array[array.Length - 1];

                LoadModel();

                IpfcParameterOwner iParameterOwner = (IpfcParameterOwner)model;
                IpfcParameters paramList = iParameterOwner.ListParams();
                for (int i = 0; i < paramList.Count; i++)
                {
                    //参数描述
                    string paramDescription = "";
                    if (((object)paramList[i].Description).GetType().FullName.IndexOf("System.DBNull") < 0)
                        paramDescription = paramList[i].Description;

                    string paramUnit = "";
                    if (paramList[i].Units != null)
                    {
                        paramUnit = paramList[i].Units.Name;
                    }

                    //参数名称
                    IpfcNamedModelItem nameModelItem = (IpfcNamedModelItem)paramList[i];
                    string paramName = nameModelItem.Name;

                    //参数值
                    IpfcParamValue ipfcParamValue = (IpfcParamValue)paramList[i].GetScaledValue();
                    string paramType = GetParamType(ipfcParamValue.discr);
                    string paramValue = "";
                    if (paramType == "int")
                    {
                        paramValue = ipfcParamValue.IntValue.ToString();

                        CreoParamModel creoParamModel = new CreoParamModel();
                        creoParamModel.paramName = paramName;
                        creoParamModel.paramDisplayName = paramDescription;
                        creoParamModel.paramValue = paramValue;
                        creoParamModel.paramValueType = paramType;
                        creoParamModel.paramUnit = paramUnit;
                        creoModel.paramList.Add(creoParamModel);
                    }
                    if (paramType == "double")
                    {
                        paramValue = ipfcParamValue.DoubleValue.ToString();

                        CreoParamModel creoParamModel = new CreoParamModel();
                        creoParamModel.paramName = paramName;
                        creoParamModel.paramDisplayName = paramDescription;
                        creoParamModel.paramValue = paramValue;
                        creoParamModel.paramValueType = paramType;
                        creoParamModel.paramUnit = paramUnit;
                        creoModel.paramList.Add(creoParamModel);
                    }
                    if (paramType == "string")
                    {
                        paramValue = ipfcParamValue.StringValue.ToString();
                    }
                    if (paramType == "bool")
                    {
                        paramValue = ipfcParamValue.BoolValue.ToString();
                    }
                }

                return creoModel;
            }
            catch (Exception e)
            {
                CreoStop();
            }
            return null;
        }

        public Dictionary<string, string> GetModelParam()
        {
            Dictionary<string,string> resultDict = new Dictionary<string,string>();
            IpfcParameterOwner iParameterOwner = (IpfcParameterOwner)model;
            IpfcParameters paramList = iParameterOwner.ListParams();
            for (int i = 0; i < paramList.Count; i++)
            {
                //参数描述
                string paramDescription = "";
                if (((object)paramList[i].Description).GetType().FullName.IndexOf("System.DBNull") < 0)
                    paramDescription = paramList[i].Description;

                string paramUnit = "";
                if (paramList[i].Units != null)
                {
                    paramUnit = paramList[i].Units.Name;
                }

                //参数名称
                IpfcNamedModelItem nameModelItem = (IpfcNamedModelItem)paramList[i];
                string paramName = nameModelItem.Name;

                //参数值
                IpfcParamValue ipfcParamValue = (IpfcParamValue)paramList[i].GetScaledValue();
                string paramType = GetParamType(ipfcParamValue.discr);
                string paramValue = "";
                if (paramType == "int")
                {
                    paramValue = ipfcParamValue.IntValue.ToString();

                    resultDict.Add(paramName, paramValue);
                }
                if (paramType == "double")
                {
                    paramValue = ipfcParamValue.DoubleValue.ToString();

                    resultDict.Add(paramName, paramValue);
                }
                if (paramType == "string")
                {
                    paramValue = ipfcParamValue.StringValue.ToString();
                }
                if (paramType == "bool")
                {
                    paramValue = ipfcParamValue.BoolValue.ToString();
                }
            }
            return resultDict;
        }

        public string GetParamType(int nType)
        {
            if (nType == 0)
                return "string";
            if (nType == 1)
                return "int";
            if (nType == 2)
                return "bool";
            if (nType == 3)
                return "double";
            return "";
        }

        public void LoadModel()
        {
            try
            {
                //加载模型时，由于文件夹名称带有GUID加载失败，需要将模型复制到临时文件夹
                string[] array = this.modelPath.Split('\\');
                this.tempModelPath = "C:\\tempMdl\\" + array[array.Length - 1];
                if (Directory.Exists("C:\\tempMdl"))
                    File.Copy(this.modelPath, tempModelPath, true);
                else
                {
                    Directory.CreateDirectory("C:\\tempMdl");
                    File.Copy(this.modelPath, tempModelPath, true);
                }

                IpfcModelDescriptor modelDesc;
                IpfcRetrieveModelOptions retrieveModelOptions;
                retrieveModelOptions = (new CCpfcRetrieveModelOptions()).Create();
                retrieveModelOptions.AskUserAboutReps = false;
                //加载零件
                session = asyncConncerion.Session as IpfcBaseSession;
                modelDesc = (new CCpfcModelDescriptor()).Create((int)EpfcModelType.EpfcMDL_PART, tempModelPath, "");
                model = session.RetrieveModel(modelDesc);
            }
            catch (Exception)
            {
                CreoStop();
            }
        }

        public Dictionary<string,string> RegenerateModel(Dictionary<string,string> paramDict)
        {
            try
            {
                IpfcParameterOwner iParameterOwner = (IpfcParameterOwner)model;
                IpfcParameters paramList = iParameterOwner.ListParams();
                for (int i = 0; i < paramList.Count; i++)
                {
                    IpfcNamedModelItem nameModelItem = (IpfcNamedModelItem)paramList[i];
                    string paramName = nameModelItem.Name;

                    //参数值
                    IpfcParamValue ipfcParamValue = (IpfcParamValue)paramList[i].GetScaledValue();
                    string paramType = GetParamType(ipfcParamValue.discr);
                    if (paramType == "int")
                    {
                        if (paramDict.ContainsKey(paramName))
                            ipfcParamValue.IntValue = Convert.ToInt32(paramDict[paramName]);
                    }
                    if (paramType == "double")
                    {
                        if (paramDict.ContainsKey(paramName))
                            ipfcParamValue.DoubleValue = Convert.ToDouble(paramDict[paramName]);
                    }
                    if (paramType == "string")
                    {
                        if (paramDict.ContainsKey(paramName))
                            ipfcParamValue.StringValue = paramDict[paramName];
                    }
                    if (paramType == "bool")
                    {
                        if (paramDict.ContainsKey(paramName))
                            ipfcParamValue.BoolValue = Convert.ToBoolean(paramDict[paramName]);
                    }
                    paramList[i].SetScaledValue(ipfcParamValue, null);
                }
                IpfcSolid solid = (IpfcSolid)this.model;
                solid.Regenerate(null);

                //删除临时文件
                DelectDir("C:\\tempMdl");

                return GetModelParam();
            }
            catch (Exception e)
            {
                CreoStop();
                return null;
            }
        }

        public void DelectDir(string srcPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        //如果 使用了 streamreader 在删除前 必须先关闭流 ，否则无法删除 sr.close();
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
                Directory.Delete(srcPath);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }

    public class CreoModel
    {
        public string modelName;
        public List<CreoParamModel> paramList = new List<CreoParamModel>();
    }

    public class CreoParamModel
    {
        public string paramName;//参数名称
        public string paramDisplayName;//参数显示名称
        public string paramValue;//参数描述
        public string paramValueType;//参数值类型
        public string paramUnit;//参数单位
    }
}
