using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;

namespace AppTools.util
{
    class GloabConfig
    {
        public static string serverUrl = "https://www.baidu.com";
        public static bool devModel = false;
        public static int width = 250;
        public static int height = 700;
        public static string knowledgeAppUrl = "http://localhost:8020/";

        public static void readConfigFile()
        {
            try
            {
                string filePath = Application.StartupPath + "/system.config";
                if (!File.Exists(filePath))
                {
                    return;
                }

                double workWidth = SystemInformation.WorkingArea.Width; // 屏幕工作区域宽度
                double workHeight = SystemInformation.WorkingArea.Height; // 屏幕工作区域高度
                width = (int)(workWidth * 0.2);   // 设置窗体宽度
                height = (int)(workHeight * 0.8); // 设置窗体高度
                string line;
                using (System.IO.StreamReader sr = new System.IO.StreamReader(filePath))
                {
                    // 从文件读取并显示行，直到文件的末尾 
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains("serverUrl"))
                        {
                            if (line.Contains("../"))
                            {
                                //serverUrl = AppDomain.CurrentDomain.BaseDirectory + "webSource/AppToolsClient";
                                serverUrl = System.Windows.Forms.Application.StartupPath + "\\webSource/AppToolsClient";
                                serverUrl = serverUrl.Replace("\\", "/");
                            }
                            else {
                                serverUrl = Regex.Split(line, "=", RegexOptions.IgnoreCase)[1];
                            }
                        }
                        else if (line.Contains("devModel"))
                        {
                            devModel = bool.Parse(Regex.Split(line, "=", RegexOptions.IgnoreCase)[1]);
                        }
                        else if (line.Contains("width"))
                        {
                            if (line.Contains("%"))
                            {
                                string w=Regex.Split(line, "=", RegexOptions.IgnoreCase)[1];
                                width = (int)(double.Parse(w.Replace("%", "")) / 100 * workWidth);
                            }
                            else {
                                width = int.Parse(Regex.Split(line, "=", RegexOptions.IgnoreCase)[1]);
                            }
                        }else if(line.Contains("height")){
                            if (line.Contains("%"))
                            {
                                string h = Regex.Split(line, "=", RegexOptions.IgnoreCase)[1];
                                height = (int)(double.Parse(h.Replace("%", "")) / 100 * (int)workHeight);
                            }
                            else
                            {
                                height = int.Parse(Regex.Split(line, "=", RegexOptions.IgnoreCase)[1]);
                            }
                        }
                        else if (line.Contains("knowledgeAppUrl"))
                        {
                            knowledgeAppUrl = Regex.Split(line, "=", RegexOptions.IgnoreCase)[1];                           
                        }
                    }
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                string exce = "";
                exce += ex.Message + "\r\n";
                exce += ex.Source + "\r\n";
                exce += ex.StackTrace + "\r\n";
                exce += ex.TargetSite + "\r\n";
                exce += ex.Data + "\r\n";

                MessageBox.Show(null, exce, "运行错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
