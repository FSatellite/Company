using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IniParser
{
    public class IniParser
    {
        /// <summary>
        /// 获取所有参数
        /// </summary>
        /// <returns></returns>
        public static string GetParamValue(string filePath,string paramName)
        {
            try
            {
                List<List<string>> dataList = new List<List<string>>();
                StreamReader sr = new StreamReader(filePath);
                string line = sr.ReadLine().Trim();
                int nFlag = -1;//遇到一个参数后开始计数，到下一个参数时加1
                while (line != null)
                {
                    line = line.Trim();
                    if (nFlag == 0 && !line.Contains("["))
                    {
                        string []array = Regex.Split(line, "\\s+", RegexOptions.IgnoreCase);
                        List<string> list = new List<string>(array);
                        dataList.Add(list);
                    }

                    if (line.Contains(paramName))
                    {
                        nFlag = 0;
                    }
                    else
                    {
                        if (line.Contains("[") && !line.Contains(paramName))
                        {
                            nFlag = -1;
                        }
                    }
                    line = sr.ReadLine();
                }

                List<List<string>> convertList = ListConvert(dataList);
                string paramStr = "";
                for (int i = 0; i < convertList.Count; i++)
                {
                    string tempStr = "";
                    for (int j = 0; j < convertList[i].Count; j++)
                    {
                        if (j == convertList[i].Count - 1)
                        {
                            tempStr += convertList[i][j];
                        }
                        else
                        {
                            tempStr += convertList[i][j] + ",";
                        }
                    }

                    paramStr += tempStr + ";";
                }

                return paramStr.Remove(paramStr.LastIndexOf(';'));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 将参数写入INI文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="paramDict"></param>
        /// ini中数据为5 X 4  paramDict中数据为4 X 5  需要先转置
        public static void WriteParamToFile(string filePath,Dictionary<string,string> paramDict)
        {
            try
            {
                string writeText = "";
                foreach (var item in paramDict)
                {
                    string paramNameText = "[parameter]" + item.Key + "\n";
                    string[] array = item.Value.Split(';');
                    string paramValueText = "";
                    List<List<string>> paramValueList = new List<List<string>>();//保存参数值
                    foreach (var item2 in array)
                    {
                        /*
                         * 当前数据           需要写入数据
                         * 1 2 3 4              1 5 9
                         * 5 6 7 8              2 6 0
                         * 9 0 1 4              3 7 1
                         *                         4 8 4
                         */
                        paramValueList.Add(new List<string>(item2.Split(',')));
                    }

                    List<List<string>> convertParamValueList = ListConvert(paramValueList);

                    for (int i = 0; i < convertParamValueList.Count; i++)
                    {
                        string text = "";
                        for (int j = 0; j < convertParamValueList[i].Count; j++)
                        {
                            text += convertParamValueList[i][j] + " ";
                        }

                        paramValueText += text.Trim() + "\n";
                    }

                    writeText += paramNameText;
                    writeText += paramValueText;
                }

                //写入文件
                FileStream fs = new FileStream(filePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(writeText);
                sw.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// list转置
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        private static List<List<string>> ListConvert(List<List<string>> dataList)
        {
            if (dataList.Count == 0)
            {
                return null;
            }
            List<List<string>> convertList = new List<List<string>>();
            for (int i = 0; i < dataList[0].Count; i++)
            {
                List<string> tempList = new List<string>();
                for (int j = 0; j < dataList.Count; j++)
                {
                    tempList.Add(dataList[j][i]);
                }
                convertList.Add(tempList);
            }

            return convertList;
        }
    }
}
